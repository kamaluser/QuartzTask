using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.FlowerDtos
{
    public class FlowerEditDto
    {
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public double Price { get; set; }
        public List<int>? CategoryIds { get; set; } = new List<int>();
        public List<int>? RemovingPhotosIds { get; set; } = new List<int>();
        public List<IFormFile>? NewPhotos { get; set; } = new List<IFormFile>();

       
    }

    public class FlowerEditDtoValidator : AbstractValidator<FlowerEditDto>
    {
        public FlowerEditDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage("Name must be less than 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Desc)
                .MaximumLength(500).WithMessage("Description must be less than 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Desc));

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .When(x => x.Price != null);

            RuleForEach(x => x.NewPhotos)
                .Must(IsValidContentType).WithMessage("Invalid photo format. Only .jpg, .jpeg, and .png are allowed.")
                .Must(IsValidSize).WithMessage("Photo size must be less than 5MB.");
        }

        private bool IsValidContentType(IFormFile photo)
        {
            var allowedContentTypes = new[] { "image/jpeg", "image/png" };
            return allowedContentTypes.Contains(photo.ContentType);
        }

        private bool IsValidSize(IFormFile photo)
        {
            return photo.Length <= 5 * 1024 * 1024;
        }
    }
}
