using FluentValidation;
using workshopManager.Application.Commands.ServiceRequest;

namespace workshopManager.Application.Validators.ServiceRequest;

public sealed class CreateServiceRequestValidator : AbstractValidator<CreateServiceRequestCommand>
{
    public CreateServiceRequestValidator()
    {
    }
}
