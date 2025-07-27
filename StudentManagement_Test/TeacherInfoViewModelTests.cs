using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.ViewModels;
using PRN212_Project_StudentManagement.Data.Interfaces;

namespace StudentManagement_Test
{
    public class TeacherInfoViewModelTests
    {
        private List<Teacher> GetSampleTeachers()
        {
            return new List<Teacher>
            {
                new Teacher { TeacherId = 1, User = new User { FullName = "Nguyễn A" } },
                new Teacher { TeacherId = 2, User = new User { FullName = "Trần B" } },
                new Teacher { TeacherId = 3, User = new User { FullName = "Lê C" } }
            };
        }

        [Fact]
        public void LoadTeachers_ShouldInitializeCorrectly()
        {
            var mockRepo = new Mock<ITeacherRepository>();
            var mockMsgBox = new Mock<IMessageBoxService>();

            mockRepo.Setup(r => r.GetAllTeachers()).Returns(GetSampleTeachers());

            var vm = new TeacherInfoViewModel(mockRepo.Object, mockMsgBox.Object);

            Assert.Equal(3, vm.Teachers.Count);
            Assert.Contains(vm.Teachers, t => t.User.FullName == "Nguyễn A");
        }

        [Fact]
        public void DeleteTeacherCommand_ShouldDelete_WhenConfirmed()
        {
            var mockRepo = new Mock<ITeacherRepository>();
            var mockMsgBox = new Mock<IMessageBoxService>();

            mockRepo.Setup(r => r.GetAllTeachers()).Returns(GetSampleTeachers());
            mockRepo.Setup(r => r.DeleteTeacher(2)).Verifiable();

            mockMsgBox.Setup(m => m.ConfirmDelete(It.IsAny<string>())).Returns(true); // Giả lập nhấn Yes

            var vm = new TeacherInfoViewModel(mockRepo.Object, mockMsgBox.Object);
            vm.SelectedTeacher = vm.Teachers.First(t => t.TeacherId == 2);

            vm.DeleteTeacherCommand.Execute(null);

            mockRepo.Verify(r => r.DeleteTeacher(2), Times.Once);
        }

        [Fact]
        public void AddTeacherCommand_ShouldReloadTeacherList()
        {
            var mockRepo = new Mock<ITeacherRepository>();
            var mockMsgBox = new Mock<IMessageBoxService>();

            mockRepo.SetupSequence(r => r.GetAllTeachers())
                .Returns(new List<Teacher>()) // initial empty
                .Returns(GetSampleTeachers()); // after adding

            var vm = new TeacherInfoViewModel(mockRepo.Object, mockMsgBox.Object, isTest: true);

            vm.AddTeacherCommand.Execute(null);

            Assert.Equal(3, vm.Teachers.Count); // Reload sau khi thêm
        }
    }
}
