using FluentValidation;
using MediatR;
using System.Net.Mime;
using workshopManager.Application.Commands.ServiceRequest;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;

namespace workshopManager.Api.Endpoints.ServiceRequest;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/serviceRequest", async (
            CreateServiceRequestCommand request,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(request, cancellationToken);
            return result.Match(
                success => Results.Created($"/serviceRequest/{success.Id}", success),
                validationException => Results.BadRequest(validationException),
                alreadyExistException => Results.Conflict(alreadyExistException));
        })
        .Produces<ServiceRequestDto>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .Produces<ValidationException>(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .Produces<AlreadyExistException>(StatusCodes.Status409Conflict, MediaTypeNames.Application.Json)
        .WithName("createServiceRequest");
    }
}
