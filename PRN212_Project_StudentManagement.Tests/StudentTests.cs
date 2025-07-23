using PRN212_Project_StudentManagement.Models;
using Xunit;
using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Tests
{
    public class StudentTests
    {
        [Fact]
        public void Student_Initialization_DefaultValues()
        {
            // Arrange
            var student = new Student();

            // Assert
            Assert.Equal(0, student.StudentId);
            Assert.Equal(0, student.UserId);
            Assert.Equal(0, student.ClassId);
            Assert.Equal(default(DateOnly), student.EnrollmentDate);
            Assert.NotNull(student.Class);
            Assert.NotNull(student.StudentLogs);
            Assert.IsType<List<StudentLog>>(student.StudentLogs);
            Assert.NotNull(student.User);
        }
    }
} 