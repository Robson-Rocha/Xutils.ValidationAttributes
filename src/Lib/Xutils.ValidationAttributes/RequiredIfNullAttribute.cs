using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Xutils.ValidationAttributes
{
    /// <summary>
    /// Validates if the property or field value is specifed when another property value is null
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RequiredIfNullAttribute : ValidationAttribute
    {
        private readonly string _propertyOrFieldName;

        /// <summary>
        /// Validates if the property or field value is specifed when another property value is null
        /// </summary>
        /// <param name="propertyOrFieldName">Name of the property or field in which, when the value is null, will make the annotated property or field to be required</param>
        public RequiredIfNullAttribute(string propertyOrFieldName)
        {
            this._propertyOrFieldName = propertyOrFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var membersToTest = validationContext.ObjectType.GetMember(_propertyOrFieldName);

            if (membersToTest.Length == 0)
            {
                throw new ArgumentException($"The type {validationContext.ObjectType.Name} does not have a property or field named {_propertyOrFieldName}");
            }

            object testedValue;
            switch (membersToTest[0])
            {
                case FieldInfo fieldinfo:
                    testedValue = ((FieldInfo)membersToTest[0]).GetValue(validationContext.ObjectInstance);
                    break;

                case PropertyInfo propertyInfo:
                    testedValue = ((PropertyInfo)membersToTest[0]).GetValue(validationContext.ObjectInstance);
                    break;

                default:
                    throw new ArgumentException($"The type {validationContext.ObjectType.Name} member {_propertyOrFieldName} must be a property or field");
            }

            return testedValue == null && value == null
                ? new ValidationResult($"{validationContext.MemberName} is required when {_propertyOrFieldName} is null")
                : ValidationResult.Success;
        }
    }
}
