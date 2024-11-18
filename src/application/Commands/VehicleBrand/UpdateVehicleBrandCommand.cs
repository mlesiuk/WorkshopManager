using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehicleBrand;

public sealed record class UpdateVehicleBrandCommand : VehicleBrandDto, IRequest<OneOf<VehicleBrandDto, ValidationException, NotFoundException>>;

public sealed class UpdateVehicleBrandCommandHandler
    : IRequestHandler<UpdateVehicleBrandCommand, OneOf<VehicleBrandDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<UpdateVehicleBrandCommand> _validator;
    private readonly IVehicleBrandRepository _vehicleBrandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleBrandCommandHandler(
        IValidator<UpdateVehicleBrandCommand> validator,
        IVehicleBrandRepository vehicleRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleBrandRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleBrandDto, ValidationException, NotFoundException>> Handle(UpdateVehicleBrandCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = await _vehicleBrandRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Body type with ID {request.Id} not found.");
        }

        entity.UpdateName(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleBrandDto>();
    }
}
