using AutoMapper;
using Core.Models;
using Core.ViewModels;
using Core.ViewModels.Treatments;
using Microsoft.AspNetCore.Identity;

namespace Vetman.Mapper
{
    public class vetmanMapper : Profile
    {
        public vetmanMapper()
        {
            CreateMap<CustomerBooking, CustomerBookingReadDto>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))
            .ForMember(dest => dest.BookingGroupId, opt => opt.MapFrom(src => src.BookingGroupId))
            .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
            .ForMember(dest => dest.BookingGroupName, opt => opt.MapFrom(src => src.BookingGroup.Name))
            .ForMember(dest => dest.TotalPaid, opt => opt.MapFrom(src => src.TotalPaid))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.QuantityBooked * src.BookingGroup.ExpectedPrice))
            .ForMember(dest => dest.QuantityBooked, opt => opt.MapFrom(src => src.QuantityBooked))
            .ForMember(dest => dest.ExpectedPrice, opt => opt.MapFrom(src => src.BookingGroup.ExpectedPrice))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.BookingGroup.Product.Name))
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
            .ForMember(dest => dest.CompanyBranchId, opt => opt.MapFrom(src => src.CompanyBranchId));


            CreateMap<BookingGroup, BookingGroupViewModel>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<Customer, ClientInfoAndBookingViewModel>();

            CreateMap<ClientInfoAndBookingViewModel, Customer>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<ClientInfoAndBookingViewModel, CustomerBooking>()
              .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<IdentityRole, RoleViewModel>();

            CreateMap<Customer, CustomerViewModel>();

            CreateMap<ModuleCost, ModuleCostViewModel>();
            CreateMap<Patient, PatientViewModel>()
                .ForMember(des => des.Breed, opt => opt.MapFrom(src => src.Breed.Name))
                .ForMember(des => des.SpecieId, opt => opt.MapFrom(src => src.SpeciesId))
                .ForMember(des => des.Gender, opt => opt.MapFrom(src => src.Gender.Name))
                .ForMember(des => des.Customer, opt => opt.MapFrom(src => src.Customer.FullName))
                .ForMember(des => des.Profession, opt => opt.MapFrom(src => src.Customer.Profession));

          
           

		}
    }
}
