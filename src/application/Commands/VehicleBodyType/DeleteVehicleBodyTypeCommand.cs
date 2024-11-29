using MediatR;
using OneOf;
using workshopManager.Application.Exceptions;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Application.Commands.VehicleBodyType;

public sealed record class DeleteVehicleBodyTypeCommand(Guid Id) : IRequest<OneOf<bool, NotFoundException>>;

public sealed class DeleteVehicleBodyTypeCommandHandler
    : IRequestHandler<DeleteVehicleBodyTypeCommand, OneOf<bool, NotFoundException>>
{
    private readonly IVehicleBodyTypeRepository _vehicleBodyTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleBodyTypeCommandHandler(
        IVehicleBodyTypeRepository vehicleBodyTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleBodyTypeRepository = vehicleBodyTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<bool, NotFoundException>> Handle(DeleteVehicleBodyTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleBodyTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Body type with ID {request.Id} not found.");
        }

        await _vehicleBodyTypeRepository.RemoveAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
