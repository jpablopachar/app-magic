using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Data;
using server.Dtos;
using server.Models;

namespace server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserApp> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly string _secretKey;

        public UserRepository(AppDbContext context, IConfiguration configuration, UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret")!;
        }

        public bool IsUserUnique(string username)
        {
            var user = _context.UsersApp.FirstOrDefault(u => u.UserName!.ToLower() == username.ToLower());

            if (user == null) return true;

            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _context.UsersApp.FirstOrDefaultAsync(u => u.UserName!.ToLower() == loginRequestDto.Username!.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user!, loginRequestDto.Password!);

            if (user == null || !isValid) return new LoginResponseDto() { Token = "", User = null };

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new(ClaimTypes.Name, user.UserName!.ToString()),
                    new(ClaimTypes.Role, roles.FirstOrDefault()!)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new() { Token = tokenHandler.WriteToken(token), User = _mapper.Map<UserDto>(user) };

            return loginResponseDto;
        }

        public async Task<UserDto> Register(RegisterRequestDto registerRequestDto)
        {
            UserApp user = new()
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
                NormalizedEmail = registerRequestDto.Username!.ToUpper(),
                Names = registerRequestDto.Names
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDto.Password!);

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));

                        await _roleManager.CreateAsync(new IdentityRole("client"));
                    }

                    await _userManager.AddToRoleAsync(user, "admin");

                    var userApp = _context.UsersApp.FirstOrDefault(u => u.UserName == registerRequestDto.Username);

                    return _mapper.Map<UserDto>(userApp);
                }
            }
            catch (Exception) { throw; }

            return new UserDto();
        }
    }
}