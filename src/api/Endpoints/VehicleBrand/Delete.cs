using FluentValidation;
using MediatR;
using System.Net.Mime;
using workshopManager.Application.Commands.VehicleBrand;

namespace workshopManager.Api.Endpoints.VehicleBrand;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapDelete("/vehicleBrand/{id}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(new DeleteVehicleBrandCommand(id), cancellationToken);
            return result.Match(
                success => Results.NoContent(),
                validationException => Results.BadRequest(validationException));
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ValidationException>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .WithName("deleteVehicleBrand");
    }
}
