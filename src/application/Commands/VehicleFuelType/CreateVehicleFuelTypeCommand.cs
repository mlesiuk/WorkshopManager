using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using VehicleFuelTypeEntity = workshopManager.Domain.Entities.VehicleFuelType;

namespace workshopManager.Application.Commands.VehicleFuelType;

public record CreateVehicleFuelTypeCommand : VehicleFuelTypeDto, IRequest<OneOf<VehicleFuelTypeDto, ValidationException, AlreadyExistException>>;

public sealed class CreateVehicleFuelTypeCommandHandler
: IRequestHandler<CreateVehicleFuelTypeCommand, OneOf<VehicleFuelTypeDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateVehicleFuelTypeCommand> _validator;
    private readonly IVehicleFuelTypeRepository _fuelTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleFuelTypeCommandHandler(
        IValidator<CreateVehicleFuelTypeCommand> validator,
        IVehicleFuelTypeRepository fuelTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _fuelTypeRepository = fuelTypeRepository;
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
        if (await _fuelTypeRepository.AlreadyExistsAsync(entity, cancellationToken))
        {
            return new AlreadyExistException(entity.Name);
        }

        await _fuelTypeRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleFuelTypeDto>();
    }
}
