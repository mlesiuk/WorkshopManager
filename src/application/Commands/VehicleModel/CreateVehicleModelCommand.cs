using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using VehicleModelEntity = workshopManager.Domain.Entities.VehicleModel;

namespace workshopManager.Application.Commands.VehicleModel;

public sealed record class CreateVehicleModelCommand : VehicleModelDto, IRequest<OneOf<VehicleModelDto, ValidationException, AlreadyExistException>>;

public sealed class CreateVehicleModelCommandHandler
    : IRequestHandler<CreateVehicleModelCommand, OneOf<VehicleModelDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateVehicleModelCommand> _validator;
    private readonly IVehicleModelRepository _vehicleModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleModelCommandHandler(
        IValidator<CreateVehicleModelCommand> validator,
        IVehicleModelRepository vehicleModelRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleModelRepository = vehicleModelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleModelDto, ValidationException, AlreadyExistException>> Handle(CreateVehicleModelCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = VehicleModelEntity.Create(request.Name);
        if (await _vehicleModelRepository.AlreadyExistsAsync(entity, cancellationToken))
        {
            return new AlreadyExistException(entity.Name);
        }

        await _vehicleModelRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleModelDto>();
    }
}
