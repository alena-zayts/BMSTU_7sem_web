using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication1.Models; 
using WebApplication1.Options;
using System.Text.Json;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AccountController : Controller
    {

        private BL.Facade _facade;
        private uint _userID = 1;
        public AccountController()
        {
            _facade = OtherOptions.createFacade();
        }

        // POST: account
        /// <summary>
        /// Log in
        /// </summary>
        /// <param name="userEmail">User's email</param>
        /// <param name="userPassword">User's password</param>
        /// <returns>Token?</returns>
        /// <response code="200" cref="JsonResult">Authorization went successfully</response>
        /// <response code="404">User with such email wasn't found</response>
        /// <response code="401">Incorrect password</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LogInAsync([FromQuery] string userEmail, [FromQuery] string userPassword)
        {
            try
            {
                BL.Models.User foundUser = await _facade.LogInAsync(_userID, userEmail, userPassword);
                UserAccountDTO userAccount = Converters.UserAccountConverter.ConvertUserToUserAccountDTO(foundUser);

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
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name
                };

                return Json(response);

                //string ResponseJSON = JsonSerializer.Serialize(response, OtherOptions.JsonOptions());

                //return new ContentResult
                //{
                //    Content = ResponseJSON,
                 //   StatusCode = 200
                //};
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
    }
}