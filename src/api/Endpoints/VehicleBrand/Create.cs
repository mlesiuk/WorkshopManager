using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using workshopManager.Application.Commands.VehicleBrand;
using workshopManager.Application.Dtos;

namespace workshopManager.Api.Endpoints.VehicleBrand;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/vehicleBrand", async (
           [FromBody] CreateVehicleBrandCommand request,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(request, cancellationToken);
            return result.Match(
                success => Results.Created("/vehicleBrand", success),
                invalid => Results.BadRequest(invalid),
                validationException => Results.BadRequest(validationException));
        })
        .Produces<VehicleBrandDto>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .Produces<Exception>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .Produces<ValidationException>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .WithName("vehicleBrand-create");
    }
}
