using AutoMapper;
using Core.Entities;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Dtos.FlowerDtos;
using Service.Dtos;
using Service.Exceptions;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories.Implementations;

namespace Service.Services.Implementations
{
    public class FlowerService : IFlowerService
    {
        private readonly IFlowerRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public FlowerService(IFlowerRepository repository, ICategoryRepository categoryRepository, IMapper mapper, IWebHostEnvironment environment)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _environment = environment;
        }

        public int Create(FlowerCreateDto createDto)
        {
            if (_repository.Exists(x => x.Name.ToLower() == createDto.Name.ToLower() && !x.IsDeleted))
                throw new RestException(StatusCodes.Status400BadRequest, "Name", "Flower with the same name already exists.");


            if (createDto.CategoryIds != null && createDto.CategoryIds.Any())
            {
                var existingCategoryIds = _categoryRepository.GetAll(x => true).Select(c => c.Id).ToHashSet();
                foreach (var categoryId in createDto.CategoryIds)
                {
                    if (!existingCategoryIds.Contains(categoryId))
                    {
                        throw new RestException(StatusCodes.Status400BadRequest, "Category", $"Invalid category ID: {categoryId}");
                    }
                }
            }

            Flower flower = _mapper.Map<Flower>(createDto);
            flower.CreatedAt = DateTime.Now;

            HandlePhotos(createDto.Photos, flower);

            MapCategories(flower, createDto.CategoryIds);

            _repository.Add(flower);
            _repository.Save();

            return flower.Id;
        }

        public PaginatedList<FlowerGetDto> GetAllByPage(string? search = null, int page = 1, int size = 10)
        {
            var query = _repository.GetAll(x => !x.IsDeleted && (search == null || x.Name.Contains(search)), "Photos", "FlowerCategories.Category");
            var paginated = PaginatedList<Flower>.Create(query, page, size);
            if (page > paginated.TotalPages)
            {
                page = paginated.TotalPages;
                paginated = PaginatedList<Flower>.Create(query, page, size);
            }
            return new PaginatedList<FlowerGetDto>(_mapper.Map<List<FlowerGetDto>>(paginated.Items), paginated.TotalPages, page, size);
        }

        public FlowerGetDto GetById(int id)
        {
            Flower entity = _repository.Get(x => x.Id == id && !x.IsDeleted, "Photos", "FlowerCategories.Category");

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound, "Flower not found");

            return _mapper.Map<FlowerGetDto>(entity);
        }

        public void Edit(int id, FlowerEditDto editDto)
        {
            Flower entity = _repository.Get(x => x.Id == id && !x.IsDeleted, "Photos", "FlowerCategories.Category");

            if (entity == null)
                throw new RestException(StatusCodes.Status404NotFound, "Flower not found");

            if (entity.Name != editDto.Name && _repository.Exists(x => x.Name == editDto.Name && !x.IsDeleted))
                throw new RestException(StatusCodes.Status400BadRequest, "Name", "Flower by this name already exists.");

            if (editDto.CategoryIds != null && editDto.CategoryIds.Any())
            {
                var existingCategoryIds = _categoryRepository.GetAll(x => true).Select(c => c.Id).ToHashSet();
                foreach (var categoryId in editDto.CategoryIds)
                {
                    if (!existingCategoryIds.Contains(categoryId))
                    {
                        throw new RestException(StatusCodes.Status400BadRequest, "Category", $"Invalid category ID: {categoryId}");
                    }
                }
            }

            entity.Name = editDto.Name;
            entity.Desc = editDto.Desc;
            entity.Price = editDto.Price;

            HandlePhotos(editDto.NewPhotos, entity);

            List<string> removeStrList = new List<string>();
            if (editDto.RemovingPhotosIds != null && editDto.RemovingPhotosIds.Any())
            {
                var photosToRemove = entity.Photos.Where(p => editDto.RemovingPhotosIds.Contains(p.Id)).ToList();
                foreach (var photo in photosToRemove)
                {
                    entity.Photos.Remove(photo);
                    removeStrList.Add(photo.Name);
                }
            }
            MapCategories(entity, editDto.CategoryIds);

            entity.ModifiedAt = DateTime.Now;

            _repository.Save();

            if (removeStrList.Count > 0)
            {
                DeleteAll(_environment.WebRootPath, "uploads//photos", removeStrList);
            }
        }

        public void Delete(int id)
        {
            Flower entity = _repository.Get(x => x.Id == id);

            if (entity == null)
            {
                throw new RestException(StatusCodes.Status404NotFound, "Flower not found");
            }

            if (entity.IsDeleted)
            {
                throw new RestException(StatusCodes.Status400BadRequest, "Flower already deleted");
            }

            entity.ModifiedAt = DateTime.Now;
            entity.IsDeleted = true;

            _repository.Save();
        }

        private void DeletePhotoFile(Photo photo)
        {
            string filePath = Path.Combine(_environment.WebRootPath, "uploads\\photos", photo.Name);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void HandlePhotos(List<IFormFile> formFiles, Flower flower)
        {
            if (formFiles != null && formFiles.Any())
            {
                foreach (var formFile in formFiles)
                {
                    if (formFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads\\photos");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(formFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(fileStream);
                        }

                        var photo = new Photo
                        {
                            Name = uniqueFileName,
                            Flower = flower
                        };
                        flower.Photos.Add(photo);
                    }
                }
            }
        }

        private void MapCategories(Flower flower, List<int> categoryIds)
        {
            if (categoryIds != null && categoryIds.Any())
            {
                flower.FlowerCategories.Clear();
                foreach (var categoryId in categoryIds)
                {
                    flower.FlowerCategories.Add(new FlowerCategory { CategoryId = categoryId });
                }
            }
        }

        private void DeleteAll(string rootPath, string folder, List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                string path = Path.Combine(rootPath, folder, fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

            }
        }
    }
}
