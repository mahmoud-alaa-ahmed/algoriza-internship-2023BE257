using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Vezeeta.Core.Dtos.AuthDtos;
using Vezeeta.Core.Helpers;
using Vezeeta.Core.Models;
using Vezeeta.Core.ServiceInterfaces;

namespace Vezeeta.Services
{
    public class AuthenticationService : IAuth
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private IConfiguration _conf;
		public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration conf, RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_conf = conf;
		}
		public async Task<string> CreateJWTAsync(ApplicationUser user)
        {
            DateTime? EndDate = DateTime.Now.AddDays(7);

            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.NameIdentifier,$"{user.Id}"),
            };
            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var identity = new ClaimsIdentity(userClaims);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_conf.GetSection("JWT:Key").Value));

            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = EndDate,   // DateTime.Now.AddDays(double.Parse(_conf.GetSection("JWT:DurationInDays").Value)),
                SigningCredentials = credintials,

            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApplicationUser> FindUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<AuthModel> OnLoginAsync(LoginDTO logInfo)
        {
            var result = new AuthModel();
            var user = await _userManager.FindByEmailAsync(logInfo.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, logInfo.Password))
            {
                result.Message = "Incorrect Email or Password";
                return result;
            }
            var userToken = await CreateJWTAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            return new AuthModel()
            {
                Message = "login Success",
                Email = user.Email,
                Roles = userRoles.ToList(),
                IsAuthenticated = true,
                Token = userToken,
                Id = user.Id
            };
        }

        public async Task<AuthModel> OnRegisterAsync(RegisterDTO regInfo,string FolderName,string userType)
        {
            AuthModel result = new AuthModel();
            var ExistUser = await _userManager.FindByEmailAsync(regInfo.Email);
            var emailPattern = EmailValidation.CheckEmailRegex(regInfo.Email);
            if (!emailPattern)
            {
                result.Message = "Invalid Email Address";
                return result;
            }
            if (ExistUser is not null)
            {
                result.Message = "Email Already Taken";
                return result;
            }

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = regInfo.FirstName,
                LastName = regInfo.LastName,
                Email = regInfo.Email,
                DateOfBirth = regInfo.DateOfBirth,
                Phone = regInfo.Phone,
                Gender = regInfo.Gender,
                UserType = userType,
				UserName = $"Aa123{regInfo.FirstName}{regInfo.LastName}"
			};

			if (regInfo.Image != null)
				user.Image = UploadHelpers.UploadFile(regInfo.Image, FolderName);
			else
				user.Image = $"images/{FolderName}/{FolderName}-not-found.jpg";


			IdentityResult res = await _userManager.CreateAsync(user, regInfo.Password);
            if (!res.Succeeded)
            {
                StringBuilder str = new StringBuilder();
                foreach (var err in res.Errors)
                {
                    str.Append(err.Description + Environment.NewLine);
                }
                return new AuthModel { Message = str.ToString() };
            }
            if (userType == UserRole.Patient)
                await _userManager.AddToRoleAsync(user, "Patient");
            else
                await _userManager.AddToRoleAsync(user, "Doctor");

            var userRoles = await _userManager.GetRolesAsync(user);
            return new AuthModel
            {
                Message = "Created Successfully",
                IsAuthenticated = true,
                Roles = userRoles.ToList(),
                Email = user.Email,
                Token = await CreateJWTAsync(user),
                Id = user.Id
            };
        }

        public async Task<string> OnSetRoleAsync(RoleSetModelDTO requestInfo)
        {
            var existUser = await _userManager.FindByEmailAsync(requestInfo.Email);
            if (existUser is null || !await _roleManager.RoleExistsAsync(requestInfo.Role))
            {
                return "Invalid User Or Role!";
            }
            if (await _userManager.IsInRoleAsync(existUser, requestInfo.Role))
                return $"User Already Have The Role {requestInfo.Role} ";
            var res = await _userManager.AddToRoleAsync(existUser, requestInfo.Role);
            return res.Succeeded ? string.Empty : "Request Failed";
        }

    }
}
