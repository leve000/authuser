using authuserAPI.Models;
using authuserAPI.Models.DTOs;
using authuserAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace authuserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthuserContext _context;
        private readonly PasswordHasher _password;

        public UserController(AuthuserContext context, PasswordHasher password)
        {
            _context = context;
            _password = password;
        }

        [HttpPost("newUser")]
        public async Task<IActionResult> NewUser([FromBody] NewUserDTO newUserDTO)
        {
            var (pass, salt) = _password.HashPassword(newUserDTO.Password!);
            var user = new User
            {
                Name = newUserDTO.Name,
                Email = newUserDTO.Email,
                Password = pass,
                Salt = salt
            };
            if (user is not null)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "User created successfully", result = user });

            }
            return StatusCode(400, new { message = "User creation failed" });
        }
        [HttpGet("allUsers")]
        public async Task<IActionResult> UserById([FromQuery] Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user is not null)
            {
                return StatusCode(200, new { message = "User found", result = user });
            }
            return StatusCode(404, new { message = "User not found" });
        }
        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser([FromQuery] Guid id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user is not null)
            {
                user.Name = updateUserDTO.Name;
                user.Email = updateUserDTO.Email;
                user.Password = updateUserDTO.Password;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "User updated successfully", result = user });
            }
            return StatusCode(404, new { message = "User not found" });
        }
        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user is not null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return StatusCode(200, new { message = "User deleted successfully" });
            }
            return StatusCode(404, new { message = "User not found" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
           var login = await _context.Users.SingleOrDefaultAsync(u => u.Name == loginDTO.Name);
            if (login is null)
            {
                return StatusCode(404, new { message = "Login failed" });
            }
            var isValid = _password.VerifyPassword(
                loginDTO.Password, login.Password!, login.Salt!);
            if (!isValid)
            {
                return StatusCode(401, new { message = "Invalid credentials" });
            }
            return StatusCode(200, new { message = "Login successful", result = login });
        }
    }
}

