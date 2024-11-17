using FluentValidation;
using workshopManager.Application.Commands.VehicleFuelType;

namespace workshopManager.Application.Validators.VehicleFuelType;

public sealed class UpdateVehicleFuelTypeValidator : AbstractValidator<UpdateVehicleFuelTypeCommand>
{
    public UpdateVehicleFuelTypeValidator()
    {
        RuleFor(vft => vft.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
