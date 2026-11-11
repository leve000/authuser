using authuserAPI.Models;
using authuserAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace authuserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public RolesController(AuthuserContext context)
        {
            _context = context;
        }
        [HttpPost("newRole")]
        public async Task<IActionResult> NewRole([FromBody] Role role)
        {
            if (role is not null)
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Role created successfully", result = role });
            }
            return StatusCode(400, new { message = "Role creation failed" });
        }
        [HttpGet("allRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            if (roles is not null)
            {
                return StatusCode(200, new { message = "Roles found", result = roles });
            }
            return StatusCode(404, new { message = "Roles not found" });
        }
        [HttpPut("updateRole")]
        public async Task<IActionResult> UpdateRole([FromQuery] Guid id, [FromBody] Role role)
        {
            var existingRole = await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);
            if (existingRole is not null)
            {
                existingRole.Name = role.Name;
                _context.Roles.Update(existingRole);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "Role updated successfully", result = existingRole });
            }
            return StatusCode(404, new { message = "Role not found" });
        }
        [HttpDelete("deleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] Guid id)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);
            if (role is not null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "Role deleted successfully" });
            }
            return StatusCode(404, new { message = "Role not found" });
        }
    }
}
