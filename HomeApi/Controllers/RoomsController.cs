using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomsController(
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _roomRepository.GetRoomByName(request.Name);

            if (existingRoom is null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _roomRepository.AddRoom(newRoom);

                return StatusCode(201, $"Room {request.Name} added.");
            }

            return StatusCode(409, $"Error. Room {request.Name} has already been added.");
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateRoomRequest request)
        {
            var room = await _roomRepository.GetRoomById(id);

            if (room is null)
                return StatusCode(400, $"Error: Room {id} not found");

            await _roomRepository.UpdateRoom(
                room, 
                new UpdateRoomQuery() 
                {
                    NewName = request.Name,
                    NewArea = request.Area,
                    NewGasConnected = request.GasConnected,
                    NewVoltage = request.Voltage,
                });

            return StatusCode(200, $"Room {id} updated{Environment.NewLine}" +
                $"Name: {request.Name}{Environment.NewLine}" +
                $"Area: {request.Area}{Environment.NewLine}" +
                $"GasConnected: {request.GasConnected}{Environment.NewLine}" +
                $"Voltage: {request.Voltage}");
        }
    }
}
