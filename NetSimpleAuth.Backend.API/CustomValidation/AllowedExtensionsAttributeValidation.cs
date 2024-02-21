using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace NetSimpleAuth.Backend.API.CustomValidation;

/// <summary>
/// Attribute that checks for extensions in the submitted file
/// </summary>
public class AllowedExtensionsAttribute:ValidationAttribute
{
    private readonly string[] _extensions;
        
    /// <summary>
    /// Attribute that checks for extensions in the submitted file
    /// </summary>
    /// <param name="extensions">The allowed extensions</param>
    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }
    
    /// <summary>
    /// Checks whether or not the file has the valid extensions
    /// </summary>
    /// <param name="value">The value to be validated</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>The <see cref="ValidationResult"/> of the operation</returns>
    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return ValidationResult.Success;
            
        var extension = Path.GetExtension(file.FileName);
        return !((IList) _extensions).Contains(extension.ToLower()) ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
    }
}