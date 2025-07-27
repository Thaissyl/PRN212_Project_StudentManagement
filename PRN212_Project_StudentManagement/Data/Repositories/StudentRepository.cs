using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.DTOs;
using PRN212_Project_StudentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace PRN212_Project_StudentManagement.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        public IEnumerable<StudentDTO> GetAllStudents()
        {
            using (var context = new DBContext())
            {
                var studentData = from s in context.Students
                                  join u in context.Users on s.UserId equals u.UserId
                                  join c in context.Classes on s.ClassId equals c.ClassId into classJoin
                                  from c in classJoin.DefaultIfEmpty()
                                  select new StudentDTO
                                  {
                                      StudentID = s.StudentId,
                                      FullName = u.FullName,
                                      Email = u.Email,
                                      ClassName = c != null ? c.ClassName : "No Class Assigned",
                                      EnrollmentDate = s.EnrollmentDate
                                  };
                return studentData.ToList();
            }
        }

        public void UpdateStudent(StudentDTO student)
        {
            using (var context = new DBContext())
            {
                var studentToUpdate = context.Students.Find(student.StudentID);
                if (studentToUpdate != null)
                {
                    var userToUpdate = context.Users.Find(studentToUpdate.UserId);
                    if (userToUpdate != null)
                    {
                        userToUpdate.FullName = student.FullName;
                        userToUpdate.Email = student.Email;
                    }

                    var classToUpdate = context.Classes.FirstOrDefault(c => c.ClassName == student.ClassName);
                    if (classToUpdate != null)
                    {
                        studentToUpdate.ClassId = classToUpdate.ClassId;
                    }

                    studentToUpdate.EnrollmentDate = student.EnrollmentDate;
                    context.SaveChanges();
                }
            }
        }

        public IEnumerable<string> GetAllClassNames()
        {
            using (var context = new DBContext())
            {
                return context.Classes.Select(c => c.ClassName).ToList();
            }
        }

        public void AddStudent(StudentDTO student)
        {
            using (var context = new DBContext())
            {
                var user = new User
                {
                    FullName = student.FullName,
                    Email = student.Email,
                    Password = "password", // Set a default password
                    Role = "Student",
                    IsActive = true
                };
                context.Users.Add(user);
                context.SaveChanges();

                var classEntity = context.Classes.FirstOrDefault(c => c.ClassName == student.ClassName);
                if (classEntity == null)
                {
                    throw new InvalidOperationException("Class not found.");
                }

                var newStudent = new Student
                {
                    UserId = user.UserId,
                    ClassId = classEntity.ClassId,
                    EnrollmentDate = student.EnrollmentDate
                };

                context.Students.Add(newStudent);
                context.SaveChanges();
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var context = new DBContext())
            {
                var studentToDelete = context.Students.Find(studentId);
                if (studentToDelete != null)
                {
                    var userToDelete = context.Users.Find(studentToDelete.UserId);
                    if (userToDelete != null)
                    {
                        context.Users.Remove(userToDelete);
                    }
                    context.Students.Remove(studentToDelete);
                    context.SaveChanges();
                }
            }
        }

        public IEnumerable<StudentDTO> GetUnassignedStudents()
        {
            using (var context = new DBContext())
            {
                // Get users with role "Student" who don't have corresponding Student records
                var unassignedStudents = from u in context.Users
                                        where u.Role == "Student" && 
                                              !context.Students.Any(s => s.UserId == u.UserId)
                                        select new StudentDTO
                                        {
                                            StudentID = u.UserId, // Use UserId as StudentID for unassigned students
                                            FullName = u.FullName,
                                            Email = u.Email,
                                            ClassName = "No Class Assigned",
                                            EnrollmentDate = DateOnly.FromDateTime(DateTime.Now)
                                        };
                return unassignedStudents.ToList();
            }
        }

        public void AssignStudentToClass(int studentId, string className)
        {
            using (var context = new DBContext())
            {
                var classEntity = context.Classes.FirstOrDefault(c => c.ClassName == className);
                if (classEntity == null)
                {
                    throw new InvalidOperationException("Class not found.");
                }

                // For unassigned students, studentId is actually the UserId
                // Check if this is an existing student or a new one
                var existingStudent = context.Students.FirstOrDefault(s => s.StudentId == studentId);
                
                if (existingStudent != null)
                {
                    // Update existing student's class
                    existingStudent.ClassId = classEntity.ClassId;
                }
                else
                {
                    // Create new student record
                    var user = context.Users.FirstOrDefault(u => u.UserId == studentId && u.Role == "Student");
                    if (user == null)
                    {
                        throw new InvalidOperationException("Student user not found.");
                    }

                    var newStudent = new Student
                    {
                        UserId = user.UserId,
                        ClassId = classEntity.ClassId,
                        EnrollmentDate = DateOnly.FromDateTime(DateTime.Now)
                    };
                    context.Students.Add(newStudent);
                }

                context.SaveChanges();
            }
        }

        public IEnumerable<Mark> GetStudentMarks(int studentId)
        {
            using (var context = new DBContext())
            {
                return context.Marks.Where(m => m.StudentId == studentId).ToList();
            }
        }

        public IEnumerable<StudentMarkDTO> GetStudentMarksWithSubjects(int studentId)
        {
            using (var context = new DBContext())
            {
                var marks = from m in context.Marks
                           join s in context.Subjects on m.SubjectId equals s.SubjectId
                           where m.StudentId == studentId
                           select new StudentMarkDTO
                           {
                               MarkId = m.MarkId,
                               StudentId = m.StudentId,
                               SubjectId = m.SubjectId,
                               SubjectName = s.SubjectName,
                               ClassId = m.ClassId,
                               SemesterId = m.SemesterId,
                               Mark1 = m.Mark1,
                               ExamDate = m.ExamDate
                           };
                return marks.ToList();
            }
        }
    }
}
