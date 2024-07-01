using AutoMapper;
using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Service.Dtos.SliderDtos;
using Service.Dtos;
using Service.Exceptions;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Service.Dtos.CategoryDtos;
using Microsoft.AspNetCore.Hosting;

namespace Service.Services.Implementations
{
    public class SliderService : ISliderService
    {
        private readonly ISliderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public SliderService(ISliderRepository repository, IMapper mapper, IWebHostEnvironment environment)
        {
            _repository = repository;
            _mapper = mapper;
            _environment = environment;
        }

        public int Create(SliderCreateDto createDto)
        {
           /* if (_repository.Exists(x => x.Title.ToLower() == createDto.Title.ToLower()))
                throw new RestException(StatusCodes.Status400BadRequest, "Title", "Slider by this title already exists.");*/

            var slider = _mapper.Map<Slider>(createDto);

            if (createDto.ImageFile != null && createDto.ImageFile.Length > 0)
            {
                slider.Image = UploadImage(createDto.ImageFile);
            }

            _repository.Add(slider);
            _repository.Save();

            return slider.Id;
        }

        public void Edit(int id, SliderEditDto editDto)
        {
            var slider = _repository.Get(x => x.Id == id);

            if (slider == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, "Slider not found");
            }

           /* if (slider.Title != editDto.Title && _repository.Exists(x => x.Title == editDto.Title))
            {
                throw new RestException(StatusCodes.Status400BadRequest, "Name", "Slider by this title already exists.");
            }*/

            if (editDto.ImageFile != null && editDto.ImageFile.Length > 0)
            {
                var allowedContentTypes = new[] { "image/jpeg", "image/png" };
                if (!allowedContentTypes.Contains(editDto.ImageFile.ContentType))
                {
                    throw new RestException(StatusCodes.Status400BadRequest, "Invalid photo format. Only .jpg, .jpeg and .png are allowed.");
                }
            }

           /* _mapper.Map(editDto, slider);*/

            slider.Title = editDto.Title;
            slider.Desc = editDto.Desc;
            if (editDto.Order != null && editDto.Order > 0)
            {
                slider.Order = editDto.Order.Value;
            }
            if (editDto.ImageFile != null && editDto.ImageFile.Length > 0)
            {
                DeletePhotoFile(slider.Image);
                slider.Image = UploadImage(editDto.ImageFile);
            }
            slider.ModifiedAt = DateTime.Now;


            _repository.Save();
        }

        public void Delete(int id)
        {
            var slider = _repository.Get(x => x.Id == id);
            if (slider == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, "Slider not found");
            }

            if (slider.IsDeleted)
            {
                throw new RestException(StatusCodes.Status400BadRequest, "Slider already deleted");
            }


            slider.ModifiedAt = DateTime.Now;
            slider.IsDeleted = true;
            _repository.Delete(slider);
            _repository.Save();
        }

        public PaginatedList<SliderGetDto> GetAllByPage(string? search = null, int page = 1, int size = 10)
        {
            var query = _repository.GetAll(x => search == null || x.Title.Contains(search));
            var paginated = PaginatedList<Slider>.Create(query, page, size);
            if (page > paginated.TotalPages)
            {
                page = paginated.TotalPages;
                paginated = PaginatedList<Slider>.Create(query, page, size);
            }
            return new PaginatedList<SliderGetDto>(_mapper.Map<List<SliderGetDto>>(paginated.Items), paginated.TotalPages, page, size);
        }

        public SliderGetDto GetById(int id)
        {
            var slider = _repository.Get(x => x.Id == id && !x.IsDeleted);

            if (slider == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, "Slider not found");
            }

            return _mapper.Map<SliderGetDto>(slider);
        }

        private void DeletePhotoFile(string photo)
        {
            string filePath = Path.Combine(_environment.WebRootPath, "uploads\\sliders", photo);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new RestException(StatusCodes.Status400BadRequest, "Image file is required");
            }

            string uploadsFolder = Path.Combine(_environment.ContentRootPath,"wwwroot", "uploads", "sliders");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}
