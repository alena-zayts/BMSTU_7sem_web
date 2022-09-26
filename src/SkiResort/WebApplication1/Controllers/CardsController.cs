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
    public class CardsController : ControllerBase
    {
        private BL.Facade _facade;
        private uint _userID = 1;
        public CardsController()
        {
            _facade = OtherOptions.createFacade();
        }

        // GET: cards
        /// <summary>
        /// Get information about all cards
        /// </summary>
        /// <returns>Information about all cards</returns>
        /// <response code="200" cref="ListOfCardDTO">Information about all cards</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CardDTO>))]
        public async Task<IActionResult> Get()
        {
            List<BL.Models.Card> cards = await _facade.AdminGetCardsAsync(_userID);
            List<CardDTO> cardsDTO = Converters.CardConverter.ConvertCardsToCardsDTO(cards);
            string cardsJSON = JsonSerializer.Serialize(cardsDTO, OtherOptions.JsonOptions());
            return new ContentResult
            {
                Content = cardsJSON,
                StatusCode = 200
            };
        }

        // GET: cards/1
        /// <summary>
        /// Get information about a card by it's ID
        /// </summary>
        /// <param name="cardID">Card ID</param>
        /// <returns>A card with the specified ID</returns>
        /// <response code="200" cref="CardDTO">Card with specified ID</response>
        /// <response code="404">Card with specified ID not found</response>
        [HttpGet("{cardID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CardDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] uint cardID)
        {
            try
            {
                BL.Models.Card card = await _facade.AdminGetCardAsync(_userID, cardID);
                CardDTO cardWithSlopesDTO = Converters.CardConverter.ConvertCardToCardDTO(card);
                string cardJSON = JsonSerializer.Serialize(cardWithSlopesDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = cardJSON,
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.CardExceptions.CardNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Card with specified ID not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }         
        }

        // POST: cards
        /// <summary>
        /// Add a new card
        /// </summary>
        /// <param name="activationTime">Time when the new card was activated</param>
        /// <param name="type">The type of the new card</param>
        /// <returns>The added card with assigned ID</returns>
        /// <response code="200" cref="CardDTO">The added card with assigned ID</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CardDTO>))]
        public async Task<IActionResult> Post([FromQuery] DateTimeOffset activationTime, [FromQuery] string type)
        {
            uint cardID = await _facade.AdminAddAutoIncrementCardAsync(_userID, activationTime, type);
            CardDTO cardDTO = new CardDTO(cardID, activationTime, type);
            string cardJSON = JsonSerializer.Serialize(cardDTO, OtherOptions.JsonOptions());

            return new ContentResult
            {
                Content = cardJSON,
                StatusCode = 200
            };
        }

        // PUT: cards/1
        /// <summary>
        /// Update information about an existing card
        /// </summary>
        /// <param name="cardID">ID of the card to update</param>
        /// <param name="activationTime">Time when the new card was activated</param>
        /// <param name="type">The type of the card</param>
        /// <returns></returns>
        /// <response code="200">The card was successfully updated</response>
        /// <response code="404">A card with specified ID was not found</response>
        [HttpPut("{cardID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] uint cardID, [FromQuery] DateTimeOffset activationTime, [FromQuery] string type)
        {
            try
            {
                await _facade.AdminUpdateCardAsync(_userID, cardID, activationTime, type);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.CardExceptions.CardUpdateException)
            {
                string errorMessage = JsonSerializer.Serialize("Card with specified ID not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
        }

        // DELETE: cards/1
        /// <summary>
        /// Delete a card by it's ID
        /// </summary>
        /// <param name="cardID">ID of the card to delete</param>
        /// <returns></returns>
        /// <response code="200" cref="CardDTO">Card was successfully deleted</response>
        /// <response code="404">Card with specified ID not found</response>
        [HttpDelete("{cardID}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] uint cardID)
        {
            try
            {
                await _facade.AdminDeleteCardAsync(_userID, cardID);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.CardExceptions.CardDeleteException)
            {
                string errorMessage = JsonSerializer.Serialize("Card with specified ID not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
        }
    }
}