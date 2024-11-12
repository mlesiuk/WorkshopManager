using FluentValidation;
using workshopManager.Application.Commands.VehicleEngine;

namespace workshopManager.Application.Validators.VehicleEngine;

public class CreateVehicleEngineValidator : AbstractValidator<CreateVehicleEngineCommand>
{
    public CreateVehicleEngineValidator()
    {
        RuleFor(vehicleBrand => vehicleBrand.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
