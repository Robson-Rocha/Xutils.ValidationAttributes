using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Xutils.ValidationAttributes.Tests
{
    [TestClass]
    public class FileMustExistAttributeTests
    {
        private class ValidationClass
        {
            [FileMustExist]
            public string FileName { get; set; }
        }

        [TestMethod]
        public void When_FileExist_Then_MustBeValid()
        {
            // Arrange
            var subject = new ValidationClass { FileName = Path.GetTempFileName() };

            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void When_FileDoesNotExist_Then_MustNotBeValid()
        {
            // Arrange
            var subject = new ValidationClass { FileName = "loremipsum.txt" };

            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsFalse(isValid);
        }

        private class ValidationClassWithOptionalFileName
        {
            [FileMustExist(Optional = true)]
            public string FileName { get; set; }
        }

        [TestMethod]
        public void When_FileNameIsOptional_And_FileNameIsNotSpecified_Then_MustBeValid()
        {
            // Arrange
            var subject = new ValidationClassWithOptionalFileName { FileName = null };

            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsTrue(isValid);
        }

    }
}
