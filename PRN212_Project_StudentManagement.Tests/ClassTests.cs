using PRN212_Project_StudentManagement.Models;
using Xunit;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Tests
{
    public class ClassTests
    {
        [Fact]
        public void Class_Initialization_DefaultValues()
        {
            // Arrange
            var c = new Class();

            // Assert
            Assert.Equal(0, c.ClassId);
            Assert.NotNull(c.ClassName);
            Assert.Null(c.TeacherId);
            Assert.Equal(0, c.AcademicFromYear);
            Assert.Equal(0, c.AcademicToYear);
            Assert.NotNull(c.Students);
            Assert.IsType<List<Student>>(c.Students);
            Assert.Null(c.Teacher);
        }
    }
} 