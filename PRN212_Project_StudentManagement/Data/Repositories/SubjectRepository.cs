using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace PRN212_Project_StudentManagement.Data.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        public List<Subject> GetAllSubjects()
        {
            using (var context = new DBContext())
            {
                return context.Subjects.ToList();
            }
        }
        public void AddSubject(Subject subject)
        {
            using (var context = new DBContext())
            {
                context.Subjects.Add(subject);
                context.SaveChanges();
            }
        }
        public void UpdateSubject(int subjectId, string subjectName)
        {
            using (var context = new DBContext())
            {
                var subject = context.Subjects.Find(subjectId);
                if (subject != null)
                {
                    subject.SubjectName = subjectName;
                    context.SaveChanges();
                }
            }
        }
        public void DeleteSubject(int subjectId)
        {
            using (var context = new DBContext())
            {
                var subject = context.Subjects.Find(subjectId);
                if (subject != null)
                {
                    context.Subjects.Remove(subject);
                    context.SaveChanges();
                }
            }
        }
    }
} 