using FluentValidation;
using workshopManager.Application.Commands.VehiclePropulsion;

namespace workshopManager.Application.Validators.VehiclePropulsion;

public sealed class UpdateVehiclePropulsionValidator : AbstractValidator<UpdateVehiclePropulsionCommand>
{
    public UpdateVehiclePropulsionValidator()
    {
        RuleFor(vp => vp.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
