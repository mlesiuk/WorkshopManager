using FluentValidation;
using workshopManager.Application.Commands.VehicleModel;

namespace workshopManager.Application.Validators.VehicleModel;

public sealed class UpdateVehicleModelValidator : AbstractValidator<UpdateVehicleModelCommand>
{
    public UpdateVehicleModelValidator()
    {
        RuleFor(vm => vm.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
