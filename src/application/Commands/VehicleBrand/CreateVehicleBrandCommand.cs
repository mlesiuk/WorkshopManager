using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;

namespace workshopManager.Application.Commands.VehicleBrand;

public sealed class CreateVehicleBrandCommand : IRequest<OneOf<VehicleBrandDto, ValidationException, Exception>>
{
    public string Name { get; set; } = string.Empty;
}

public sealed class CreateVehicleBrandCommandHandler 
    : IRequestHandler<CreateVehicleBrandCommand, OneOf<VehicleBrandDto, ValidationException, Exception>>
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

    public async Task<OneOf<VehicleBrandDto, ValidationException, Exception>> Handle(CreateVehicleBrandCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(request);
        var failures = validationResult.Errors?.ToList();
        if (failures?.Count > 0)
        {
            return new ValidationException(failures);
        }

        var element = Domain.Entities.VehicleBrand.Create(request.Name);

        if (await _vehicleBrandRepository.AlreadyExistsAsync(element, cancellationToken))
        {
            return new Exception($"Entity {element.Name} already exists");
        }

        await _vehicleBrandRepository.AddAsync(element, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return element.Adapt<VehicleBrandDto>();
    }
}
