using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using VehiclePropulsionEntity = workshopManager.Domain.Entities.VehiclePropulsion;

namespace workshopManager.Application.Commands.VehiclePropulsion;

public sealed record class CreateVehiclePropulsionCommand : VehiclePropulsionDto, IRequest<OneOf<VehiclePropulsionDto, ValidationException, AlreadyExistException>>;

public sealed class CreateVehiclePropulsionCommandHandler
    : IRequestHandler<CreateVehiclePropulsionCommand, OneOf<VehiclePropulsionDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateVehiclePropulsionCommand> _validator;
    private readonly IVehiclePropulsionRepository _vehiclePropulsionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehiclePropulsionCommandHandler(
        IValidator<CreateVehiclePropulsionCommand> validator,
        IVehiclePropulsionRepository vehiclePropulsionRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehiclePropulsionRepository = vehiclePropulsionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehiclePropulsionDto, ValidationException, AlreadyExistException>> Handle(CreateVehiclePropulsionCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = VehiclePropulsionEntity.Create(request.Name);
        if (await _vehiclePropulsionRepository.AlreadyExistsAsync(entity, cancellationToken))
        {
            return new AlreadyExistException(entity.Name);
        }

        await _vehiclePropulsionRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehiclePropulsionDto>();
    }
}
