using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AccessToDB.Exceptions;
using WebApplication1.Models;
using WebApplication1.Options;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ApiController]
    public class usersController : ControllerBase
    {
        private BL.Facade _facade;
        public usersController(BL.Facade facade)
        {
            _facade = facade;
        }

        // GET: users
        /// <summary>
        /// Get information about all users
        /// </summary>
        /// <returns>Information about all users</returns>
        /// <response code="200" cref="ListOfUserDTO">Information about all users</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserAccount>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
                List<BL.Models.User> users = await _facade.AdminGetUsersAsync(_userID);
                List<UserAccount> usersDTO = Converters.UserAccountConverter.ConvertUsersToUsersDTO(users);
                string usersJSON = JsonSerializer.Serialize(usersDTO, OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = usersJSON,
                    StatusCode = 200
                };
            }
            catch (BL.Exceptions.PermissionExceptions.PermissionException ex)
            {
                string errorMessage = JsonSerializer.Serialize("You are not authorized for this option", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 401
                };
            }
        }

        // GET: users/1
        /// <summary>
        /// Get information about a user by it's ID
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>A user with the specified ID</returns>
        /// <response code="200" cref="User">User with specified ID</response>
        /// <response code="404">User with specified ID not found</response>
        [HttpGet("{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAccount))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] uint userID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                BL.Models.User user = await _facade.AdminGetUserByIDAsync(_userID, userID);
                UserAccount userWithSlopesDTO = Converters.UserAccountConverter.ConvertUserToUserAccountDTO(user);
                string userJSON = JsonSerializer.Serialize(userWithSlopesDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = userJSON,
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.UserExceptions.UserNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("User with specified ID not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
            catch (BL.Exceptions.PermissionExceptions.PermissionException ex)
            {
                string errorMessage = JsonSerializer.Serialize("You are not authorized for this option", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 401
                };
            }
        }

        // POST: users
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="userEmail">Email of user to add</param>
        /// <param name="userPassword">Password of user to add</param>
        /// <param name="role">Role of the user to add</param>
        /// <param name="cardID">ID of the user's card</param>
        /// <returns>The added user with assigned ID</returns>
        /// <response code="201" cref="User">The added user with assigned ID</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserAccount))]
        public async Task<IActionResult> Post([FromQuery] string userEmail, [FromQuery] string userPassword, [FromQuery] string role, [FromQuery] uint cardID)
        {
            try
            {
                uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
                uint userID = await _facade.AdminAddAutoIncrementUserAsync(_userID, cardID, userEmail, userPassword, Converters.UserAccountConverter.RoleToPermissions(role));
                UserAccount userDTO = new UserAccount(userEmail, userPassword, role, cardID);
                string userJSON = JsonSerializer.Serialize(userDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = userJSON,
                    StatusCode = 201
                };
            }
            catch (BL.Exceptions.PermissionExceptions.PermissionException ex)
            {
                string errorMessage = JsonSerializer.Serialize("You are not authorized for this option", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 401
                };
            }
        }

        // PUT: users/1
        /// <summary>
        /// Update information about an existing user
        /// </summary>
        /// <param name="userID">ID of the user to update</param>
        /// <param name="userEmail">Email of user to add</param>
        /// <param name="userPassword">Password of user to add</param>
        /// <param name="role">Role of the user to add</param>
        /// <param name="cardID">ID of the user's card</param>
        /// <returns></returns>
        /// <response code="200">The user was successfully updated</response>
        /// <response code="404">A user with specified ID was not found</response>
        [HttpPut("{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] uint userID, [FromQuery] string userEmail, [FromQuery] string userPassword, [FromQuery] string role, [FromQuery] uint cardID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _facade.AdminUpdateUserAsync(_userID, userID, cardID, userEmail, userPassword, Converters.UserAccountConverter.RoleToPermissions(role));
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.UserExceptions.UserUpdateException)
            {
                string errorMessage = JsonSerializer.Serialize("User with specified ID not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
            catch (BL.Exceptions.PermissionExceptions.PermissionException ex)
            {
                string errorMessage = JsonSerializer.Serialize("You are not authorized for this option", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 401
                };
            }
        }

        // DELETE: users/1
        /// <summary>
        /// Delete a user by it's ID
        /// </summary>
        /// <param name="userID">ID of the user to delete</param>
        /// <returns></returns>
        /// <response code="200" cref="User">User was successfully deleted</response>
        /// <response code="404">User with specified ID not found</response>
        [HttpDelete("{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] uint userID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _facade.AdminDeleteUserAsync(_userID, userID);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.UserExceptions.UserDeleteException)
            {
                string errorMessage = JsonSerializer.Serialize("User with specified ID not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
            catch (BL.Exceptions.PermissionExceptions.PermissionException ex)
            {
                string errorMessage = JsonSerializer.Serialize("You are not authorized for this option", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 401
                };
            }
        }
    }
}