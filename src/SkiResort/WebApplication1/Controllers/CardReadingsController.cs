using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AccessToDB.Exceptions;
using WebApplication1.Models;
using WebApplication1.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ApiController]
    public class cardReadingsController : ControllerBase
    {
        private BL.Services.CardReadingsService _cardReadingsService;
        public cardReadingsController(BL.Services.CardReadingsService cardReadingsService)
        {
            _cardReadingsService = cardReadingsService;
        }

        // GET: cardReadings
        /// <summary>
        /// Get information about all cardReadings
        /// </summary>
        /// <returns>Information about all cardReadings</returns>
        /// <response code="200" cref="ListOfCardReadingDTO">Information about all cardReadings</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CardReading>))]
        public async Task<IActionResult> Get()
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                List<BL.Models.CardReading> cardReadings = await _cardReadingsService.AdminGetCardReadingsAsync(_userID);
                List<CardReading> cardReadingsDTO = Converters.CardReadingConverter.ConvertCardReadingsToCardReadingsDTO(cardReadings);
                string cardReadingsJSON = JsonSerializer.Serialize(cardReadingsDTO, OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = cardReadingsJSON,
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

        // GET: cardReadings/1
        /// <summary>
        /// Get information about a cardReading by it's ID
        /// </summary>
        /// <param name="recordID">CardReading ID</param>
        /// <returns>A cardReading with the specified ID</returns>
        /// <response code="200" cref="CardReading">CardReading with specified ID</response>
        /// <response code="404">CardReading with specified ID not found</response>
        [HttpGet("{recordID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CardReading))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] uint recordID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                BL.Models.CardReading cardReading = await _cardReadingsService.AdminGetCardReadingAsync(_userID, recordID);
                CardReading cardReadingWithSlopesDTO = Converters.CardReadingConverter.ConvertCardReadingToCardReadingDTO(cardReading);
                string cardReadingJSON = JsonSerializer.Serialize(cardReadingWithSlopesDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = cardReadingJSON,
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.CardReadingExceptions.CardReadingNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("CardReading with specified ID not found", OtherOptions.JsonOptions());
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

        // POST: cardReadings
        /// <summary>
        /// Add a new cardReading
        /// </summary>
        /// <param name="turnstileID">ID of the turnstile where the reading took place</param>
        /// <param name="cardID">ID of the card that was read</param>
        /// <param name="readingTime">The time of the card reading</param>
        /// <returns>The added cardReading with assigned ID</returns>
        /// <response code="201" cref="CardReading">The added cardReading with assigned ID</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CardReading))]
        public async Task<IActionResult> Post([FromQuery] uint turnstileID, [FromQuery] uint cardID, [FromQuery] DateTimeOffset readingTime)
        {
            try
            {
                uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
                uint recordID = await _cardReadingsService.AdminAddAutoIncrementCardReadingAsync(_userID, turnstileID, cardID, readingTime);
                CardReading cardReadingDTO = new CardReading(recordID, turnstileID, cardID, readingTime);
                string cardReadingJSON = JsonSerializer.Serialize(cardReadingDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = cardReadingJSON,
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

        // PUT: cardReadings/1
        /// <summary>
        /// Update information about an existing cardReading
        /// </summary>
        /// <param name="recordID">ID of the cardReading to update</param>
        /// <param name="turnstileID">ID of the turnstile where the reading took place</param>
        /// <param name="cardID">ID of the card that was read</param>
        /// <param name="readingTime">The time of the card reading</param>
        /// <returns></returns>
        /// <response code="200">The cardReading was successfully updated</response>
        /// <response code="404">A cardReading with specified ID was not found</response>
        [HttpPut("{recordID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] uint recordID, [FromQuery] uint turnstileID, [FromQuery] uint cardID, [FromQuery] DateTimeOffset readingTime)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _cardReadingsService.AdminUpdateCardReadingAsync(_userID, recordID, turnstileID, cardID, readingTime);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.CardReadingExceptions.CardReadingUpdateException)
            {
                string errorMessage = JsonSerializer.Serialize("CardReading with specified ID not found", OtherOptions.JsonOptions());
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

        // DELETE: cardReadings/1
        /// <summary>
        /// Delete a cardReading by it's ID
        /// </summary>
        /// <param name="recordID">ID of the cardReading to delete</param>
        /// <returns></returns>
        /// <response code="200" cref="CardReading">CardReading was successfully deleted</response>
        /// <response code="404">CardReading with specified ID not found</response>
        [HttpDelete("{recordID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] uint recordID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _cardReadingsService.AdminDeleteCardReadingAsync(_userID, recordID);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.CardReadingExceptions.CardReadingDeleteException)
            {
                string errorMessage = JsonSerializer.Serialize("CardReading with specified ID not found", OtherOptions.JsonOptions());
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