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
    public class LiftsController : ControllerBase
    {
        private BL.Facade _facade;
        private uint _userID = 1;
        public LiftsController()
        {
            _facade = OtherOptions.createFacade();
        }

        // GET: lifts
        /// <summary>
        /// Get information about all lifts
        /// </summary>
        /// <returns>Information about all lifts</returns>
        /// <response code="200" cref="ListOfLiftDTO">Information about all lifts</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LiftDTO>))]
        public async Task<IActionResult> Get()
        {
            List<BL.Models.Lift> lifts = await _facade.GetLiftsInfoAsync(_userID);
            List<LiftDTO> liftsDTO = Converters.LiftConverter.ConvertLiftsToLiftsDTO(lifts);
            string liftsJSON = JsonSerializer.Serialize(liftsDTO, OtherOptions.JsonOptions());
            return new ContentResult
            {
                Content = liftsJSON,
                StatusCode = 200
            };
        }

        // GET: lifts/A0
        /// <summary>
        /// Get information about a lift by it's name
        /// </summary>
        /// <param name="liftName">Name of the lift</param>
        /// <returns>A lift with the specified name</returns>
        /// <response code="200" cref="LiftWithSlopesDTO">Lift with specified name</response>
        /// <response code="404">Lift with specified name not found</response>
        [HttpGet("{liftName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LiftWithSlopesDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string liftName)
        {
            try
            {
                BL.Models.Lift lift = await _facade.GetLiftInfoAsync(_userID, liftName);
                LiftWithSlopesDTO liftWithSlopesDTO = Converters.LiftConverter.ConvertLiftToLiftWithSlopesDTO(lift);
                string liftJSON = JsonSerializer.Serialize(liftWithSlopesDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = liftJSON,
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.LiftExceptions.LiftNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Lift with specified name not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }         
        }

        // POST: lifts
        /// <summary>
        /// Add a new lift
        /// </summary>
        /// <param name="liftName">Name of the new lift</param>
        /// <param name="isOpen">Is the new lift currently working or not</param>
        /// <param name="seatsAmount">The amount of seats in the new lift</param>
        /// <param name="liftingTime">The time the new lift needs to lift from the beginning to the end</param>
        /// <returns>The added lift with assigned ID</returns>
        /// <response code="200" cref="LiftDTO">The added lift with assigned ID</response>
        /// <response code="400">A lift with such name already exists</response>
        /// /// <remarks>
        /// Note that the names of lifts should be unique.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LiftDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromQuery] string liftName, [FromQuery] bool isOpen, [FromQuery] uint seatsAmount, [FromQuery] uint liftingTime)
        {
            try
            {
                uint liftID = await _facade.AdminAddAutoIncrementLiftAsync(_userID, liftName, isOpen, seatsAmount, liftingTime);
                LiftDTO liftDTO = new LiftDTO(liftID, liftName, isOpen, seatsAmount, liftingTime);
                string liftJSON = JsonSerializer.Serialize(liftDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = liftJSON,
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.LiftExceptions.LiftAddAutoIncrementException)
            {
                string errorMessage = JsonSerializer.Serialize("A lift with such name already exists", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 400
                };
            }
        }

        // PATCH: lifts/A0
        /// <summary>
        /// Update information about an existing lift
        /// </summary>
        /// <param name="liftName">Name of the lift to update</param>
        /// <param name="patchLift">Infromation about lift to be updated</param>
        /// <returns></returns>
        /// <response code="200">The lift was successfully updated</response>
        /// <response code="404">A lift with specified name was not found</response>
        [HttpPatch("{liftName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] string liftName, [FromQuery] PatchLiftDTO patchLift)
        {
            try
            {
                BL.Models.Lift liftFromDB = await _facade.GetLiftInfoAsync(_userID, liftName);

                bool isOpen = patchLift.IsFieldPresent(nameof(patchLift.IsOpen)) ? patchLift.IsOpen : liftFromDB.IsOpen;
                uint seatsAmount = patchLift.IsFieldPresent(nameof(patchLift.SeatsAmount)) ? patchLift.SeatsAmount : liftFromDB.SeatsAmount;
                uint liftingTime = patchLift.IsFieldPresent(nameof(patchLift.LiftingTime)) ? patchLift.LiftingTime : liftFromDB.LiftingTime;

                await _facade.UpdateLiftInfoAsync(_userID, liftName, isOpen, seatsAmount, liftingTime);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.LiftExceptions.LiftNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Lift with specified name not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
        }
        //// PATCH: lifts/A0
        ///// <summary>
        ///// Update information about an existing lift
        ///// </summary>
        ///// <param name="liftName">Name of the lift to update</param>
        ///// <param name="isOpen">Is the lift currently working or not</param>
        ///// <param name="seatsAmount">The amount of seats in the lift to update</param>
        ///// <param name="liftingTime">The time the lift needs to lift from the beginning to the end</param>
        ///// <returns></returns>
        ///// <response code="200">The lift was successfully updated</response>
        ///// <response code="404">A lift with specified name was not found</response>
        //[HttpPatch("{liftName}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> Put([FromRoute] string liftName, [FromQuery] bool isOpen, [FromQuery] uint seatsAmount, [FromQuery] uint liftingTime)
        //{
        //    try
        //    {
        //        await _facade.UpdateLiftInfoAsync(_userID, liftName, isOpen, seatsAmount, liftingTime);
        //        return new ContentResult
        //        {
        //            StatusCode = 200
        //        };
        //    }
        //    catch (AccessToDB.Exceptions.LiftExceptions.LiftNotFoundException)
        //    {
        //        string errorMessage = JsonSerializer.Serialize("Lift with specified name not found", OtherOptions.JsonOptions());
        //        return new ContentResult
        //        {
        //            Content = errorMessage,
        //            StatusCode = 404
        //        };
        //    }
        //}

        // DELETE: lifts/A0
        /// <summary>
        /// Delete a lift by it's name
        /// </summary>
        /// <param name="liftName">Name of the lift</param>
        /// <returns></returns>
        /// <response code="200" cref="LiftDTO">Lift was successfully deleted</response>
        /// <response code="404">Lift with specified name not found</response>
        [HttpDelete("{liftName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string liftName)
        {
            try
            {
                await _facade.AdminDeleteLiftAsync(_userID, liftName);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.LiftExceptions.LiftNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Lift with specified name not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
                };
            }
        }

        // PUT: lifts/A0
        /// <summary>
        /// Add a connected slope to the lift
        /// </summary>
        /// <param name="liftName">Name of the lift to update</param>
        /// <param name="slopeName">Name of the slope to add</param>
        /// <returns></returns>
        /// <response code="200">The lift was successfully updated</response>
        /// <response code="404">A lift or a slope with specified name was not found</response>
        [HttpPut("{liftName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddConnectedSlope([FromRoute] string liftName, [FromQuery] string slopeName)
        {
            try
            {
                uint recordID = await _facade.AdminAddAutoIncrementLiftSlopeAsync(_userID, liftName, slopeName);
                return new ContentResult
                {
                    StatusCode = 200
                };
            }
            catch (AccessToDB.Exceptions.LiftExceptions.LiftNotFoundException)
            {
                string errorMessage = JsonSerializer.Serialize("Lift with specified name not found", OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = errorMessage,
                    StatusCode = 404
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