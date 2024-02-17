using CleanArchitecture.Application.Constants;
using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Models.Identity;
using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Identity.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        // JwtSettings class should be mapped to the appsettings.json so it must be wrapped with the IOptions so it returns a
        // configured instance of JwtSettings.
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new Exception($"User with email {request.Email} couldn't be found");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, false);

            if (!result.Succeeded)
            {
                throw new Exception("Incorrect username or password");
            }

            var token = await GenerateToken(user);

            var authResponse = new AuthResponse()
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return authResponse;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);

            if(existingUser != null)
            {
                throw new Exception("A user registered with that username already exists");
            }

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);

            if(existingEmail != null)
            {
                throw new Exception("A user registered with that email already exists");
            }

            var user = new ApplicationUser()
            {
                Email = request.Email,
                Name = request.UserName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
            throw new Exception($"{result.Errors}");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "OPERATOR");

            if (!roleResult.Succeeded)
            {
                throw new Exception($"{result.Errors}");
            }

            var token = await GenerateToken(user);

            return new RegistrationResponse()
            {
                Email = user.Email,
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = user.UserName
            };
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            var claims = new[]
            {
                // it is like a dictionary, you provide a key to be used to access a value,
                // you can use the predefined types or custom types e.g. new Claim("userName", user.UserName!)
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(CustomClaimTypes.Uid, user.Id)
                // then we gonna add the userClaims and the roleClaims, to have all of the claims in a single object
            }.Union(userClaims).Union(roleClaims);

            // key to access to the data, like a jwt secret in jwt library you use in express applications
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            // defining the signin credentials and the signature algorithm to be used
            var signinCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signinCredentials
                );

            return jwtSecurityToken;
        }

    }
}
