using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApplication.Services.VerifyEmail
{
    public class EmailTemplates
    {
        public static string VerifyEmailTemplate(string Code)
        {
            return $@"<div style='font-family: Segoe UI, Roboto, sans-serif; max-width: 600px; margin: 0 auto; background-color: #ffffff; border: 1px solid #e0e0e0; padding: 30px; border-radius: 10px; box-shadow: 0 8px 20px rgba(0, 0, 0, 0.08);'>
                        <div style='text-align: center; margin-bottom: 30px;'>
                            <h1 style='margin: 0; color: #2c3e50;'>🩺 Clinic Management System</h1>
                            <p style='margin: 5px 0; color: #7f8c8d; font-size: 14px;'>Your health, our priority</p>
                        </div>

                        <h2 style='color: #1a73e8; text-align: center;'>Email Verification</h2>

                        <p style='font-size: 16px; color: #34495e;'>Hello,</p>

                        <p style='font-size: 16px; color: #34495e;'>
                            Thank you for registering with <strong>Clinic Management System</strong>!  
                            To activate your account and access our healthcare services, please verify your email using the code below:
                        </p>

                        <div style='background-color: #f0f8ff; padding: 20px; text-align: center; font-size: 30px; font-weight: bold; color: #1a73e8; letter-spacing: 3px; border-radius: 6px; margin: 25px 0;'>
                            {Code}
                        </div>

                            <p style='font-size: 14px; color: #7f8c8d;'>
                                This verification code will expire in <strong>30 minutes</strong>.  
                                If you did not initiate this request, please ignore this email.
                            </p>

                            <hr style='border: none; border-top: 1px solid #ecf0f1; margin: 35px 0;' />

                            <p style='font-size: 12px; color: #95a5a6; text-align: center;'>
                                &copy; {DateTime.Now.Year} Clinic Management System. All rights reserved.<br />
                                Wishing you good health and wellness 🌿
                            </p>
                        </div>";
        }
        public static string GetResetPasswordTemplate(string code)
        {
            return $@"<div style='font-family: Segoe UI, Roboto, sans-serif; max-width: 600px; margin: 0 auto; background-color: #ffffff; border: 1px solid #e0e0e0; padding: 30px; border-radius: 10px; box-shadow: 0 8px 20px rgba(0, 0, 0, 0.08);'>
    <div style='text-align: center; margin-bottom: 30px;'>
        <h1 style='margin: 0; color: #2c3e50;'>🩺 Clinic Management System</h1>
        <p style='margin: 5px 0; color: #7f8c8d; font-size: 14px;'>Your healthcare, simplified</p>
    </div>

    <h2 style='color: #1a73e8; text-align: center;'>Reset Your Password</h2>

    <p style='font-size: 16px; color: #34495e;'>Dear User,</p>

    <p style='font-size: 16px; color: #34495e;'>
        We received a request to reset your password for your Clinic Management System account. 
        Please use the verification code below to proceed:
    </p>

    <div style='background-color: #f0f8ff; padding: 20px; text-align: center; font-size: 30px; font-weight: bold; color: #1a73e8; letter-spacing: 3px; border-radius: 6px; margin: 25px 0;'>
        {code}
    </div>

    <p style='font-size: 14px; color: #7f8c8d;'>
        This code will expire in <strong>10 minutes</strong>. 
        If you didn’t request this, please ignore this email.
    </p>

    <hr style='border: none; border-top: 1px solid #ecf0f1; margin: 35px 0;' />

    <p style='font-size: 12px; color: #95a5a6; text-align: center;'>
        &copy; {DateTime.Now.Year} Clinic Management System. All rights reserved.
    </p>
</div>";

        }
    }
}
