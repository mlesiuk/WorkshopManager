using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Commands.VehicleModel;

public sealed record class UpdateVehicleModelCommand : VehicleModelDto, IRequest<OneOf<VehicleModelDto, ValidationException, NotFoundException>>;

public sealed class UpdateVehicleModelCommandHandler
    : IRequestHandler<UpdateVehicleModelCommand, OneOf<VehicleModelDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<UpdateVehicleModelCommand> _validator;
    private readonly IVehicleModelRepository _vehicleModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleModelCommandHandler(
        IValidator<UpdateVehicleModelCommand> validator,
        IVehicleModelRepository vehicleModelRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleModelRepository = vehicleModelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleModelDto, ValidationException, NotFoundException>> Handle(UpdateVehicleModelCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = await _vehicleModelRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        entity.UpdateName(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleModelDto>();
    }
}
