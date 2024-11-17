using FluentValidation;
using workshopManager.Application.Commands.VehicleBodyType;

namespace workshopManager.Application.Validators.VehicleBodyType;

public sealed class UpdateVehicleBodyTypeValidator : AbstractValidator<UpdateVehicleBodyTypeCommand>
{
    public UpdateVehicleBodyTypeValidator()
    {
        RuleFor(vbt => vbt.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
