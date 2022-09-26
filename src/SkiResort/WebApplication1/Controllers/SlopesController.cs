using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AccessToDB.Exceptions;
using WebApplication1.Models;
using WebApplication1.Options;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class SlopesController : ControllerBase
    {
        private BL.Facade _facade;
        private uint _userID = 1;
        public SlopesController()
        {
            _facade = OtherOptions.createFacade();
        }

        // GET: slopes
        /// <summary>
        /// Get information about all slopes
        /// </summary>
        /// <returns>Information about all slopes</returns>
        /// <response code="200" cref="ListOfSlopeDTO">Information about all slopes</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SlopeDTO>))]
        public async Task<IActionResult> Get()
        {
            List<BL.Models.Slope> slopes = await _facade.GetSlopesInfoAsync(_userID);
            List<SlopeDTO> slopesDTO = Converters.SlopeConverter.ConvertSlopesToSlopesDTO(slopes);
            string slopesJSON = JsonSerializer.Serialize(slopesDTO, OtherOptions.JsonOptions());
            return new ContentResult
            {
                Content = slopesJSON,
                StatusCode = 200
            };
        }

        // GET: slopes/A0
        /// <summary>
        /// Get information about a slope by it's name
        /// </summary>
        /// <param name="slopeName">Name of the slope</param>
        /// <returns>A slope with the specified name</returns>
        /// <response code="200" cref="SlopeWithLiftsDTO">Slope with specified name</response>
        /// <response code="404">Slope with specified name not found</response>
        [HttpGet("{slopeName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlopeWithLiftsDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string slopeName)
        {
            try
            {
                BL.Models.Slope slope = await _facade.GetSlopeInfoAsync(_userID, slopeName);
                SlopeWithLiftsDTO slopeWithLiftsDTO = Converters.SlopeConverter.ConvertSlopeToSlopeWithLiftsDTO(slope);
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
        }

        // POST: slopes
        /// <summary>
        /// Add a new slope
        /// </summary>
        /// <param name="slopeName">Name of the new slope</param>
        /// <param name="isOpen">Is the new slope currently working or not</param>
        /// <param name="difficultyLevel">The difficulty level of the new slope</param>
        /// <returns>The added slope with assigned ID</returns>
        /// <response code="200" cref="SlopeDTO">The added slope with assigned ID</response>
        /// <response code="400">A slope with such name already exists</response>
        /// /// <remarks>
        /// Note that the names of slopes should be unique.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SlopeDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromQuery] string slopeName, [FromQuery] bool isOpen, [FromQuery] uint difficultyLevel)
        {
            try
            {
                uint slopeID = await _facade.AdminAddAutoIncrementSlopeAsync(_userID, slopeName, isOpen, difficultyLevel);
                SlopeDTO slopeDTO = new SlopeDTO(slopeID, slopeName, isOpen, difficultyLevel);
                string slopeJSON = JsonSerializer.Serialize(slopeDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = slopeJSON,
                    StatusCode = 200
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] string slopeName, [FromQuery] bool isOpen, [FromQuery] uint difficultyLevel)
        {
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
        }

        // DELETE: slopes/A0
        /// <summary>
        /// Delete a slope by it's name
        /// </summary>
        /// <param name="slopeName">Name of the slope</param>
        /// <returns></returns>
        /// <response code="200" cref="SlopeDTO">Slope was successfully deleted</response>
        /// <response code="404">Slope with specified name not found</response>
        [HttpDelete("{slopeName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string slopeName)
        {
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
        }
    }
}