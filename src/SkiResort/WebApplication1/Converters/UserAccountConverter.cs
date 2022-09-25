using BL.Models;

namespace WebApplication1.Converters
{
    public class UserAccountConverter
    {
        public static WebApplication1.Models.UserAccountDTO ConvertUserToUserAccountDTO(BL.Models.User user)
        {
            string role = "";
            switch (user.Permissions)
            {
                case PermissionsEnum.ADMIN:
                    role = "admin";
                    break;
                case PermissionsEnum.SKI_PATROL:
                    role = "ski_patrol";
                    break;
                case PermissionsEnum.UNAUTHORIZED:
                    role = "unathurozied";
                    break;
                case PermissionsEnum.AUTHORIZED:
                    role = "authorized";
                    break;
            }
            

            return new WebApplication1.Models.UserAccountDTO(user.UserEmail, user.Password, role);
        }
    }
}
