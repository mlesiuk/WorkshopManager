using FluentValidation;
using Mapster;
using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehicleBodyType;

public record UpdateVehicleBodyTypeCommand : VehicleBodyTypeDto, IRequest<OneOf<VehicleBodyTypeDto, ValidationException, NotFoundException>>;

public sealed class UpdateVehicleBodyTypeCommandHandler
    : IRequestHandler<UpdateVehicleBodyTypeCommand, OneOf<VehicleBodyTypeDto, ValidationException, NotFoundException>>
{
    private readonly IValidator<UpdateVehicleBodyTypeCommand> _validator;
    private readonly IVehicleBodyTypeRepository _bodyTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleBodyTypeCommandHandler(
        IValidator<UpdateVehicleBodyTypeCommand> validator,
        IVehicleBodyTypeRepository bodyTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _bodyTypeRepository = bodyTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<VehicleBodyTypeDto, ValidationException, NotFoundException>> Handle(UpdateVehicleBodyTypeCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ValidationException(validationResult.Errors);
        }

        var entity = await _bodyTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Body type with ID {request.Id} not found.");
        }

        entity.UpdateName(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Adapt<VehicleBodyTypeDto>();
    }
}
