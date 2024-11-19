using FluentValidation;
using workshopManager.Application.Commands.VehiclePropulsion;

namespace workshopManager.Application.Validators.VehiclePropulsion;

public sealed class CreateVehiclePropulsionValidator : AbstractValidator<CreateVehiclePropulsionCommand>
{
    public CreateVehiclePropulsionValidator()
    {
        RuleFor(vp => vp.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
