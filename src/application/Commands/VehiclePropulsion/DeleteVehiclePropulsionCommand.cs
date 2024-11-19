using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehiclePropulsion;

public sealed record class DeleteVehiclePropulsionCommand : VehiclePropulsionDto, IRequest<OneOf<bool, NotFoundException>>;

public sealed class DeleteVehiclePropulsionCommandHandler
    : IRequestHandler<DeleteVehiclePropulsionCommand, OneOf<bool, NotFoundException>>
{
    private readonly IVehiclePropulsionRepository _vehiclePropulsionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehiclePropulsionCommandHandler(
        IVehiclePropulsionRepository vehiclePropulsionRepository,
        IUnitOfWork unitOfWork)
    {
        _vehiclePropulsionRepository = vehiclePropulsionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<bool, NotFoundException>> Handle(DeleteVehiclePropulsionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _vehiclePropulsionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        await _vehiclePropulsionRepository.RemoveAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
