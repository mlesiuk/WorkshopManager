﻿using FluentValidation;
using workshopManager.Application.Commands.VehicleEngine;

namespace workshopManager.Application.Validators.VehicleEngine;

public sealed class CreateVehicleEngineValidator : AbstractValidator<CreateVehicleEngineCommand>
{
    public CreateVehicleEngineValidator()
    {
        RuleFor(ve => ve.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(255);
    }
}
