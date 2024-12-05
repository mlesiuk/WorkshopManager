using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Application.Utils;
using workshopManager.Domain.Abstractions.Interfaces;
using workshopManager.Domain.Events;
using ServiceRequestEntity = workshopManager.Domain.Entities.ServiceRequest;

namespace workshopManager.Application.Commands.ServiceRequest;

public sealed record class CreateServiceRequestCommand : ServiceRequestDto, IRequest<OneOf<ServiceRequestDto, ValidationException, NotFoundException>>;

public sealed class CreateServiceRequestCommandHandler
    : IRequestHandler<CreateServiceRequestCommand, OneOf<ServiceRequestDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<CreateServiceRequestCommand> _validator;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ICustomerRepository _customerRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceRequestCommandHandler(
        ICustomerRepository customerRepository,
        IVehicleRepository vehicleRepository,
        IServiceRequestRepository serviceRequestRepository,
        IValidator<CreateServiceRequestCommand> validator,
        IUnitOfWork unitOfWork,
        IMessagePublisher messagePublisher)
    {
        _customerRepository = customerRepository;
        _vehicleRepository = vehicleRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
    }

    public async Task<OneOf<ServiceRequestDto, ValidationException, NotFoundException>> Handle(CreateServiceRequestCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        var failures = validationResult.Errors?.ToList();
        if (failures?.Count > 0)
        {
            return new ValidationException(failures);
        }

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            return new NotFoundException("Customer", request.CustomerId);
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle is null)
        {
            return new NotFoundException("Vehicle", request.VehicleId);
        }

        if (!customer.VehicleBelongsToCustomer(vehicle.Id))
        {
            return new ValidationException($"Vehicle with id {vehicle.Id} does not belong to customer with id {customer.Id}.");
        }

        if (request.ServiceDate.IsWeekendDay())
        {
            return new ValidationException("Service cannot be registered at weekend day.");
        }

        var serviceRequest = ServiceRequestEntity.Create(customer, vehicle, request.ServiceDate);
        await _serviceRequestRepository.AddAsync(serviceRequest, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _messagePublisher.PublishAsync(new ServiceRegisteredEvent
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            VehicleId = vehicle.Id
        });

        return serviceRequest.Adapt<ServiceRequestDto>();
    }
}
