using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xutils.ValidationAttributes.Tests
{
    [TestClass]
    public class RequiredIfNullAttributeTests
    {
        private class ValidationClass
        {
            [RequiredIfNull(nameof(ConditionProperty))]
            public string ConditionalProperty { get; set; }

            public string ConditionProperty { get; set; }
        }

        [TestMethod]
        public void When_ConditionIsNull_And_ConditionalIsSet_Then_MustBeValid()
        {
            //Arrange
            var subject = new ValidationClass { ConditionalProperty = "Value", ConditionProperty = null };
            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void When_ConditionIsNull_And_ConditionalIsNull_Then_MustNotBeValid()
        {
            //Arrange
            var subject = new ValidationClass { ConditionalProperty = null, ConditionProperty = null };
            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
        }

        [TestMethod]
        public void When_ConditionIsSet_And_ConditionalIsNull_Then_MustBeValid()
        {
            //Arrange
            var subject = new ValidationClass { ConditionalProperty = null, ConditionProperty = "value" };
            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void When_ConditionIsSet_And_ConditionalIsSet_Then_MustBeValid()
        {
            //Arrange
            var subject = new ValidationClass { ConditionalProperty = "value", ConditionProperty = "value" };
            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }

        private class ValidationClassWithoutCondition
        {
            [RequiredIfNull("ConditionProperty")]
            public string ConditionalProperty { get; set; }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void When_ConditionalDoesNotExist_Then_MustRaiseException()
        {
            //Arrange
            var subject = new ValidationClassWithoutCondition { ConditionalProperty = "value" };
            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            _ = Validator.TryValidateObject(subject, validationContext, validationResults, true);
        }

        private class ValidationClassWithNonFieldOrPropertyCondition
        {
            [RequiredIfNull(nameof(ConditionProperty))]
            public string ConditionalProperty { get; set; }

            public void ConditionProperty()
            { }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void When_ConditionalIsNotFieldOrProperty_Then_MustRaiseException()
        {
            //Arrange
            var subject = new ValidationClassWithNonFieldOrPropertyCondition { ConditionalProperty = "value" };
            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            _ = Validator.TryValidateObject(subject, validationContext, validationResults, true);
        }

        private class ValidationClassWithConditionField
        {
            [RequiredIfNull(nameof(ConditionField))]
            public string ConditionalProperty { get; set; }

            public string ConditionField;
        }

        [TestMethod]
        public void When_ConditionIsField_Then_MustNotThrow()
        {
            //Arrange
            var subject = new ValidationClassWithConditionField { ConditionalProperty = "Value", ConditionField = null };
            var validationContext = new ValidationContext(subject);
            var validationResults = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(subject, validationContext, validationResults, true);

            //Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }

    }
}
