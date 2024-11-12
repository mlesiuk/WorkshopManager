using FluentValidation;
using MediatR;
using System.Net.Mime;
using workshopManager.Application.Commands.VehicleEngine;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Api.Endpoints.VehicleEngine;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/vehicleEngine", async (
            CreateVehicleEngineCommand request,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(request, cancellationToken);
            return result.Match(
                success => Results.Created($"/vehicleEngine/{success.Id}", success),
                validationException => Results.BadRequest(validationException),
                alreadyExistException => Results.Conflict(alreadyExistException));
        })
        .Produces<VehicleEngineDto>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .Produces<ValidationException>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .Produces<AlreadyExistException>(StatusCodes.Status409Conflict, MediaTypeNames.Application.Json)
        .WithName("createVehicleEngine");
    }
}
