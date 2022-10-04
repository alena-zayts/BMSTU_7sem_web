using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IRepositories;
using BL.Models;
using BL.Exceptions.UserExceptions;

namespace BL.Services
{
    public class UsersService
    {
        private IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<User> LogInAsUnauthorizedAsync()
        {
            int l = (await _usersRepository.GetUsersAsync()).Count() + 1;
            uint newUserID = await _usersRepository.AddUserAutoIncrementAsync(User.UniversalCardID, $"unauthorized_email_{l}", $"unauthorized_password_{l}", PermissionsEnum.UNAUTHORIZED);
            User unauthorizedUser = await _usersRepository.GetUserByIdAsync(newUserID);
            return unauthorizedUser;
        }

        public async Task<User> RegisterAsync(uint cardID, string email, string password)
        {

            if (email.Length == 0 || password.Length == 0)
            {
                throw new UserRegistrationException($"Could't register new user because of incorrect password or email");
            }


            if (await _usersRepository.CheckUserEmailExistsAsync(email))
            {
                throw new UserRegistrationException($"Could't register new user because such email already exists");
            }

            uint newUserID = await _usersRepository.AddUserAutoIncrementAsync(cardID, email, password, PermissionsEnum.AUTHORIZED);
            User authorizedUser = await _usersRepository.GetUserByEmailAsync(email);
            return authorizedUser;
        }

        public async Task<User> LogInAsync(string email, string password)
        {
            User userFromDB = await _usersRepository.GetUserByEmailAsync(email);

            if (password != userFromDB.Password)
            {
                throw new UserAuthorizationException($"Could't authorize user because of wrong password");
            }

            //User authorizedUser = new(userFromDB.UserID, userFromDB.CardID, userFromDB.UserEmail, userFromDB.Password, PermissionsEnum.AUTHORIZED);
            //await usersRepository.UpdateUserByIDAsync(authorizedUser.UserID, authorizedUser.CardID, authorizedUser.UserEmail, authorizedUser.Password, authorizedUser.Permissions);
            return userFromDB;
        }

        public async Task<User> LogOutAsync(uint requesterUserID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);

            User userFromDB = await _usersRepository.GetUserByIdAsync(requesterUserID);

            User unauthorizedUser = new(userFromDB.UserID, userFromDB.CardID, userFromDB.UserEmail, userFromDB.Password, PermissionsEnum.UNAUTHORIZED);
            await _usersRepository.UpdateUserByIDAsync(unauthorizedUser.UserID, unauthorizedUser.CardID, unauthorizedUser.UserEmail, unauthorizedUser.Password, unauthorizedUser.Permissions);
            return unauthorizedUser;
        }

        public async Task<List<User>> AdminGetUsersAsync(uint requesterUserID, uint offset = 0, uint limit = 0)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _usersRepository.GetUsersAsync(offset, limit);
        }

        public async Task AdminAddUserAsync(uint requesterUserID, uint userID, uint cardID, string userEmail, string password, PermissionsEnum permissions)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _usersRepository.AddUserAsync(userID, cardID, userEmail, password, permissions);
        }

        public async Task<uint> AdminAddAutoIncrementUserAsync(uint requesterUserID, uint cardID, string userEmail, string password, PermissionsEnum permissions)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            IUsersRepository usersRepository = _usersRepository;
            return await usersRepository.AddUserAutoIncrementAsync(cardID, userEmail, password, permissions);
        }

        public async Task<uint> AddUnauthorizedUserAsync()
        {
            var usersAmount = (await _usersRepository.GetUsersAsync()).Count();
            return await _usersRepository.AddUserAutoIncrementAsync(User.UniversalCardID, $"unauthorized{usersAmount + 1}", $"unauthorized{usersAmount + 1}", PermissionsEnum.UNAUTHORIZED);
        }

        public async Task AdminUpdateUserAsync(uint requesterUserID, uint userID, uint newCardID, string newUserEmail, string newPassword, PermissionsEnum newPermissions)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _usersRepository.UpdateUserByIDAsync(userID, newCardID, newUserEmail, newPassword, newPermissions);
        }

        public async Task AdminDeleteUserAsync(uint requesterUserID, uint userToDeleteID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);

            User userFromDB = await _usersRepository.GetUserByIdAsync(userToDeleteID);
            await _usersRepository.DeleteUserByIDAsync(userFromDB.UserID);
        }

        public async Task<User> AdminGetUserByIDAsync(uint requesterUserID, uint userID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _usersRepository.GetUserByIdAsync(userID);
        }

    }
}
