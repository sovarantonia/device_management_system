using backend.Entity.DTO;
using backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService deviceService;

        public DeviceController(IDeviceService deviceService) 
        {
            this.deviceService = deviceService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDeviceById(Guid id)
        {
            var device = await deviceService.GetByIdAsync(id);

            return Ok(DeviceMapper.ToDTO(device));
        }

        [HttpPost]
        public async Task<IActionResult> SaveDevice([FromBody] DeviceRequest deviceRequest)
        {
            var device = await deviceService.SaveAsync(deviceRequest);
            return Ok(DeviceMapper.ToDTO(device));
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetUserDevices(Guid userId)
        {
            var devices = await deviceService.GetUserDevicesAsync(userId);
            return Ok(devices.Select(u => DeviceMapper.ToDTO(u)));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDevice(Guid id)
        {
            await deviceService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await deviceService.GetAllAsync();
            return Ok(devices.Select(u => DeviceMapper.ToDTO(u)));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDeviceDetails(Guid id, [FromBody] DeviceRequest request)
        {
            var device = await deviceService.UpdateDetailsAsync(id, request);    

            return Ok(DeviceMapper.ToDTO(device));
        }

        [HttpPut("{id:guid}/assign")]
        public async Task<IActionResult> AssignDevice(Guid id, [FromQuery] Guid userId)
        {
            var device = await deviceService.AssignDeviceAsync(id, userId);

            return Ok(DeviceMapper.ToDTO(device));
        }

        [HttpPut("{id:guid}/unassign")]
        public async Task<IActionResult> UnassignDevice(Guid id, [FromQuery] Guid userId)
        {
            var device = await deviceService.UnassignDeviceAsync(id, userId);  

            return Ok(DeviceMapper.ToDTO(device));
        }
    }
}
