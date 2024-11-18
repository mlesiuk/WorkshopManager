using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehicleGearbox;

public sealed record class UpdateVehicleGearboxCommand : VehicleGearboxDto, IRequest<OneOf<VehicleGearboxDto, ValidationException, NotFoundException>>;

public sealed class UpdateVehicleGearboxCommandHandler
    : IRequestHandler<UpdateVehicleGearboxCommand, OneOf<VehicleGearboxDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<UpdateVehicleGearboxCommand> _validator;
    private readonly IVehicleGearboxRepository _vehicleGearboxRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleGearboxCommandHandler(
        IValidator<UpdateVehicleGearboxCommand> validator,
        IVehicleGearboxRepository vehicleGearboxRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleGearboxRepository = vehicleGearboxRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleGearboxDto, ValidationException, NotFoundException>> Handle(UpdateVehicleGearboxCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = await _vehicleGearboxRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        entity.UpdateName(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleGearboxDto>();
    }
}
