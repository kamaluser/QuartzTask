using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Service.Dtos.CategoryDtos;
using Service.Dtos.FlowerDtos;
using Service.Dtos.Photos;
using Service.Dtos.SliderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Profiles
{
    public class MapProfile : Profile
    {
        private readonly IHttpContextAccessor _accessor;

        public MapProfile(IHttpContextAccessor accessor)
        {
            _accessor = accessor;

            var uriBuilder = new UriBuilder(_accessor.HttpContext.Request.Scheme, _accessor.HttpContext.Request.Host.Host, _accessor.HttpContext.Request.Host.Port ?? -1);
            if (uriBuilder.Uri.IsDefaultPort)
            {
                uriBuilder.Port = -1;
            }
            string baseUrl = uriBuilder.Uri.AbsoluteUri;

            //category

            CreateMap<Category, CategoryGetDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryEditDto, Category>();

            //photo
            CreateMap<Photo, PhotoGetDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{baseUrl}/uploads/photos/{src.Name}"));

            //flower
            CreateMap<Flower, FlowerGetDto>()
            .ForMember(dest => dest.Categories, x => x.MapFrom(src => src.FlowerCategories.Select(fc => fc.Category).ToList()))
            .ForMember(dest => dest.Photos, x => x.MapFrom(src => src.Photos));
            CreateMap<FlowerCreateDto, Flower>()
            .ForMember(dest => dest.Photos, opt => opt.Ignore())
            .ForMember(dest => dest.FlowerCategories, opt => opt.Ignore());
            CreateMap<FlowerEditDto, Flower>();

            //slider
            CreateMap<Slider, SliderGetDto>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                var baseUrl = _accessor.HttpContext.Request.Scheme + "://" + _accessor.HttpContext.Request.Host.Value;
                return baseUrl + $"/uploads/sliders/{src.Image}";
            }));

            CreateMap<SliderCreateDto, Slider>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<SliderEditDto, Slider>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

        }
    }
}
