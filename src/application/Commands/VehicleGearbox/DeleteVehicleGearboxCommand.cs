using MediatR;
using OneOf;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Commands.VehicleGearbox;

public sealed record class DeleteVehicleGearboxCommand : VehicleGearboxDto, IRequest<OneOf<bool, NotFoundException>>;

public sealed class DeleteVehicleGearboxCommandHandler
    : IRequestHandler<DeleteVehicleGearboxCommand, OneOf<bool, NotFoundException>>
{
    private readonly IVehicleGearboxRepository _vehicleGearboxRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleGearboxCommandHandler(
        IVehicleGearboxRepository vehicleGearboxRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleGearboxRepository = vehicleGearboxRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<bool, NotFoundException>> Handle(DeleteVehicleGearboxCommand request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleGearboxRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        await _vehicleGearboxRepository.RemoveAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
