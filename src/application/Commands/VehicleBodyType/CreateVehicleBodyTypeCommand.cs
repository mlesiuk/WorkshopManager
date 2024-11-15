using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using VehicleBodyTypeEntity = workshopManager.Domain.Entities.VehicleBodyType;

namespace workshopManager.Application.Commands.VehicleBodyType;

public record CreateVehicleBodyTypeCommand(string Name) : IRequest<OneOf<VehicleBodyTypeDto, ValidationException, AlreadyExistException>>;

public sealed class CreateVehicleBodyTypeCommandHandler
    : IRequestHandler<CreateVehicleBodyTypeCommand, OneOf<VehicleBodyTypeDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateVehicleBodyTypeCommand> _validator;
    private readonly IVehicleBodyTypeRepository _vehicleBodyTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleBodyTypeCommandHandler(
        IValidator<CreateVehicleBodyTypeCommand> validator,
        IVehicleBodyTypeRepository vehicleBodyTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleBodyTypeRepository = vehicleBodyTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleBodyTypeDto, ValidationException, AlreadyExistException>> Handle(CreateVehicleBodyTypeCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = VehicleBodyTypeEntity.Create(request.Name);
        if (await _vehicleBodyTypeRepository.AlreadyExistsAsync(entity, cancellationToken))
        {
            return new AlreadyExistException(entity.Name);
        }

        await _vehicleBodyTypeRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleBodyTypeDto>();
    }
}
