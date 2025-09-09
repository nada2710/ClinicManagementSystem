using ClinicApplication.Dtos.AccountDto;
using ClinicApplication.ServicesAbstractions;
using ClinicApplication.ServicesAbstractions.Account;
using ClinicApplication.ServicesAbstractions.VerifyEmail;
using ClinicDomain.Entities.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApplication.Services.Account
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;
        private readonly IEmailHandler _emailHandler;
        private readonly IConfiguration _configuration;
        private const string PendingUsersCacheKey = "PendingUsers_";//prefix

        public AccountServices( UserManager<ApplicationUser> userManager,IMemoryCache Cache,IEmailHandler emailHandler, IConfiguration configuration)
        {
            _userManager=userManager;
            _cache=Cache;
            _emailHandler=emailHandler;
            _configuration=configuration;
        }
        public async Task<AuthModel> Login(LoginDto loginDto)
        {
            var authModel = new AuthModel();
            var User = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User==null||!await _userManager.CheckPasswordAsync(User, loginDto.Password))
            {
                authModel.Message="Email or Password is incorrect";
                return authModel;
            }
            var JwtSecurityToken = CreateJwtToken(User);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(await JwtSecurityToken);
            authModel.Message=$"{User.UserName} login successfully";
            authModel.IsAuthenticated=true;
            authModel.Token = tokenString;

            if (User.refreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = User.refreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken=activeRefreshToken.Token;
                authModel.RefreshTokenExpiration=activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken=refreshToken.Token;
                authModel.RefreshTokenExpiration=refreshToken.ExpiresOn;
                User.refreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(User);
            }
            return authModel;
        }
        public async Task<AuthModel> Register(RegisterDto registerDto)
        {
            var response = new AuthModel();

            //1.make user email and name is not in the db
            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
            {
                response.Message = "Email is already registered!";
                return response;
            }
            if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
            {
                response.Message ="Name is already registered!";
                return response;
            }
            if (_cache.TryGetValue($"{PendingUsersCacheKey}{registerDto.Email}", out PendingUserData existingPending))
            {
                var timeSinceLastRequest = DateTime.UtcNow-(existingPending.Expiration.AddHours(-24));
                if(timeSinceLastRequest.TotalMinutes<5)
                {
                    response.Message="Verification email already send,please wait 5 minutes before requesting another.";
                    return response;
                }
                _cache.Remove($"{PendingUsersCacheKey}{registerDto.Email}");
            }
            var pendingUser = new PendingUserData
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                gender = registerDto.gender,
                Password = registerDto.Password,
                UserType=registerDto.UserType,
                VerificationCode=GenerateVerificationCode(),
                Expiration = DateTime.UtcNow.AddHours(24),
            };
            try
            {
                await _emailHandler.SendVerificationEmail(pendingUser.Email, pendingUser.VerificationCode);
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(pendingUser.Expiration)
                    .SetPriority(CacheItemPriority.Normal);

                _cache.Set($"{PendingUsersCacheKey}{pendingUser.Email}", pendingUser, cacheOptions);
                response.Message="Verification email sent";
                response.IsAuthenticated=true;
                return response;
            }
            catch(Exception ex)
            {
                response.Message=$"An error occurred while sending code : {ex.Message}";
                return response;
            }
        }
        public async Task<AuthModel> VerifyEmail(VerifyEmailDto verifyEmailDto)
        {
            var response = new AuthModel();
            if (!_cache.TryGetValue($"{PendingUsersCacheKey}{verifyEmailDto.Email}",out PendingUserData pendingUser))
            {
                response.Message="Invalid or expired verification request";
                return response;
            }
            if(pendingUser.VerificationCode!=verifyEmailDto.VerificationCode)
            {
                response.Message="Invalid verification code";
                return response;
            }
            var user = new ApplicationUser
            {
                Email = pendingUser.Email,
                UserName = pendingUser.UserName,
                PhoneNumber = pendingUser.PhoneNumber,
                UserType=pendingUser.UserType,
                VerificationCode=pendingUser.VerificationCode,
                gender=pendingUser.gender,
                EmailConfirmed = true,
                IsActive=true,
            };
            var result = await _userManager.CreateAsync(user, pendingUser.Password);
            if(!result.Succeeded)
            {
                response.Message="User creation failed";
                return response;
            }
            _cache.Remove($"{PendingUsersCacheKey}{verifyEmailDto.Email}");
            var JwtSecurityToken =await CreateJwtToken(user);
           

            //RefreshToken
            var refreshToken = GenerateRefreshToken();
            user.refreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            

            response.IsAuthenticated=true;
            response.Message="Registration completed successfully";
            response.Token= new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);
            return response;

        }
        public async Task<AuthModel> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var response = new AuthModel();
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                response.Message="If the account exists, you will receive a password reset token.";
                return response;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailHandler.SendResetCodeEmail(forgotPasswordDto.Email, token);
            response.Message="If the email is correct, a password reset token has been sent.";
            response.IsAuthenticated=true;
            return response;
        }

        public async Task<AuthModel> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var response = new AuthModel();
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user==null)
            {
                response.Message="Invalid request.";
                return response;

            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Code, resetPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                response.Message="Password reset failed.";
                return response;

            }
            response.Message="Password has been reset successfully!";
            response.IsAuthenticated=true;
            return response;
        }

        //RefreshToken
        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var authModel = new AuthModel();
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.refreshTokens.Any(t => t.Token==token));
            if (user is null)
            {
                authModel.Message="Invalid token";
                return authModel;
            }

            var refreshToken = user.refreshTokens.Single(t => t.Token==token);
            if (!refreshToken.IsActive)
            {
                authModel.Message="Inactive token";
                return authModel;
            }

            refreshToken.RevokedOn=DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.refreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated =true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email =user.Email;
            authModel.UserName = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles=roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
            return authModel;
        }
        public async Task<TokenRevocationResult> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.refreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                return new TokenRevocationResult
                {
                    Success = false,
                    Message = "Invalid token."
                };
            }
            var refreshToken = user.refreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return new TokenRevocationResult
                {
                    Success = false,
                    Message = "Token is already revoked or expired."
                };

            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return new TokenRevocationResult
            {
                Success = true,
                Message = "Token revoked successfully."
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var UserClaims = await _userManager.GetClaimsAsync(user);
            var Roles = await _userManager.GetRolesAsync(user);
            var RoleClaims = new List<Claim>();
            foreach (var Rolename in Roles)
            {
                RoleClaims.Add(new Claim(ClaimTypes.Role, Rolename));
            }
            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)

            }.Union(UserClaims)
            .Union(RoleClaims);

            var SecretKeyString = _configuration.GetSection("JwtSettings:Secret").Value;
            var SecreteKeyBytes = Encoding.ASCII.GetBytes(SecretKeyString);
            SecurityKey securityKey = new SymmetricSecurityKey(SecreteKeyBytes);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Expiredate = DateTime.Now.AddDays(2);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                claims: Claims,
                signingCredentials: signingCredentials,
                expires: Expiredate
                );
            return jwtSecurityToken;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn =DateTime.UtcNow
            };
        }
        private string GenerateVerificationCode()
        {
            var randomNumber = new byte[4];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToHexString(randomNumber)[..6].ToUpper();
        }
    }
}
