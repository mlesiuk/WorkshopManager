using FluentValidation;
using workshopManager.Application.Commands.VehicleBrand;

namespace workshopManager.Application.Validators.VehicleBrand;

public class CreateVehicleBrandValidator : AbstractValidator<CreateVehicleBrandCommand>
{
    public CreateVehicleBrandValidator()
    {
        RuleFor(vehicleBrand => vehicleBrand.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
