using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Xutils.ValidationAttributes
{
    /// <summary>
    /// Validates if the path represented by the annotated property or field exists in disk
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PathMustExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string path = value as string;
            return !string.IsNullOrWhiteSpace(path) && Directory.Exists(path);
        }
    }
}