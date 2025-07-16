using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.DTOs;
using PRN212_Project_StudentManagement.Models;

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
                                  join c in context.Classes on s.ClassId equals c.ClassId
                                  select new StudentDTO
                                  {
                                      StudentID = s.StudentId,
                                      FullName = u.FullName,
                                      Email = u.Email,
                                      ClassName = c.ClassName,
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
    }
}
