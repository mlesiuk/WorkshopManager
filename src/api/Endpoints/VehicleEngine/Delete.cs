using FluentValidation;
using MediatR;
using System.Net.Mime;
using workshopManager.Application.Commands.VehicleEngine;

namespace workshopManager.Api.Endpoints.VehicleEngine;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapDelete("/vehicleEngine/{id}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(new DeleteVehicleEngineCommand(id), cancellationToken);
            return result.Match(
                success => Results.NoContent(),
                validationException => Results.BadRequest(validationException));
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ValidationException>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .WithName("deleteVehicleEngine");
    }
}
