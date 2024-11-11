using FluentValidation;
using MediatR;
using System.Net.Mime;
using workshopManager.Application.Commands.VehicleBrand;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Api.Endpoints.VehicleBrand;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/vehicleBrand", async (
            CreateVehicleBrandCommand request,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(request, cancellationToken);
            return result.Match(
                success => Results.Created($"/vehicleBrand/{success.Id}", success),
                validationException => Results.BadRequest(validationException),
                alreadyExistException => Results.Conflict(alreadyExistException));
        })
        .Produces<VehicleBrandDto>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .Produces<ValidationException>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .Produces<AlreadyExistException>(StatusCodes.Status409Conflict, MediaTypeNames.Application.Json)
        .WithName("createVehicleBrand");
    }
}
