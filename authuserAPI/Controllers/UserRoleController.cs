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

                bool isCreated = user.Roles.Any(r => r.Id == newUserRoleDTO.RoleId);
                if (isCreated)
                {
                    return StatusCode(400, new { message = "Role already assigned to user" });
                }
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "Role assigned successfully", result = user });

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("allUsersAllRoles")]
        public async Task<IActionResult> GetAllUsersAndRoles()
        {
            var users = await _context.Users.Include(u => u.Roles).Select(u=>new
            {
                Users = u.Name,
                UserRoles=u.Roles
            })
                .ToListAsync();
            return Ok(users);
        }
        [HttpGet("userByIdAndRoles")]

        public async Task<IActionResult> GetUserByIdAndRoles([FromQuery] Guid id)
        {
            var user = await _context.Users.Include(u => u.Roles).Select(u => new
            {
                Users = u.Name,
                UserRoles = u.Roles
            }).SingleOrDefaultAsync(u => u.Users == _context.Users.SingleOrDefault(user => user.Id == id).Name);
            if (user is not null)
            {
                return StatusCode(200, new { message = "User found", result = user });
            }
            return StatusCode(404, new { message = "User not found" });
        }

    }
}
