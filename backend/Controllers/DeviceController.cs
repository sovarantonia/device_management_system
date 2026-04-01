using backend.Entity;
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
            if (device == null)
            {
                return NotFound("Device not found");
            }

            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDevice([FromBody] DeviceRequest device)
        {
            await deviceService.SaveAsync(device);
            return Ok(device);
        }

        [HttpGet("/user/{userId:guid}")]
        public async Task<IActionResult> GetUserDevices(Guid userId)
        {
            var devices = await deviceService.GetUserDevicesAsync(userId);
            return Ok(devices);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDevice(Guid id)
        {
            var deleted = await deviceService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound("Device not found");
            }

            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDeviceDetails(Guid id, [FromBody] DeviceRequest request)
        {
            var updated = await deviceService.UpdateDetailsAsync(id, request);
            if (!updated)
            {
                return NotFound("Device not found");
            }
            var device = await deviceService.GetByIdAsync(id);

            return Ok(device);
        }
    }
}
