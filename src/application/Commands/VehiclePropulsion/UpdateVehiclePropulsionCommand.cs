using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehiclePropulsion;

public sealed record class UpdateVehiclePropulsionCommand : VehiclePropulsionDto, IRequest<OneOf<VehiclePropulsionDto, ValidationException, NotFoundException>>;

public sealed class UpdateVehiclePropulsionCommandHandler
    : IRequestHandler<UpdateVehiclePropulsionCommand, OneOf<VehiclePropulsionDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<UpdateVehiclePropulsionCommand> _validator;
    private readonly IVehiclePropulsionRepository _vehiclePropulsionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehiclePropulsionCommandHandler(
        IValidator<UpdateVehiclePropulsionCommand> validator,
        IVehiclePropulsionRepository vehiclePropulsionRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehiclePropulsionRepository = vehiclePropulsionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehiclePropulsionDto, ValidationException, NotFoundException>> Handle(UpdateVehiclePropulsionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = await _vehiclePropulsionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        entity.UpdateName(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehiclePropulsionDto>();
    }
}
