using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace NetSimpleAuth.Backend.API.CustomValidation
{
    public class AllowedExtensionsAttribute:ValidationAttribute
    {
        private readonly string[] _extensions;
        
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }
    
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (!(value is IFormFile file)) return ValidationResult.Success;
            
            var extension = Path.GetExtension(file.FileName);
            return extension != null && !((IList) _extensions).Contains(extension.ToLower()) ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
        }
    }
}