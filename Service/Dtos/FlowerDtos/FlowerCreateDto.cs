using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.FlowerDtos
{
    public class FlowerCreateDto
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }
        public List<IFormFile> Photos { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();

    }
    public class FlowerCreateDtoValidator : AbstractValidator<FlowerCreateDto>
    {
        public FlowerCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50).WithMessage("Name must be less than 50 characters.");

            RuleFor(x => x.Desc)
                .NotEmpty()
                .MaximumLength(500).WithMessage("Description must be less than 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleForEach(x => x.Photos)
                .Must(IsValidContentType).WithMessage("Invalid file content. Only .jpg, .jpeg, and .png are allowed.")
                .Must(IsValidSize).WithMessage("Photo size must be less than 5MB.");

            RuleFor(x => x.CategoryIds)
                .Must(categoryIds => categoryIds != null && categoryIds.Any()).WithMessage("At least one category ID is required.");
        }

        private bool IsValidContentType(IFormFile photo)
        {
            if (photo == null)
                return false;

            var allowedContentTypes = new[] { "image/jpeg", "image/png" };
            return allowedContentTypes.Contains(photo.ContentType);
        }

        private bool IsValidSize(IFormFile photo)
        {
            if (photo == null)
                return false;

            return photo.Length <= 5 * 1024 * 1024;
        }
    }
}
