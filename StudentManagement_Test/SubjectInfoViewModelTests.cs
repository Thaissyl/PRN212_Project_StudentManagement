using Moq;
using PRN212_Project_StudentManagement.ViewModels;
using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Data.Interfaces;
using Castle.Components.DictionaryAdapter.Xml;

namespace StudentManagement_Test
{
    public class SubjectInfoViewModelTests
    {
        private List<Subject> GetSampleSubjects() => new()
        {
            new Subject { SubjectId = 1, SubjectName = "Toán" },
            new Subject { SubjectId = 2, SubjectName = "Văn" },
            new Subject { SubjectId = 3, SubjectName = "Vật Lý" },
            new Subject { SubjectId = 4, SubjectName = "Hóa Học" },
            new Subject { SubjectId = 5, SubjectName = "Tiếng Anh" }
        };

        [Fact]
        public void LoadSubjects_ShouldInitializeSubjectsCorrectly()
        {
            var mockRepo = new Mock<ISubjectRepository>();
            mockRepo.Setup(r => r.GetAllSubjects()).Returns(GetSampleSubjects());

            var vm = new SubjectInfoViewModel(mockRepo.Object);

            Assert.Equal(5, vm.Subjects.Count);
            Assert.Contains(vm.Subjects, s => s.SubjectName == "Toán");
            Assert.Equal(1, vm.Subjects.First(s => s.SubjectId == 1).Index);
        }

        [Fact]
        public void CanExecuteEditOrDeleteSubject_ShouldReturnFalse_WhenNothingSelected()
        {
            var mockRepo = new Mock<ISubjectRepository>();
            mockRepo.Setup(r => r.GetAllSubjects()).Returns(GetSampleSubjects());

            var vm = new SubjectInfoViewModel(mockRepo.Object);
            vm.SelectedSubject = null;

            Assert.False(vm.DeleteSubjectCommand.CanExecute(null));
            Assert.False(vm.EditSubjectCommand.CanExecute(null));
        }
    }
}
