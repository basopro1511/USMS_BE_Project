using BusinessObject;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using UserService.Repository.UserRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace UserService.Services.UserServices
    {
    public class UserService
        {
        private readonly IUserRepository _userRepository;

        public UserService()
            {
            _userRepository=new UserRepository();
            }

        #region Get All User
        /// <summary>
        /// Get all user
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse> GetAllUser()
            {
            APIResponse aPIResponse = new APIResponse();
            List<UserDTO> users =await _userRepository.GetAllUser();
            if (users==null||users.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy người dùng";
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=users;
                }
            return aPIResponse;
            }

        #endregion

        #region Get User By Id
        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetUserById(string userId)
            {
            APIResponse aPIResponse = new APIResponse();
            UserDTO user = await _userRepository.GetUserById(userId);
            if (user==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy người dùng với mã: " + userId ;
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=user;
                }
            return aPIResponse;
            }
        #endregion

        #region HashPassword
        /// <summary>
        /// Hashing password
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <returns></returns>
        public string HashPassword(string plainPassword)
            {
            using (SHA256 sha256 = SHA256.Create())
                {
                byte[] bytes = Encoding.UTF8.GetBytes(plainPassword);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
                }
            }
        #endregion

        #region Reset Password
        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        public async Task<APIResponse> ResetPassword ( ResetPasswordDTO resetPassword)
            {
            APIResponse aPIResponse = new APIResponse();
            UserDTO user = await _userRepository.GetUserById(resetPassword.UserId);
            if (user==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Không tìm thấy người dùng khả dụng!."
                    };
                }
            resetPassword.oldPassword = HashPassword(resetPassword.oldPassword);
            #region 1. Validation
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (resetPassword.oldPassword != user.PasswordHash, "Mật khẩu cũ không hợp lệ, vui lòng thử lại!"),
                  (resetPassword.newPassword.Length < 8 || resetPassword.newPassword.Length > 36,"Độ dài mật khẩu mới phải từ 8 đến 36 ký tự."),
            };
            foreach (var validation in validations)
                {
                if (validation.condition)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message=validation.errorMessage
                        };
                    }
                }
            #endregion
            resetPassword.newPassword = HashPassword(resetPassword.newPassword);
            bool isSuccess = await _userRepository.ResetPassword(resetPassword);
            if (isSuccess) {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật mật khẩu thành công!"
                    };
                }
            else
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Cập nhật mật khẩu thất bại!"
                    };
                };
            }
        #endregion

        #region Generate OTP
        /// <summary>
        /// Generate OTP to reset password
        /// </summary>
        /// <returns></returns>
        private string GenerateOTP()
            {
            Random random = new Random();
            return random.Next(0, 999999).ToString("D6");
            }
        #endregion
        #region Forgot Password
        /// <summary>
        /// Forgot password send OTP
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<APIResponse> ForgotPassword(string email)
            {
            APIResponse aPIResponse = new APIResponse();
            var isExist = await _userRepository.GetUserByEmail(email);

            if (isExist == null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Email này không tồn tại trong hệ thống!";
                return aPIResponse;
                }
            try
                {
                string fromEmail = "thainam26903@gmail.com";
                string appPassword = "rzsd encn mqbj dwfy";
                ResourceManager rm = new ResourceManager("UserService.Resources.OTP", Assembly.GetExecutingAssembly());
                string template = rm.GetString("OTPTemplate");
                string otp = GenerateOTP();
                string emailBody = template.Replace("@paramOTP", otp);
                MailMessage mail = new MailMessage();
                mail.From=new MailAddress(fromEmail);
                mail.To.Add(email);
                mail.Subject=$"Mã OTP của bạn là: {otp}";
                mail.Body=emailBody;
                mail.IsBodyHtml=true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials=new NetworkCredential(fromEmail, appPassword);
                smtp.EnableSsl=true;
                smtp.Send(mail);
                }
            catch (Exception ex)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Lỗi khi gửi OTP: "+ex.Message;
                }

            return aPIResponse;
            }
        #endregion
        }
    }
