using FluentValidation;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Contracts.Validation;

namespace HomeApi.Contracts.Validators
{
    public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
    {
        public UpdateRoomRequestValidator() 
        {
            RuleFor(x => x.Area).NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty()
                .Must(BeSupported)
                .WithMessage($"Choose from available locations: {string.Join(", ", Values.ValidRooms)}");
            RuleFor(x => x.Voltage).NotEmpty();
            RuleFor(x => x.GasConnected).NotEmpty();
        }

        private bool BeSupported(string location)
        {
            return Values.ValidRooms.Any(x => x == location);
        }
    }
}
