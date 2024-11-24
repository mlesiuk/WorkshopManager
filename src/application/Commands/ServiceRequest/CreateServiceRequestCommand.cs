﻿using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using ServiceRequestEntity = workshopManager.Domain.Entities.ServiceRequest;

namespace workshopManager.Application.Commands.ServiceRequest;

public sealed record class CreateServiceRequestCommand : ServiceRequestDto, IRequest<OneOf<ServiceRequestDto, ValidationException, NotFoundException>>;

public sealed class CreateServiceRequestCommandHandler
    : IRequestHandler<CreateServiceRequestCommand, OneOf<ServiceRequestDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<CreateServiceRequestCommand> _validator;
    private readonly ICustomerRepository _customerRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceRequestCommandHandler(
        ICustomerRepository customerRepository,
        IVehicleRepository vehicleRepository,
        IServiceRequestRepository serviceRequestRepository,
        IValidator<CreateServiceRequestCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _vehicleRepository = vehicleRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
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

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new();
    }
}