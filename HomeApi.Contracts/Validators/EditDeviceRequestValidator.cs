using FluentValidation;
using HomeApi.Contracts.Models.Devices;

namespace HomeApi.Contracts.Validation
{
    public class EditDeviceRequestValidator : AbstractValidator<EditDeviceRequest>
    {
        public EditDeviceRequestValidator() 
        {
            RuleFor(x => x.NewName).NotEmpty(); 
            RuleFor(x => x.NewRoom).NotEmpty().Must(BeSupported)
                .WithMessage($"Choose from available locations: {string.Join(", ", Values.ValidRooms)}");
        }

        private bool BeSupported(string location)
        {
            return Values.ValidRooms.Any(e => e == location);
        }
    }
}