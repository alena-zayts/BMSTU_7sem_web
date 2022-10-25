using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AccessToDB.Exceptions;
using Project1.Models;
using Project1.Options;
using Microsoft.AspNetCore.Authorization;

namespace Project1.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ApiController]
    public class turnstilesController : ControllerBase
    {
        private BL.Services.TurnstilesService _turnstilesService;
        private uint _userID = 1;
        public turnstilesController(BL.Services.TurnstilesService turnstilesService)
        {
            _turnstilesService = turnstilesService;
        }

        // GET: turnstiles
        /// <summary>
        /// Get information about all turnstiles
        /// </summary>
        /// <returns>Information about all turnstiles</returns>
        /// <response code="200" cref="ListOfTurnstileDTO">Information about all turnstiles</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Turnstile>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
                List<BL.Models.Turnstile> turnstiles = await _turnstilesService.AdminGetTurnstilesAsync(_userID);
                List<Turnstile> turnstilesDTO = Converters.TurnstileConverter.ConvertTurnstilesToTurnstilesDTO(turnstiles);
                string turnstilesJSON = JsonSerializer.Serialize(turnstilesDTO, OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = turnstilesJSON,
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

        // GET: turnstiles/1
        /// <summary>
        /// Get information about a turnstile by it's ID
        /// </summary>
        /// <param name="turnstileID">Turnstile ID</param>
        /// <returns>A turnstile with the specified ID</returns>
        /// <response code="200" cref="Turnstile">Turnstile with specified ID</response>
        /// <response code="404">Turnstile with specified ID not found</response>
        [HttpGet("{turnstileID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Turnstile))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] uint turnstileID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                BL.Models.Turnstile turnstile = await _turnstilesService.AdminGetTurnstileAsync(_userID, turnstileID);
                Turnstile turnstileWithSlopesDTO = Converters.TurnstileConverter.ConvertTurnstileToTurnstileDTO(turnstile);
                string turnstileJSON = JsonSerializer.Serialize(turnstileWithSlopesDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = turnstileJSON,
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.TurnstileExceptions.TurnstileNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Turnstile with specified ID not found", OtherOptions.JsonOptions());
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

        // POST: turnstiles
        /// <summary>
        /// Add a new turnstile
        /// </summary>
        /// <param name="liftID">ID of the lift to which the turnstile is connected</param>
        /// <param name="isOpen">Is the turnstile currently working or not</param>
        /// <returns>The added turnstile with assigned ID</returns>
        /// <response code="201" cref="Turnstile">The added turnstile with assigned ID</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Turnstile))]
        public async Task<IActionResult> Post([FromQuery] uint liftID, [FromQuery] bool isOpen)
        {
            try
            {
                uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
                uint turnstileID = await _turnstilesService.AdminAddAutoIncrementTurnstileAsync(_userID, liftID, isOpen);
                Turnstile turnstileDTO = new Turnstile(turnstileID, liftID, isOpen);
                string turnstileJSON = JsonSerializer.Serialize(turnstileDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = turnstileJSON,
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

        // PUT: turnstiles/1
        /// <summary>
        /// Update information about an existing turnstile
        /// </summary>
        /// <param name="turnstileID">ID of the turnstile to update</param>
        /// <param name="liftID">ID of the lift to which the turnstile is connected</param>
        /// <param name="isOpen">Is the turnstile currently working or not</param>
        /// <returns></returns>
        /// <response code="200">The turnstile was successfully updated</response>
        /// <response code="404">A turnstile with specified ID was not found</response>
        [HttpPut("{turnstileID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] uint turnstileID, [FromQuery] uint liftID, [FromQuery] bool isOpen)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _turnstilesService.AdminUpdateTurnstileAsync(_userID, turnstileID, liftID, isOpen);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.TurnstileExceptions.TurnstileUpdateException)
            {
                string errorMessage = JsonSerializer.Serialize("Turnstile with specified ID not found", OtherOptions.JsonOptions());
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

        // DELETE: turnstiles/1
        /// <summary>
        /// Delete a turnstile by it's ID
        /// </summary>
        /// <param name="turnstileID">ID of the turnstile to delete</param>
        /// <returns></returns>
        /// <response code="200" cref="Turnstile">Turnstile was successfully deleted</response>
        /// <response code="404">Turnstile with specified ID not found</response>
        [HttpDelete("{turnstileID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] uint turnstileID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _turnstilesService.AdminDeleteTurnstileAsync(_userID, turnstileID);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.TurnstileExceptions.TurnstileDeleteException)
            {
                string errorMessage = JsonSerializer.Serialize("Turnstile with specified ID not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
        }
    }
}