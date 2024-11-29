using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Commands.VehicleFuelType;

public sealed record class UpdateVehicleFuelTypeCommand : VehicleFuelTypeDto, IRequest<OneOf<VehicleFuelTypeDto, ValidationException, NotFoundException>>;

public sealed class UpdateVehicleFuelTypeCommandHandler
    : IRequestHandler<UpdateVehicleFuelTypeCommand, OneOf<VehicleFuelTypeDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<UpdateVehicleFuelTypeCommand> _validator;
    private readonly IVehicleFuelTypeRepository _vehicleFuelTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleFuelTypeCommandHandler(
        IValidator<UpdateVehicleFuelTypeCommand> validator,
        IVehicleFuelTypeRepository vehicleFuelTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleFuelTypeRepository = vehicleFuelTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleFuelTypeDto, ValidationException, NotFoundException>> Handle(UpdateVehicleFuelTypeCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = await _vehicleFuelTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        entity.UpdateName(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleFuelTypeDto>();
    }
}
