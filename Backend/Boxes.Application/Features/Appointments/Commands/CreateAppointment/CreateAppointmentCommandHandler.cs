using AutoMapper;
using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.Contracts.Persistence;
using Boxes.Domain.Entities;
using MediatR;

namespace Boxes.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        IWorkshopService workshopService,
        IAppointmentUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CreateAppointmentCommand, int>
    {
        private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
        private readonly IWorkshopService _workshopService = workshopService;
        private readonly IAppointmentUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var workshop = await _workshopService.GetWorkshopByIdAsync(request.PlaceId, cancellationToken);
            if (workshop is null || !workshop.Active)
                throw new InvalidOperationException($"El taller con PlaceId {request.PlaceId} no existe o no está activo.");

            var appointment = _mapper.Map<Appointment>(request);

            await _appointmentRepository.AddAsync(appointment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return appointment.Id;
        }
    }
}