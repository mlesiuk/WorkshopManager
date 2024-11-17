using MediatR;
using System.Net.Mime;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Application.Queries.VehicleEngine;

namespace workshopManager.Api.Endpoints.VehicleEngine;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/vehicleEngine/{id}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(new GetVehicleEngineQuery(id), cancellationToken);
            return result.Match(
                success => Results.Ok(success),
                notFound => Results.NotFound());
        })
        .Produces<VehicleEngineDto>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<NotFoundException>(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("getVehicleEngine");
    }
}
