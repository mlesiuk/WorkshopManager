using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehicleBrand;

public record DeleteVehicleBrandCommand(Guid Id) : IRequest<OneOf<bool, NotFoundException>>;

public sealed class DeleteVehicleBrandCommandHandler
    : IRequestHandler<DeleteVehicleBrandCommand, OneOf<bool, NotFoundException>>
{
    private readonly IVehicleBrandRepository _vehicleBrandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleBrandCommandHandler(
        IVehicleBrandRepository vehicleBrandRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleBrandRepository = vehicleBrandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<bool, NotFoundException>> Handle(DeleteVehicleBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleBrandRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Body type with ID {request.Id} not found.");
        }

        await _vehicleBrandRepository.RemoveAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
