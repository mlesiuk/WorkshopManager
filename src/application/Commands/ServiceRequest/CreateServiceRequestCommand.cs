using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using ServiceRequestEntity = workshopManager.Domain.Entities.ServiceRequest;

namespace workshopManager.Application.Commands.ServiceRequest;

public sealed record class CreateServiceRequestCommand : ServiceRequestDto, IRequest<OneOf<ServiceRequestDto, ValidationException, AlreadyExistException>>;

public sealed class CreateServiceRequestCommandHandler
    : IRequestHandler<CreateServiceRequestCommand, OneOf<ServiceRequestDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateServiceRequestCommand> _validator;
    private readonly ICustomerRepository _customerRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceRequestCommandHandler(
        IValidator<CreateServiceRequestCommand> validator,
        ICustomerRepository customerRepository,
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<ServiceRequestDto, ValidationException, AlreadyExistException>> Handle(CreateServiceRequestCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        var failures = validationResult.Errors?.ToList();
        if (failures?.Count > 0)
        {
            return new ValidationException(failures);
        }

        var customer = await _customerRepository.GetByIdAsync(request.Customer.Id, cancellationToken);
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Vehicle.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new();
    }
}