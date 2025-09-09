using ClinicApplication.Dtos.AccountDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApplication.ServicesAbstractions.Account
{
    public interface IAccountServices
    {
        Task<AuthModel> Login(LoginDto loginDto);
        Task<AuthModel> Register(RegisterDto registerDto);
        Task<AuthModel> VerifyEmail(VerifyEmailDto verifyEmailDto);
        Task<AuthModel> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<AuthModel> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<TokenRevocationResult> RevokeTokenAsync(string token);
    }
}
