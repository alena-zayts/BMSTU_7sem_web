﻿using System.Collections.Generic;
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
            List<LiftDTO> liftsDTO = Converters.LiftsConverter.ConvertLiftsToLiftsDTO(lifts);
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
        /// <response code="200" cref="LiftDTO">Lift with specified name</response>
        /// <response code="404">Lift with specified name not found</response>
        [HttpGet("{liftName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LiftDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string liftName)
        {
            try
            {
                BL.Models.Lift lift = await _facade.GetLiftInfoAsync(_userID, liftName);
                LiftDTO liftDTO = Converters.LiftConverter.ConvertLiftToLiftDTO(lift);
                string liftJSON = JsonSerializer.Serialize(liftDTO, OtherOptions.JsonOptions());

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

        // PUT: lifts/A0
        /// <summary>
        /// Update information about an existing lift
        /// </summary>
        /// <param name="liftName">Name of the lift to update</param>
        /// <param name="isOpen">Is the lift currently working or not</param>
        /// <param name="seatsAmount">The amount of seats in the lift to update</param>
        /// <param name="liftingTime">The time the lift needs to lift from the beginning to the end</param>
        /// <returns></returns>
        /// <response code="200">The lift was successfully updated</response>
        /// <response code="404">A lift with specified name was not found</response>
        [HttpPut("{liftName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IActionResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] string liftName, [FromQuery] bool isOpen, [FromQuery] uint seatsAmount, [FromQuery] uint liftingTime)
        {
            try
            {
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
    }
}