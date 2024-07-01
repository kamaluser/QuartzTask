using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.SliderDtos
{
    public class SliderCreateDto
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Order { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class SliderCreateDtoValidator : AbstractValidator<SliderCreateDto>
    {
        public SliderCreateDtoValidator()
        {
            RuleFor(dto => dto.Title).NotEmpty().MaximumLength(100);
            RuleFor(dto => dto.Desc).NotEmpty().MaximumLength(500);
            RuleFor(dto => dto.Order).NotEmpty().GreaterThan(0).WithMessage("Order must be greater than zero.");
            /*RuleFor(dto => dto.ImageFile)
                .Must(IsValidContentType).WithMessage("Invalid photo format. Only .jpg, .jpeg, and .png are allowed.")
                .Must(IsValidSize).WithMessage("Photo size must be less than 5MB.");*/

            RuleFor(x => x).Custom((dto, context) =>
            {
                if (dto.ImageFile != null)
                {
                    if (dto.ImageFile.Length > 5 * 1024 * 1024)
                    {
                        context.AddFailure("ImageFile", "File must be less or equal than 5MB.");
                    }

                    var allowedContentTypes = new[] { "image/jpeg", "image/png" };
                    if (!allowedContentTypes.Contains(dto.ImageFile.ContentType))
                    {
                        context.AddFailure("ImageFile", "Invalid photo format. Only .jpg, .jpeg, and .png are allowed.");
                    }
                }
            });

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
