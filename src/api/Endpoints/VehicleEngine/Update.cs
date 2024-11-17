using FluentValidation;
using MediatR;
using System.Net.Mime;
using workshopManager.Application.Commands.VehicleEngine;
using workshopManager.Application.Dtos;

namespace workshopManager.Api.Endpoints.VehicleEngine;

public sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut("/vehicleEngine/{id}", async (
            Guid id,
            UpdateVehicleEngineCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return result.Match(
                success => Results.Created($"/vehicleEngine/{success.Id}", success),
                validationException => Results.BadRequest(validationException),
                alreadyExistException => Results.Conflict(alreadyExistException));
        })
        .Produces<VehicleEngineDto>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .Produces<ValidationException>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .WithName("updateVehicleEngine");
    }
}
