using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using VehicleBrandEntity = workshopManager.Domain.Entities.VehicleBrand;

namespace workshopManager.Application.Commands.VehicleBrand;

public sealed record class CreateVehicleBrandCommand : VehicleBrandDto, IRequest<OneOf<VehicleBrandDto, ValidationException, AlreadyExistException>>;

public sealed class CreateVehicleBrandCommandHandler 
    : IRequestHandler<CreateVehicleBrandCommand, OneOf<VehicleBrandDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateVehicleBrandCommand> _validator;
    private readonly IVehicleBrandRepository _vehicleBrandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleBrandCommandHandler(
        IValidator<CreateVehicleBrandCommand> validator,
        IVehicleBrandRepository vehicleBrandRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleBrandRepository = vehicleBrandRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OneOf<VehicleBrandDto, ValidationException, AlreadyExistException>> Handle(CreateVehicleBrandCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        var failures = validationResult.Errors?.ToList();
        if (failures?.Count > 0)
        {
            return new ValidationException(failures);
        }

        var entity = VehicleBrandEntity.Create(request.Name);
        if (await _vehicleBrandRepository.AlreadyExistsAsync(entity, cancellationToken))
        {
            return new AlreadyExistException(entity.Name);
        }

        await _vehicleBrandRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleBrandDto>();
    }
}
