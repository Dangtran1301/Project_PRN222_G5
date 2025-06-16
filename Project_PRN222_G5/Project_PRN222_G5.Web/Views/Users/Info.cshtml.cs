using Project_PRN222_G5.DataAccess.DTOs.Users.Requests;
using Project_PRN222_G5.DataAccess.DTOs.Users.Responses;

namespace Project_PRN222_G5.Web.Views.Users
{
    public class InfoPageViewModel
    {
        public UserResponse User { get; set; } = default!;
        public UpdateInfoUser UpdateInfo { get; set; } = default!;
        public ResetPasswordRequest ResetPassword { get; set; } = default!;
    }
}