using MediatR;
using System.Net.Mime;
using workshopManager.Application.Dtos;
using workshopManager.Application.Queries.VehicleBrand;

namespace workshopManager.Api.Endpoints.VehicleBrand;

public sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/vehicleBrand", async (
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            return Results.Ok(await sender.Send(new GetAllVehicleBrandsQuery(), cancellationToken));
        })
        .Produces<IEnumerable<VehicleBrandDto>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .WithName("getAllVehicleBrands");
    }
}
