using FluentValidation;
using workshopManager.Application.Commands.VehicleEngine;

namespace workshopManager.Application.Validators.VehicleEngine;

public sealed class UpdateVehicleEngineValidator : AbstractValidator<UpdateVehicleEngineCommand>
{
    public UpdateVehicleEngineValidator()
    {
        RuleFor(ve => ve.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
