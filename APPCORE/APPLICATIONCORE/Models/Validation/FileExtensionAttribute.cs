using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;



namespace APPLICATIONCORE.Models.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png", "jpeg" };
                bool result = extensions.Any(a => extension.EndsWith(a));
                if (!result)
                {
                    return new ValidationResult("Chỉ chứa ảnh có đuôi file jpg, png, jpeg");
                }
            }
            return ValidationResult.Success;
        }
    }
}
