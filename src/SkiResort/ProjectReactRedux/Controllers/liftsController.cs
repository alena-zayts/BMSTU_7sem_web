using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AccessToDB.Exceptions;
using ProjectReactRedux.Models;
using ProjectReactRedux.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ProjectReactRedux.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ApiController]
    public class liftsController : ControllerBase
    {
        private BL.Services.LiftsService _liftsService;
        private BL.Services.LiftsSlopesService _liftsSlopeService;
        public liftsController(BL.Services.LiftsService liftsService, BL.Services.LiftsSlopesService liftsSlopesService)
        {
            _liftsService = liftsService;
            _liftsSlopeService = liftsSlopesService;
        }

        // GET: lifts
        /// <summary>
        /// Get information about all lifts
        /// </summary>
        /// <returns>Information about all lifts</returns>
        /// <response code="200" cref="ListOfLiftDTO">Information about all lifts</response>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Lift>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                uint _userID = BL.Models.User.UnauthorizedUserID;
                List<BL.Models.Lift> lifts = await _liftsService.GetLiftsInfoAsync(_userID);
                List<Lift> liftsDTO = Converters.LiftConverter.ConvertLiftsToLiftsDTO(lifts);
                string liftsJSON = JsonSerializer.Serialize(liftsDTO, OtherOptions.JsonOptions());
                return new ContentResult
                {
                    Content = liftsJSON,
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

        // GET: lifts/A0
        /// <summary>
        /// Get information about a lift by it's name
        /// </summary>
        /// <param name="liftName">Name of the lift</param>
        /// <returns>A lift with the specified name</returns>
        /// <response code="200" cref="LiftWithSlopes">Lift with specified name</response>
        /// <response code="404">Lift with specified name not found</response>
        [HttpGet("{liftName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LiftWithSlopes))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string liftName)
        {
            uint _userID = BL.Models.User.UnauthorizedUserID;
            try
            {
                BL.Models.Lift lift = await _liftsService.GetLiftInfoAsync(_userID, liftName);
                LiftWithSlopes liftWithSlopesDTO = Converters.LiftConverter.ConvertLiftToLiftWithSlopesDTO(lift);
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

        // POST: lifts
        /// <summary>
        /// Add a new lift
        /// </summary>
        /// <param name="liftName">Name of the new lift</param>
        /// <param name="isOpen">Is the new lift currently working or not</param>
        /// <param name="seatsAmount">The amount of seats in the new lift</param>
        /// <param name="liftingTime">The time the new lift needs to lift from the beginning to the end</param>
        /// <returns>The added lift with assigned ID</returns>
        /// <response code="201" cref="Lift">The added lift with assigned ID</response>
        /// <response code="400">A lift with such name already exists</response>
        /// /// <remarks>
        /// Note that the names of lifts should be unique.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Lift))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromQuery] string liftName, [FromQuery] bool isOpen, [FromQuery] uint seatsAmount, [FromQuery] uint liftingTime)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                uint liftID = await _liftsService.AdminAddAutoIncrementLiftAsync(_userID, liftName, isOpen, seatsAmount, liftingTime);
                Lift liftDTO = new Lift(liftID, liftName, isOpen, seatsAmount, liftingTime);
                string liftJSON = JsonSerializer.Serialize(liftDTO, OtherOptions.JsonOptions());

                return new ContentResult
                {
                    Content = liftJSON,
                    StatusCode = 201
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
        [Authorize(Roles = "admin, ski_patrol")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] string liftName, [FromQuery] PatchLift patchLift)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                BL.Models.Lift liftFromDB = await _liftsService.GetLiftInfoAsync(_userID, liftName);

                bool isOpen = patchLift.IsFieldPresent(nameof(patchLift.IsOpen)) ? patchLift.IsOpen : liftFromDB.IsOpen;
                uint seatsAmount = patchLift.IsFieldPresent(nameof(patchLift.SeatsAmount)) ? patchLift.SeatsAmount : liftFromDB.SeatsAmount;
                uint liftingTime = patchLift.IsFieldPresent(nameof(patchLift.LiftingTime)) ? patchLift.LiftingTime : liftFromDB.LiftingTime;

                await _liftsService.UpdateLiftInfoAsync(_userID, liftName, isOpen, seatsAmount, liftingTime);
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
        /// <response code="200" cref="Lift">Lift was successfully deleted</response>
        /// <response code="404">Lift with specified name not found</response>
        [HttpDelete("{liftName}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string liftName)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                await _liftsService.AdminDeleteLiftAsync(_userID, liftName);
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddConnectedSlope([FromRoute] string liftName, [FromQuery] string slopeName)
        {
            uint _userID = Options.OtherOptions.getUserIDFromToken(Request);
            try
            {
                uint recordID = await _liftsSlopeService.AdminAddAutoIncrementLiftSlopeAsync(_userID, liftName, slopeName);
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