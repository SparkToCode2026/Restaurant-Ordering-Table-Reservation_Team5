using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Team5.DTOs;
using Team5.Models;
using Team5.Services;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly JwtService _jwtService;
        private readonly PasswordHasher<User> _passwordHasher;


        public AuthController(
            ProjectContext context,
            JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;

            _passwordHasher = new PasswordHasher<User>();
        }


        // =====================================================
        // REGISTER
        // =====================================================

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Check whether email already exists

            var existingUser = _context.Users
                .FirstOrDefault(u => u.UserEmail == dto.UserEmail);


            if (existingUser != null)
            {
                return BadRequest(
                    "A user with this email already exists."
                );
            }


            // Create new User

            var user = new User
            {
                UserName = dto.UserName,

                UserEmail = dto.UserEmail,

                PhoneNumber = dto.PhoneNumber,

                Role = dto.Role,

                CreatedAt = DateTime.Now
            };


            // Hash password

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password
                );


            // Add user to database

            _context.Users.Add(user);

            _context.SaveChanges();


            return Ok(new
            {
                message = "Registration successful.",

                userId = user.UserId,

                userName = user.UserName,

                userEmail = user.UserEmail,

                role = user.Role
            });
        }


        // =====================================================
        // LOGIN
        // =====================================================

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Find user by email

            var user = _context.Users
                .FirstOrDefault(
                    u => u.UserEmail == dto.UserEmail
                );


            if (user == null)
            {
                return Unauthorized(
                    "Invalid email or password."
                );
            }


            // Verify password

            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.Password
                );


            if (passwordResult ==
                PasswordVerificationResult.Failed)
            {
                return Unauthorized(
                    "Invalid email or password."
                );
            }


            // Generate JWT token

            var token =
                _jwtService.GenerateToken(user);


            return Ok(new
            {
                message = "Login successful.",

                token = token,

                user = new
                {
                    userId = user.UserId,

                    userName = user.UserName,

                    userEmail = user.UserEmail,

                    role = user.Role
                }
            });
        }
    }
}