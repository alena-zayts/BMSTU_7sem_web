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
    public class messagesController : ControllerBase
    {
        private BL.Facade _facade;
        public messagesController(BL.Facade facade)
        {
            _facade = facade;
        }

        // GET: messages
        /// <summary>
        /// Get information about all messages
        /// </summary>
        /// <returns>Information about all messages</returns>
        /// <response code="200" cref="ListOfMessageDTO">Information about all messages</response>
        [Route("")]
        [Authorize(Roles = "admin, ski_patrol")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Message>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
                List<BL.Models.Message> messages = await _facade.GetMessagesAsync(_userID);
                List<Message> messagesDTO = Converters.MessageConverter.ConvertMessagesToMessagesDTO(messages);
                string messagesJSON = JsonSerializer.Serialize(messagesDTO, OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = messagesJSON,
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

        // GET: messages/1
        /// <summary>
        /// Get information about a message by it's ID
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns>A message with the specified ID</returns>
        /// <response code="200" cref="Message">Message with specified ID</response>
        /// <response code="404">Message with specified ID not found</response>
        [HttpGet("{messageID}")]
        [Authorize(Roles = "admin, ski_patrol")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Message))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] uint messageID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                BL.Models.Message message = await _facade.GetMessageAsync(_userID, messageID);
                Message messageWithSlopesDTO = Converters.MessageConverter.ConvertMessageToMessageDTO(message);
                string messageJSON = JsonSerializer.Serialize(messageWithSlopesDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = messageJSON,
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.MessageExceptions.MessageNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Message with specified ID not found", OtherOptions.JsonOptions());
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

        // POST: messages
        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="text">Text of the message</param>
        /// <returns>The added message with assigned ID</returns>
        /// <response code="201" cref="Message">The added message with assigned ID</response>
        [HttpPost]
        [Authorize(Roles = "admin, ski_patrol, authorized")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Message))]
        public async Task<IActionResult> Post([FromQuery] string text)
        {
            try
            {
                uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
                uint messageID = await _facade.SendMessageAsync(_userID, text);
                Message messageDTO = new Message(messageID, _userID, BL.Models.Message.MessageCheckedByNobody, text);
                string messageJSON = JsonSerializer.Serialize(messageDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = messageJSON,
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

        // PUT: messages/1
        /// <summary>
        /// Update information about an existing message
        /// </summary>
        /// <param name="messageID">ID of the message to update</param>
        /// <param name="senderID">ID of the user who sent message</param>
        /// <param name="checkedByID">ID of the user who read message</param>
        /// <param name="text">Text of the message</param>
        /// <returns></returns>
        /// <response code="200">The message was successfully updated</response>
        /// <response code="404">A message with specified ID was not found</response>
        [HttpPut("{messageID}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] uint messageID, [FromQuery] uint senderID, [FromQuery] uint checkedByID, [FromQuery] string text)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _facade.AdminUpdateMessageAsync(_userID, messageID, senderID, checkedByID, text);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.MessageExceptions.MessageUpdateException)
            {
                string errorMessage = JsonSerializer.Serialize("Message with specified ID not found", OtherOptions.JsonOptions());
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

        // PATCH: messages/1
        /// <summary>
        /// Mark message read by current user
        /// </summary>
        /// <param name="messageID">ID of the message to update</param>
        /// <returns></returns>
        /// <response code="200">The message was marked read</response>
        /// <response code="404">A message with specified ID was not found</response>
        /// <response code="400">Couldn't mark message checked because it is alredy checked</response>
        [HttpPatch("{messageID}")]
        [Authorize(Roles = "admin, ski_patrol")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromRoute] uint messageID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _facade.MarkMessageReadByUserAsync(_userID, messageID);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (BL.Exceptions.MessageExceptions.MessageCheckingException)
            {
                string errorMessage = JsonSerializer.Serialize("Couldn't mark message checked because it is alredy checked", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 400
                };
            }
            catch (AccessToDB.Exceptions.MessageExceptions.MessageUpdateException)
            {
                string errorMessage = JsonSerializer.Serialize("Message with specified ID not found", OtherOptions.JsonOptions());
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

        // DELETE: messages/1
        /// <summary>
        /// Delete a message by it's ID
        /// </summary>
        /// <param name="messageID">ID of the message to delete</param>
        /// <returns></returns>
        /// <response code="200" cref="Message">Message was successfully deleted</response>
        /// <response code="404">Message with specified ID not found</response>
        [HttpDelete("{messageID}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] uint messageID)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _facade.AdminDeleteMessageAsync(_userID, messageID);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.MessageExceptions.MessageDeleteException)
            {
                string errorMessage = JsonSerializer.Serialize("Message with specified ID not found", OtherOptions.JsonOptions());
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