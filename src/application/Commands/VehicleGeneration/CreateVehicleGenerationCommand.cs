using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using VehicleGenerationEntity = workshopManager.Domain.Entities.VehicleGeneration;

namespace workshopManager.Application.Commands.VehicleGeneration;

public sealed record class CreateVehicleGenerationCommand : VehicleGenerationDto, IRequest<OneOf<VehicleGenerationDto, ValidationException, AlreadyExistException>>;

public sealed class CreateVehicleGenerationCommandHandler
    : IRequestHandler<CreateVehicleGenerationCommand, OneOf<VehicleGenerationDto, ValidationException, AlreadyExistException>>
{
    private readonly IValidator<CreateVehicleGenerationCommand> _validator;
    private readonly IVehicleGenerationRepository _vehicleGenerationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleGenerationCommandHandler(
        IValidator<CreateVehicleGenerationCommand> validator,
        IVehicleGenerationRepository vehicleGenerationRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _vehicleGenerationRepository = vehicleGenerationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleGenerationDto, ValidationException, AlreadyExistException>> Handle(CreateVehicleGenerationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = VehicleGenerationEntity.Create(request.Name);
        if (await _vehicleGenerationRepository.AlreadyExistsAsync(entity, cancellationToken))
        {
            return new AlreadyExistException(entity.Name);
        }

        await _vehicleGenerationRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleGenerationDto>();
    }
}
