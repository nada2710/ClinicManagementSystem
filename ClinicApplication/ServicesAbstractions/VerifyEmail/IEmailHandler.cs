using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApplication.ServicesAbstractions.VerifyEmail
{
    public interface IEmailHandler
    {
        Task SendVerificationEmail(string email, string code);
        Task SendResetCodeEmail(string email, string code);
    }
}
