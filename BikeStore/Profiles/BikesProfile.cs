using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BikeStore.Models;
using BikeStore.Models.EfModels;
using BikeStore.ViewModels;

namespace BikeStore.Profiles
{
    public class BikesProfile:Profile
    {
        public BikesProfile()
        {
            CreateMap<Product, BikeCardViewModel>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ReverseMap();
            CreateMap<CreateBikeViewModel, Product>()
                .ForMember(dest => dest.ProductPhoto, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Product, UpdateBikeViewModel>()
                .ForMember(dest => dest.ProductPhoto, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Product, DeleteBikeViewModel>();
            CreateMap<UserRegisterViewModel, UserIdentityModel>()
                .ReverseMap();
            CreateMap<UserIdentityModel, UsersListItemModel>()
                .ReverseMap();
            CreateMap<UserIdentityModel, UserManageViewModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Order, OrderViewModel>();
        }
    }
}
