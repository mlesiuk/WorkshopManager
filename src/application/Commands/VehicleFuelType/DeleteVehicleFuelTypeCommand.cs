using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehicleFuelType;

public sealed record class DeleteVehicleFuelTypeCommand : VehicleFuelTypeDto, IRequest<OneOf<bool, NotFoundException>>;

public sealed class DeleteVehicleFuelTypeCommandHandler
    : IRequestHandler<DeleteVehicleFuelTypeCommand, OneOf<bool, NotFoundException>>
{
    private readonly IVehicleFuelTypeRepository _vehicleFuelTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleFuelTypeCommandHandler(
        IVehicleFuelTypeRepository vehicleFuelTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleFuelTypeRepository = vehicleFuelTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<bool, NotFoundException>> Handle(DeleteVehicleFuelTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleFuelTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        await _vehicleFuelTypeRepository.RemoveAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
