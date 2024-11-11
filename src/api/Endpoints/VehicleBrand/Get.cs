using MediatR;
using OneOf.Types;
using System.Net.Mime;
using workshopManager.Application.Dtos;
using workshopManager.Application.Queries.VehicleBrand;

namespace workshopManager.Api.Endpoints.VehicleBrand;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/vehicleBrand/{id}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(new GetVehicleBrandQuery(id), cancellationToken);
            return result.Match(
                success => Results.Created("/vehicleBrand", success),
                notFound => Results.NotFound());
        })
        .Produces<VehicleBrandDto>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .Produces<NotFound>(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("vehicleBrand");
    }
}

