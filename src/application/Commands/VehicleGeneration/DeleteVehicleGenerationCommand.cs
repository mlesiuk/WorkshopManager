using MediatR;
using OneOf;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Application.Commands.VehicleGeneration;

public sealed record class DeleteVehicleGenerationCommand : VehicleGenerationDto, IRequest<OneOf<bool, NotFoundException>>;

public sealed class DeleteVehicleGenerationCommandHandler
    : IRequestHandler<DeleteVehicleGenerationCommand, OneOf<bool, NotFoundException>>
{
    private readonly IVehicleGenerationRepository _vehicleGenerationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleGenerationCommandHandler(
        IVehicleGenerationRepository vehicleGenerationRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleGenerationRepository = vehicleGenerationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<bool, NotFoundException>> Handle(DeleteVehicleGenerationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _vehicleGenerationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            return new NotFoundException($"Fuel type with ID {request.Id} not found.");
        }

        await _vehicleGenerationRepository.RemoveAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
