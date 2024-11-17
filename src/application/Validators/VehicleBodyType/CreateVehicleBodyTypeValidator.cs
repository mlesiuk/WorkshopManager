using FluentValidation;
using workshopManager.Application.Commands.VehicleBodyType;

namespace workshopManager.Application.Validators.VehicleBodyType;

public sealed class CreateVehicleBodyTypeValidator : AbstractValidator<CreateVehicleBodyTypeCommand>
{
    public CreateVehicleBodyTypeValidator()
    {
        RuleFor(vbt => vbt.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
