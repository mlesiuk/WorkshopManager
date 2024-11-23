using MediatR;
using System.Net.Mime;
using workshopManager.Application.Dtos;
using workshopManager.Application.Exceptions;
using workshopManager.Application.Queries.Customer;

namespace workshopManager.Api.Endpoints.Customer;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/customer/{id}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var result = await sender.Send(new GetCustomerQuery(id), cancellationToken);
            return result.Match(
                success => Results.Ok(success),
                notFound => Results.NotFound());
        })
        .Produces<CustomerDto>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .Produces<NotFoundException>(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("getCustomer");
    }
}

