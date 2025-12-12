using AutoMapper;
using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Commands.CreateAppointment;
using Boxes.Domain.Entities;

namespace Boxes.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<ContactDto, Contact>()
                .ConstructUsing(src => new Contact(src.Name, src.Email, src.Phone));

            CreateMap<VehicleDto, Vehicle>()
                .ConstructUsing(src => new Vehicle(src.Make, src.Model, src.Year, src.LicensePlate));

            CreateMap<CreateAppointmentCommand, Appointment>()
                .ConstructUsing(src => new Appointment(
                    src.PlaceId,
                    src.AppointmentAt,
                    src.ServiceType,
                    new Contact(src.Contact.Name, src.Contact.Email, src.Contact.Phone),
                    src.Vehicle != null
                        ? new Vehicle(src.Vehicle.Make, src.Vehicle.Model, src.Vehicle.Year, src.Vehicle.LicensePlate)
                        : null))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Contact, ContactDto>();

            CreateMap<Vehicle, VehicleDto>();

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Contact))
                .ForMember(dest => dest.Vehicle, opt => opt.MapFrom(src => src.Vehicle));
        }
    }
}
