using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Domain.Abstractions.Interfaces;
using VehicleFuelTypeEntity = workshopManager.Domain.Entities.VehicleFuelType;

namespace workshopManager.Application.Commands.VehicleFuelType;

public sealed record class CreateVehicleFuelTypeCommand : VehicleFuelTypeDto, IRequest<OneOf<VehicleFuelTypeDto, ValidationException, AlreadyExistException>>;

public sealed class CreateVehicleFuelTypeCommandHandler
    : IRequestHandler<CreateVehicleFuelTypeCommand, OneOf<VehicleFuelTypeDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateVehicleFuelTypeCommand> _validator;
    private readonly IVehicleFuelTypeRepository _vehicleFuelTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleFuelTypeCommandHandler(
        IValidator<CreateVehicleFuelTypeCommand> validator,
        IVehicleFuelTypeRepository vehicleFuelTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleFuelTypeRepository = vehicleFuelTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleFuelTypeDto, ValidationException, AlreadyExistException>> Handle(CreateVehicleFuelTypeCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = VehicleFuelTypeEntity.Create(request.Name);
        if (await _vehicleFuelTypeRepository.AlreadyExistsAsync(entity, cancellationToken))
        {
            return new AlreadyExistException(entity.Name);
        }

        await _vehicleFuelTypeRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleFuelTypeDto>();
    }
}
