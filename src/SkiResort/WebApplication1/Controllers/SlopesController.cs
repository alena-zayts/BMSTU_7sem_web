using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AccessToDB.Exceptions;
using WebApplication1.Models;
using WebApplication1.Options;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ApiController]
    public class slopesController : ControllerBase
    {
        private BL.Facade _facade;
        public slopesController(BL.Facade facade)
        {
            _facade = facade;
        }

        // GET: slopes
        /// <summary>
        /// Get information about all slopes
        /// </summary>
        /// <returns>Information about all slopes</returns>
        /// <response code="200" cref="ListOfSlopeDTO">Information about all slopes</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Slope>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                uint _userID = BL.Models.User.UnauthorizedUserID;
                List<BL.Models.Slope> slopes = await _facade.GetSlopesInfoAsync(_userID);
                List<Slope> slopesDTO = Converters.SlopeConverter.ConvertSlopesToSlopesDTO(slopes);
                string slopesJSON = JsonSerializer.Serialize(slopesDTO, OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = slopesJSON,
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

        // GET: slopes/A0
        /// <summary>
        /// Get information about a slope by it's name
        /// </summary>
        /// <param name="slopeName">Name of the slope</param>
        /// <returns>A slope with the specified name</returns>
        /// <response code="200" cref="SlopeWithLifts">Slope with specified name</response>
        /// <response code="404">Slope with specified name not found</response>
        [HttpGet("{slopeName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlopeWithLifts))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string slopeName)
        {
            uint _userID = BL.Models.User.UnauthorizedUserID;
            try
            {
                BL.Models.Slope slope = await _facade.GetSlopeInfoAsync(_userID, slopeName);
                SlopeWithLifts slopeWithLiftsDTO = Converters.SlopeConverter.ConvertSlopeToSlopeWithLiftsDTO(slope);
                string slopeJSON = JsonSerializer.Serialize(slopeWithLiftsDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = slopeJSON,
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.SlopeExceptions.SlopeNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Slope with specified name not found", OtherOptions.JsonOptions());
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

        // POST: slopes
        /// <summary>
        /// Add a new slope
        /// </summary>
        /// <param name="slopeName">Name of the new slope</param>
        /// <param name="isOpen">Is the new slope currently working or not</param>
        /// <param name="difficultyLevel">The difficulty level of the new slope</param>
        /// <returns>The added slope with assigned ID</returns>
        /// <response code="201" cref="Slope">The added slope with assigned ID</response>
        /// <response code="400">A slope with such name already exists</response>
        /// /// <remarks>
        /// Note that the names of slopes should be unique.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Slope))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromQuery] string slopeName, [FromQuery] bool isOpen, [FromQuery] uint difficultyLevel)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                uint slopeID = await _facade.AdminAddAutoIncrementSlopeAsync(_userID, slopeName, isOpen, difficultyLevel);
                Slope slopeDTO = new Slope(slopeID, slopeName, isOpen, difficultyLevel);
                string slopeJSON = JsonSerializer.Serialize(slopeDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = slopeJSON,
                    StatusCode = 201
                };
            }
            catch (AccessToDB.Exceptions.SlopeExceptions.SlopeAddAutoIncrementException)
            {
                string errorMessage = JsonSerializer.Serialize("A slope with such name already exists", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 400
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

        // PUT: slopes/A0
        /// <summary>
        /// Update information about an existing slope
        /// </summary>
        /// <param name="slopeName">Name of the slope to update</param>
        /// <param name="isOpen">Is the slope currently working or not</param>
        /// <param name="difficultyLevel">The difficulty level of the new slope</param>
        /// <returns></returns>
        /// <response code="200">The slope was successfully updated</response>
        /// <response code="404">A slope with specified name was not found</response>
        [HttpPut("{slopeName}")]
        [Authorize(Roles = "admin, ski_patrol")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] string slopeName, [FromQuery] bool isOpen, [FromQuery] uint difficultyLevel)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _facade.UpdateSlopeInfoAsync(_userID, slopeName, isOpen, difficultyLevel);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.SlopeExceptions.SlopeNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Slope with specified name not found", OtherOptions.JsonOptions());
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

        // DELETE: slopes/A0
        /// <summary>
        /// Delete a slope by it's name
        /// </summary>
        /// <param name="slopeName">Name of the slope</param>
        /// <returns></returns>
        /// <response code="200" cref="Slope">Slope was successfully deleted</response>
        /// <response code="404">Slope with specified name not found</response>
        [HttpDelete("{slopeName}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string slopeName)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _facade.AdminDeleteSlopeAsync(_userID, slopeName);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.SlopeExceptions.SlopeNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Slope with specified name not found", OtherOptions.JsonOptions());
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