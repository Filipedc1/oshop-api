using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Data.Models;
using ShopApi.DTOs;

namespace ShopApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        #region Fields

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public UserController(UserManager<AppUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _configuration = config;
        }

        #endregion

        #region Methods

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticatedUserDto>> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null) return NotFound();

            bool result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result) return Unauthorized();

            string token = await GenerateJwtToken(user);
            string role = user.Claims.FirstOrDefault(x => x.ClaimType == "role").ClaimValue;

            var authenticatedUser = new AuthenticatedUserDto
            {
                Username = user.UserName,
                IsAdmin = role == "Admin" ? true : false,
                Token = token
            };

            return Ok(authenticatedUser);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterAsync(RegisterDto model)
        {
            var hasher = new PasswordHasher<AppUser>();

            // if a salon, put address within the salon table
            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                //Address = BuildAddress(model),
                MemberSince = DateTime.Now,
                IsActive = true
            };

            user.PasswordHash = hasher.HashPassword(user, model.Password);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim("username", user.UserName),
                    new Claim("role", "Customer")
                };

                await _userManager.AddClaimsAsync(user, claims);

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("getuserid/{username}")]
        public async Task<ActionResult<string>> GetUserIdAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (string.IsNullOrEmpty(user.Id))
                return NotFound();

            return Ok(user.Id);
        }

        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(double.Parse(_configuration["Jwt:JwtExpireDays"]));

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:JwtIssuer"],
                audience: _configuration["Jwt:JwtIssuer"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

        #region Helpers

        //private Address BuildAddress(RegisterDto model)
        //{
        //    return new Address
        //    {
        //        Street = model.Street,
        //        City = model.City,
        //        State = model.State,
        //        Zip = model.Zip,
        //    };
        //}

        #endregion
    }
}