using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

public record UpdateVehicleEngineCommand : VehicleEngineDto, IRequest<OneOf<VehicleEngineDto, ValidationException, NotFoundException>>
{
}

public sealed class UpdateVehicleEngineCommandHandler
    : IRequestHandler<UpdateVehicleEngineCommand, OneOf<VehicleEngineDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<UpdateVehicleEngineCommand> _validator;
    private readonly IVehicleEngineRepository _vehicleEngineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleEngineCommandHandler(
        IValidator<UpdateVehicleEngineCommand> validator,
        IVehicleEngineRepository vehicleEngineRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleEngineRepository = vehicleEngineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleEngineDto, ValidationException, NotFoundException>> Handle(UpdateVehicleEngineCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        var failures = validationResult.Errors?.ToList();
        if (failures?.Count > 0)
        {
            return new ValidationException(failures);
        }

        var entity = await _vehicleEngineRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"VehicleEngine with ID {request.Id} not found.");
        }

        entity.UpdateName(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleEngineDto>();
    }
}
