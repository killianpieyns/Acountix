using api.Application.Services;
using api.Contracts.Requests;
using api.Domain.Entities;
using api.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace api.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
    {
        var user = new ApplicationUser
        {
            UserName = registerRequest.Username,
            Email = registerRequest.Email
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var result = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, false, false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Username);
            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            await _context.RefreshTokens.AddAsync(new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedByIp = HttpContext.Connection.RemoteIpAddress.ToString(),
                CreatedByUserAgent = Request.Headers["User-Agent"].ToString(),
                JwtId = Guid.NewGuid().ToString(),
                UserId = user.Id
            });

            await _context.SaveChangesAsync();

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        return Unauthorized();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto refreshTokenRequest)
    {
        var user = await _userManager.FindByNameAsync(refreshTokenRequest.Username);
        if (user == null) return Unauthorized();

        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshTokenRequest.RefreshToken && x.UserId == user.Id);

        if (refreshToken == null || refreshToken.IsExpired)
            return Unauthorized();

        var newToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();
        refreshToken.Token = newRefreshToken;
        refreshToken.Expires = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return Ok(new { Token = newToken, RefreshToken = newRefreshToken });
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RevokeTokenRequestDto revokeTokenRequest)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == revokeTokenRequest.RefreshToken);

        if (refreshToken != null)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }

        return Ok();
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}