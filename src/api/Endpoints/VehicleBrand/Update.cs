using FluentValidation;
using MediatR;
using System.Net.Mime;
using workshopManager.Application.Commands.VehicleBrand;
using workshopManager.Application.Dtos;

namespace workshopManager.Api.Endpoints.VehicleBrand;

public sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut("/vehicleBrand/{id}", async (
            Guid id,
            UpdateVehicleBrandCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return result.Match(
                success => Results.Created($"/vehicleBrand/{success.Id}", success),
                validationException => Results.BadRequest(validationException),
                alreadyExistException => Results.Conflict(alreadyExistException));
        })
        .Produces<VehicleBrandDto>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .Produces<ValidationException>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .WithName("updateVehicleBrand");
    }
}
