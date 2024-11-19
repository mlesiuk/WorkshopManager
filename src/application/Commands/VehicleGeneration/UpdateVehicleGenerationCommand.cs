using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehicleGeneration;

public sealed record class UpdateVehicleGenerationCommand : VehicleGenerationDto, IRequest<OneOf<VehicleGenerationDto, ValidationException, NotFoundException>>;

public sealed class UpdateVehicleGenerationCommandHandler
    : IRequestHandler<UpdateVehicleGenerationCommand, OneOf<VehicleGenerationDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<UpdateVehicleGenerationCommand> _validator;
    private readonly IVehicleGenerationRepository _vehicleGenerationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleGenerationCommandHandler(
        IValidator<UpdateVehicleGenerationCommand> validator,
        IVehicleGenerationRepository vehicleGenerationRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleGenerationRepository = vehicleGenerationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleGenerationDto, ValidationException, NotFoundException>> Handle(UpdateVehicleGenerationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = await _vehicleGenerationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        entity.UpdateName(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleGenerationDto>();
    }
}
