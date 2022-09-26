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
    public class CardReadingsController : ControllerBase
    {
        private BL.Facade _facade;
        private uint _userID = 1;
        public CardReadingsController()
        {
            _facade = OtherOptions.createFacade();
        }

        // GET: cardReadings
        /// <summary>
        /// Get information about all cardReadings
        /// </summary>
        /// <returns>Information about all cardReadings</returns>
        /// <response code="200" cref="ListOfCardReadingDTO">Information about all cardReadings</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CardReadingDTO>))]
        public async Task<IActionResult> Get()
        {
            List<BL.Models.CardReading> cardReadings = await _facade.AdminGetCardReadingsAsync(_userID);
            List<CardReadingDTO> cardReadingsDTO = Converters.CardReadingConverter.ConvertCardReadingsToCardReadingsDTO(cardReadings);
            string cardReadingsJSON = JsonSerializer.Serialize(cardReadingsDTO, OtherOptions.JsonOptions());
            return new ContentResult
            {
                Content = cardReadingsJSON,
                StatusCode = 200
            };
        }

        // GET: cardReadings/1
        /// <summary>
        /// Get information about a cardReading by it's ID
        /// </summary>
        /// <param name="recordID">CardReading ID</param>
        /// <returns>A cardReading with the specified ID</returns>
        /// <response code="200" cref="CardReadingDTO">CardReading with specified ID</response>
        /// <response code="404">CardReading with specified ID not found</response>
        [HttpGet("{recordID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CardReadingDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] uint recordID)
        {
            try
            {
                BL.Models.CardReading cardReading = await _facade.AdminGetCardReadingAsync(_userID, recordID);
                CardReadingDTO cardReadingWithSlopesDTO = Converters.CardReadingConverter.ConvertCardReadingToCardReadingDTO(cardReading);
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
        }

        // POST: cardReadings
        /// <summary>
        /// Add a new cardReading
        /// </summary>
        /// <param name="turnstileID">ID of the turnstile where the reading took place</param>
        /// <param name="cardID">ID of the card that was read</param>
        /// <param name="readingTime">The time of the card reading</param>
        /// <returns>The added cardReading with assigned ID</returns>
        /// <response code="200" cref="CardReadingDTO">The added cardReading with assigned ID</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CardReadingDTO>))]
        public async Task<IActionResult> Post([FromQuery] uint turnstileID, [FromQuery] uint cardID, [FromQuery] DateTimeOffset readingTime)
        {
            uint recordID = await _facade.AdminAddAutoIncrementCardReadingAsync(_userID, turnstileID, cardID, readingTime);
            CardReadingDTO cardReadingDTO = new CardReadingDTO(recordID, turnstileID, cardID, readingTime);
            string cardReadingJSON = JsonSerializer.Serialize(cardReadingDTO, OtherOptions.JsonOptions());

            return new ContentResult
            {
                Content = cardReadingJSON,
                StatusCode = 200
            };
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
            try
            {
                await _facade.AdminUpdateCardReadingAsync(_userID, recordID, turnstileID, cardID, readingTime);
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
        }

        // DELETE: cardReadings/1
        /// <summary>
        /// Delete a cardReading by it's ID
        /// </summary>
        /// <param name="recordID">ID of the cardReading to delete</param>
        /// <returns></returns>
        /// <response code="200" cref="CardReadingDTO">CardReading was successfully deleted</response>
        /// <response code="404">CardReading with specified ID not found</response>
        [HttpDelete("{recordID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] uint recordID)
        {
            try
            {
                await _facade.AdminDeleteCardReadingAsync(_userID, recordID);
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
        }
    }
}