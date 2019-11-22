using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace DeepRen
{
    /// <summary>
    /// Validates if the file represented by the annotated property or field exists in disk
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FileMustExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string path = value as string;
            return !string.IsNullOrWhiteSpace(path) && File.Exists(path);
        }
    }
}