using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Xutils.ValidationAttributes
{
    /// <summary>
    /// Validates if the path represented by the annotated property or field exists in disk
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class PathMustExistAttribute : ValidationAttribute
    {
        /// <summary>
        /// Indicates that this path is optional.
        /// </summary>
        public bool Optional { get; set; } = false;

        public override bool IsValid(object value)
        {
            string path = value as string;
            return (string.IsNullOrWhiteSpace(path) && Optional) || Directory.Exists(path);
        }
    }
}