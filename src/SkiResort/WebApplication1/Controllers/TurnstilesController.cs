using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AccessToDB.Exceptions;
using WebApplication1.Models;
using WebApplication1.Options;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TurnstilesController : ControllerBase
    {
        private BL.Facade _facade;
        private uint _userID = 1;
        public TurnstilesController()
        {
            _facade = OtherOptions.createFacade();
        }

        // GET: turnstiles
        /// <summary>
        /// Get information about all turnstiles
        /// </summary>
        /// <returns>Information about all turnstiles</returns>
        /// <response code="200" cref="ListOfTurnstileDTO">Information about all turnstiles</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TurnstileDTO>))]
        public async Task<IActionResult> Get()
        {
            List<BL.Models.Turnstile> turnstiles = await _facade.AdminGetTurnstilesAsync(_userID);
            List<TurnstileDTO> turnstilesDTO = Converters.TurnstileConverter.ConvertTurnstilesToTurnstilesDTO(turnstiles);
            string turnstilesJSON = JsonSerializer.Serialize(turnstilesDTO, OtherOptions.JsonOptions());
            return new ContentResult
            {
                Content = turnstilesJSON,
                StatusCode = 200
            };
        }

        // GET: turnstiles/1
        /// <summary>
        /// Get information about a turnstile by it's ID
        /// </summary>
        /// <param name="turnstileID">Turnstile ID</param>
        /// <returns>A turnstile with the specified ID</returns>
        /// <response code="200" cref="TurnstileDTO">Turnstile with specified ID</response>
        /// <response code="404">Turnstile with specified ID not found</response>
        [HttpGet("{turnstileID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TurnstileDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] uint turnstileID)
        {
            try
            {
                BL.Models.Turnstile turnstile = await _facade.AdminGetTurnstileAsync(_userID, turnstileID);
                TurnstileDTO turnstileWithSlopesDTO = Converters.TurnstileConverter.ConvertTurnstileToTurnstileDTO(turnstile);
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
        }

        // POST: turnstiles
        /// <summary>
        /// Add a new turnstile
        /// </summary>
        /// <param name="liftID">ID of the lift to which the turnstile is connected</param>
        /// <param name="isOpen">Is the turnstile currently working or not</param>
        /// <returns>The added turnstile with assigned ID</returns>
        /// <response code="200" cref="TurnstileDTO">The added turnstile with assigned ID</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TurnstileDTO>))]
        public async Task<IActionResult> Post([FromQuery] uint liftID, [FromQuery] bool isOpen)
        {
            uint turnstileID = await _facade.AdminAddAutoIncrementTurnstileAsync(_userID, liftID, isOpen);
            TurnstileDTO turnstileDTO = new TurnstileDTO(turnstileID, liftID, isOpen);
            string turnstileJSON = JsonSerializer.Serialize(turnstileDTO, OtherOptions.JsonOptions());

            return new ContentResult
            {
                Content = turnstileJSON,
                StatusCode = 200
            };
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
            try
            {
                await _facade.AdminUpdateTurnstileAsync(_userID, turnstileID, liftID, isOpen);
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
        }

        // DELETE: turnstiles/1
        /// <summary>
        /// Delete a turnstile by it's ID
        /// </summary>
        /// <param name="turnstileID">ID of the turnstile to delete</param>
        /// <returns></returns>
        /// <response code="200" cref="TurnstileDTO">Turnstile was successfully deleted</response>
        /// <response code="404">Turnstile with specified ID not found</response>
        [HttpDelete("{turnstileID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] uint turnstileID)
        {
            try
            {
                await _facade.AdminDeleteTurnstileAsync(_userID, turnstileID);
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