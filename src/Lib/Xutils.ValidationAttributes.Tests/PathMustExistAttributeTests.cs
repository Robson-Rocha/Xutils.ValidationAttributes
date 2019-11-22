using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Xutils.ValidationAttributes.Tests
{
    [TestClass]
    public class PathMustExistAttributeTests
    {
        private class ValidationClass
        {
            [PathMustExist]
            public string Path { get; set; }
        }

        [TestMethod]
        public void When_PathExist_Then_MustBeValid()
        {
            // Arrange
            var subject = new ValidationClass { Path = Path.GetTempPath() };

            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void When_PathDoesNotExist_Then_MustNotBeValid()
        {
            // Arrange
            var subject = new ValidationClass { Path = "c:\\fake\\path" };

            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsFalse(isValid);
        }

        private class ValidationClassWithOptionalPath
        {
            [PathMustExist(Optional = true)]
            public string Path { get; set; }
        }

        [TestMethod]
        public void When_PathIsOptional_And_PathIsNotSpecified_Then_MustBeValid()
        {
            // Arrange
            var subject = new ValidationClassWithOptionalPath { Path = null };

            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsTrue(isValid);
        }

    }
}
