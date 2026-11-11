using authuserAPI.Models;
using authuserAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace authuserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public UserRoleController(AuthuserContext context)
        {
            _context = context;
        }
        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] NewUserRoleDTO newUserRoleDTO)
        {
           try
            {
                var user = await _context.Users.Include(u=>u.Roles)
                    .SingleOrDefaultAsync(u => u.Id == newUserRoleDTO.UserId);
                var role = await _context.Roles.FindAsync(newUserRoleDTO.RoleId);
                if (user is null || role is null)
                {
                    return StatusCode(404, new { message = "User or Role not found" });
                }
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "Role assigned successfully", result = user });

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

    }
}
