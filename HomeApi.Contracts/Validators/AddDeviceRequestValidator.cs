using FluentValidation;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Validation;

namespace HomeApi.Contracts.Validators
{
    public class AddDeviceRequestValidator : AbstractValidator<AddDeviceRequest>
    {
        public AddDeviceRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Manufacturer).NotEmpty();
            RuleFor(x => x.Model).NotEmpty();
            RuleFor(x => x.SerialNumber).NotEmpty();
            RuleFor(x => x.CurrentVolts)
                .NotEmpty()
                .InclusiveBetween(120, 220);
            RuleFor(x => x.GasUsage).NotNull();
            RuleFor(x => x.Location)
                .NotEmpty()
                .Must(BeSupported)
                .WithMessage($"Choose from available locations: {string.Join(", ", Values.ValidRooms)}");
        }

        private bool BeSupported(string location)
        {
            return Values.ValidRooms.Any(x => x == location);
        }
    }
}
