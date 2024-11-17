using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using VehicleEngineEntity = workshopManager.Domain.Entities.VehicleEngine;

namespace workshopManager.Application.Commands.VehicleEngine;

public sealed record class CreateVehicleEngineCommand : VehicleEngineDto, IRequest<OneOf<VehicleEngineDto, ValidationException, AlreadyExistException>>;

public sealed class CreateVehicleEngineCommandHandler
    : IRequestHandler<CreateVehicleEngineCommand, OneOf<VehicleEngineDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateVehicleEngineCommand> _validator;
    private readonly IVehicleEngineRepository _vehicleEngineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleEngineCommandHandler(
        IValidator<CreateVehicleEngineCommand> validator,
        IVehicleEngineRepository vehicleEngineRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleEngineRepository = vehicleEngineRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<VehicleEngineDto, ValidationException, AlreadyExistException>> Handle(CreateVehicleEngineCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        var failures = validationResult.Errors?.ToList();
        if (failures?.Count > 0)
        {
            return new ValidationException(failures);
        }

        var entity = VehicleEngineEntity.Create(request.Name);
        if (await _vehicleEngineRepository.AlreadyExistsAsync(entity, cancellationToken))
        {
            return new AlreadyExistException(entity.Name);
        }

        await _vehicleEngineRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleEngineDto>();
    }
}
