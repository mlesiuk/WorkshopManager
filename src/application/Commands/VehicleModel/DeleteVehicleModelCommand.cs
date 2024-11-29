using MediatR;
using OneOf;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Commands.VehicleModel;

public sealed record class DeleteVehicleModelCommand : VehicleModelDto, IRequest<OneOf<bool, NotFoundException>>;

public sealed class DeleteVehicleModelCommandHandler
    : IRequestHandler<DeleteVehicleModelCommand, OneOf<bool, NotFoundException>>
{
    private readonly IVehicleModelRepository _vehicleModelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleModelCommandHandler(
        IVehicleModelRepository vehicleModelRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleModelRepository = vehicleModelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<bool, NotFoundException>> Handle(DeleteVehicleModelCommand request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleModelRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        await _vehicleModelRepository.RemoveAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
