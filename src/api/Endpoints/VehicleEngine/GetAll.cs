using MediatR;
using System.Net.Mime;
using workshopManager.Application.Dtos;
using workshopManager.Application.Queries.VehicleEngine;

namespace workshopManager.Api.Endpoints.VehicleEngine;

public sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/vehicleEngine", async (
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            return Results.Ok(await sender.Send(new GetAllVehicleEnginesQuery(), cancellationToken));
        })
        .Produces<IEnumerable<VehicleEngineDto>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .WithName("getAllVehicleEngines");
    }
}
