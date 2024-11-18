using FluentValidation;
using workshopManager.Application.Commands.VehicleGearbox;

namespace workshopManager.Application.Validators.VehicleGearbox;

public sealed class CreateVehicleGearboxValidator : AbstractValidator<CreateVehicleGearboxCommand>
{
    public CreateVehicleGearboxValidator()
    {
        RuleFor(vg => vg.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
