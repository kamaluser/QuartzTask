using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.SliderDtos
{
    public class SliderEditDto
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int? Order { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class SliderEditDtoValidator : AbstractValidator<SliderCreateDto>
    {
        public SliderEditDtoValidator()
        {
            RuleFor(dto => dto.Title).MaximumLength(100).When(dto => !string.IsNullOrEmpty(dto.Title));
            RuleFor(dto => dto.Desc).MaximumLength(500).When(dto => !string.IsNullOrEmpty(dto.Desc));
            RuleFor(dto => dto.Order).GreaterThan(0).WithMessage("Order must be greater than zero.")
            .When(dto => dto.Order>0);

            RuleFor(x => x.ImageFile)
                .Must(IsValidContentType).WithMessage("Invalid photo format. Only .jpg, .jpeg, and .png are allowed.")
                .Must(IsValidSize).WithMessage("Photo size must be less than 5MB.");
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
