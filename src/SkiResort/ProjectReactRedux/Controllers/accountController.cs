using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ProjectReactRedux.Models; 
using ProjectReactRedux.Options;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ProjectReactRedux.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class accountController : Controller
    {

        private BL.Services.UsersService _usersService;
        public accountController(BL.Services.UsersService usersService)
        {
            _usersService = usersService;
        }

        // POST: account/login
        /// <summary>
        /// Log in
        /// </summary>
        /// <param name="userEmail">User's email</param>
        /// <param name="userPassword">User's password</param>
        /// <returns>Token</returns>
        /// <response code="200" cref="JsonResult">Authorization went successfully</response>
        /// <response code="404">User with such email wasn't found</response>
        /// <response code="401">Incorrect password</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LogInAsync([FromQuery] string userEmail, [FromQuery] string userPassword)
        {
            try
            {
                BL.Models.User foundUser = await _usersService.LogInAsync(userEmail, userPassword);
                UserAccount userAccount = Converters.UserAccountConverter.ConvertUserToUserAccountDTO(foundUser);

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userAccount.UserEmail),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userAccount.Role)
                };

                ClaimsIdentity identity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);


                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                jwt.Payload["userID"] = foundUser.UserID;
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name,
                };

                return Json(response);

            }
            catch (BL.Exceptions.UserExceptions.UserAuthorizationException ex)
            {
                string errorMessage = JsonSerializer.Serialize("Incorrect password", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 401
                };
            }
            catch (AccessToDB.Exceptions.UserExceptions.UserNotFoundException ex)
            {
                string errorMessage = JsonSerializer.Serialize("User with such email wasn't found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
        }

        // POST: account/register
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="userEmail">User's email</param>
        /// <param name="userPassword">User's password</param>
        /// <param name="cardID">User's cardID</param>
        /// <returns>Token</returns>
        /// <response code="200" cref="JsonResult">Registration went successfully</response>
        /// <response code="401">User with such email already exists</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Register([FromQuery] string userEmail, [FromQuery] string userPassword, [FromQuery] uint cardID)
        {
            try
            {
                BL.Models.User newUser = await _usersService.RegisterAsync(cardID, userEmail, userPassword);
                UserAccount userAccount = Converters.UserAccountConverter.ConvertUserToUserAccountDTO(newUser);

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userAccount.UserEmail),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userAccount.Role)     
                };

                ClaimsIdentity identity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);


                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                jwt.Payload["userID"] = newUser.UserID;
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name,
                };

                return Json(response);
            }
            catch (BL.Exceptions.UserExceptions.UserRegistrationException ex)
            {
                string errorMessage = JsonSerializer.Serialize("User with such email already exists", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 401
                };
            }
        }

        //// Get: account
        ///// <summary>
        ///// Continue as anauthorized (just get token)
        ///// </summary>
        ///// <returns>Token</returns>
        ///// <response code="200" cref="JsonResult">Ok</response>
        //[Route("")]
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        //public async Task<IActionResult> ContinueWithoutAccount()
        //{
        //    BL.Models.User newUser = await _facade.LogInAsUnauthorizedAsync();

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimsIdentity.DefaultRoleClaimType, "unathurozied")
        //    };

        //    ClaimsIdentity identity =
        //    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
        //        ClaimsIdentity.DefaultRoleClaimType);


        //    var now = DateTime.UtcNow;
        //    // создаем JWT-токен
        //    var jwt = new JwtSecurityToken(
        //            issuer: AuthOptions.ISSUER,
        //            audience: AuthOptions.AUDIENCE,
        //            notBefore: now,
        //            claims: identity.Claims,
        //            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
        //            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        //    jwt.Payload["userID"] = newUser.UserID;
        //    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


        //    var response = new
        //    {
        //        access_token = encodedJwt,
        //        username = identity.Name,
        //    };

        //    return Json(response);
        //}

        // DELETE: account
        /// <summary>
        /// Log out
        /// </summary>
        /// <returns>New token for unauthorized user </returns>
        /// <response code="200" cref="JsonResult">User was successfully logged out</response>
        /// <response code="401">User was not logged in</response>
        [Authorize(Roles = "admin, authorized, ski_patrol")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete()
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var response = new
            {
                delete_access_token = _bearer_token,
            };

            return Json(response);
            return Json("");
        }

        // TODO
        // GET: account
        /// <summary>
        /// Get information about a user by his token
        /// </summary>
        [Route("")]
        [HttpGet]
        [Authorize(Roles = "admin, authorized, ski_patrol")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAccount))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync()
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                //
                BL.Models.User user = await _usersService.AdminGetUserByIDAsync(0, _userID);
                UserAccount userAccount = Converters.UserAccountConverter.ConvertUserToUserAccountDTO(user);
                userAccount.UserID = _userID;
                string userJSON = JsonSerializer.Serialize(userAccount, OtherOptions.JsonOptions());

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
    }
}