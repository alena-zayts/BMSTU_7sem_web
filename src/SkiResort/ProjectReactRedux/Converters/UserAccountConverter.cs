using System;
using System.Collections.Generic;
using BL.Models;

namespace ProjectReactRedux.Converters
{
    public class UserAccountConverter
    {
        public static PermissionsEnum RoleToPermissions(string role)
        {
            switch (role)
            {
                case "admin":
                    return PermissionsEnum.ADMIN;
                case "ski_patrol":
                    return PermissionsEnum.SKI_PATROL;
                case "unathurozied":
                    return PermissionsEnum.UNAUTHORIZED;
                case "authorized":
                    return PermissionsEnum.AUTHORIZED;
                default: throw new Exception("unknown role");
            }
        }

        public static ProjectReactRedux.Models.UserAccount ConvertUserToUserAccountDTO(BL.Models.User user)
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
            

            return new ProjectReactRedux.Models.UserAccount(user.UserEmail, user.Password, role, user.CardID);
        }

        public static List<ProjectReactRedux.Models.UserAccount> ConvertUsersToUsersDTO(List<BL.Models.User> users)
        {
            List<ProjectReactRedux.Models.UserAccount> usersDTO = new List<ProjectReactRedux.Models.UserAccount>();
            foreach (BL.Models.User user in users)
            {
                usersDTO.Add(ConvertUserToUserAccountDTO(user));
            }
            return usersDTO;
        }
    }
}
