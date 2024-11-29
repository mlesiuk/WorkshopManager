using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Domain.Abstractions.Interfaces;
using VehicleGearboxEntity = workshopManager.Domain.Entities.VehicleGearbox;

namespace workshopManager.Application.Commands.VehicleGearbox;

public sealed record class CreateVehicleGearboxCommand : VehicleGearboxDto, IRequest<OneOf<VehicleGearboxDto, ValidationException, AlreadyExistException>>;

public sealed class CreateVehicleGearboxCommandHandler
    : IRequestHandler<CreateVehicleGearboxCommand, OneOf<VehicleGearboxDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateVehicleGearboxCommand> _validator;
    private readonly IVehicleGearboxRepository _vehicleGearboxRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleGearboxCommandHandler(
        IValidator<CreateVehicleGearboxCommand> validator,
        IVehicleGearboxRepository vehicleGearboxRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleGearboxRepository = vehicleGearboxRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleGearboxDto, ValidationException, AlreadyExistException>> Handle(CreateVehicleGearboxCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = VehicleGearboxEntity.Create(request.Name);
        if (await _vehicleGearboxRepository.AlreadyExistsAsync(entity, cancellationToken))
        {
            return new AlreadyExistException(entity.Name);
        }

        await _vehicleGearboxRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleGearboxDto>();
    }
}
