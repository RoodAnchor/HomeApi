using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : Controller
    { 
        private readonly IDeviceRepository _deviceRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public DevicesController(
            IDeviceRepository deviceRepository,
            IRoomRepository roomRepository,
            IMapper mapper) 
        {
            _deviceRepository = deviceRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetDevices()
        {
            var devices = await _deviceRepository.GetDevices();
            var res = new GetDevicesResponse
            {
                DeviceCount = devices.Length,
                Devices = _mapper.Map<Device[], DeviceView[]>(devices)
            };

            return StatusCode(200, res);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add(AddDeviceRequest request)
        {
            var room = await _roomRepository.GetRoomByName(request.Location);

            if(room is null)
                return StatusCode(400, $"Error: Room {request.Location} is not added. Add room first!");

            var device = await _deviceRepository.GetDeviceByName(request.Name);

            if(device != null)
                return StatusCode(400, $"Error: Device {request.Name} has already been added.");

            var newDevice = _mapper.Map<AddDeviceRequest, Device>(request);
            await _deviceRepository.SaveDevice(newDevice, room);

            return StatusCode(201, $"Device {request.Name} added. ID: {newDevice.Id}");
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditDeviceRequest request)
        {
            var room = await _roomRepository.GetRoomByName(request.NewRoom);

            if (room is null)
                return StatusCode(400, $"Error: Room {request.NewRoom} is not added. Add room first.");

            var device = await _deviceRepository.GetDeviceById(id);

            if (device is null)
                return StatusCode(400, $"Error: Device {id} does not exist.");

            var withSameName = await _deviceRepository.GetDeviceByName(request.NewName);

            if (withSameName != null)
                return StatusCode(400, $"Error: Device with name {request.NewName} has already been added. Select different name.");

            await _deviceRepository.UpdateDevice(
                device,
                room,
                new UpdateDeviceQuery(
                    request.NewName,
                    request.NewSerial));

            return StatusCode(200, $"Device updated.{Environment.NewLine}" +
                $"Name: {device.Name}{Environment.NewLine}" +
                $"Serial number: {device.SerialNumber}{Environment.NewLine}" +
                $"Location: {device.Location}");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var device = await _deviceRepository.GetDeviceById(id);

            if (device is null)
                return StatusCode(400, $"Error: Device {id} does not exist.");

            await _deviceRepository.DeleteDevice(device);

            return StatusCode(200, $"Device {id} removed.");
        }
    }
}
