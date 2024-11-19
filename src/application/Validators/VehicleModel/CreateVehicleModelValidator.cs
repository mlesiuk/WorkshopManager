using FluentValidation;
using workshopManager.Application.Commands.VehicleModel;

namespace workshopManager.Application.Validators.VehicleModel;

public sealed class CreateVehicleModelValidator : AbstractValidator<CreateVehicleModelCommand>
{
    public CreateVehicleModelValidator()
    {
        RuleFor(vm => vm.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
