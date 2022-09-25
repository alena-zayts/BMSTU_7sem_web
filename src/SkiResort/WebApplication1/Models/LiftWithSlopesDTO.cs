﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    /// <summary>
    /// Information about a lift
    /// </summary>
    public class LiftWithSlopesDTO
    {
        public LiftWithSlopesDTO(uint liftID, string liftName, bool isOpen, uint seatsAmount, uint liftingTime, List<BL.Models.Slope> connectedSlopes, uint queueTime=0)
        {
            LiftID = liftID;
            LiftName = liftName;
            IsOpen = isOpen;
            SeatsAmount = seatsAmount;
            LiftingTime = liftingTime;
            QueueTime = queueTime;
            ConnectedSlopes = Converters.SlopeConverter.ConvertSlopesToSlopesDTO(connectedSlopes);
        }

        /// <summary>
        /// Lift ID
        /// </summary>
        //[Required] or [DefaultValue("Jon Doe")]
        public uint LiftID { get; }
        /// <summary>
        /// Lift name
        /// </summary>
        [Required]
        public string LiftName { get; }
        /// <summary>
        /// Is the lift working right now or not
        /// </summary>
        [Required]
        public bool IsOpen { get; }
        /// <summary>
        /// The amount of seats in the lift
        /// </summary>
        [Required]
        public uint SeatsAmount { get; }
        /// <summary>
        /// The time lift needs to lift from the beginning to the end
        /// </summary>
        [Required]
        public uint LiftingTime { get; }
        /// <summary>
        /// Current time in queue to the lift
        /// </summary>
        [DefaultValue(0)]
        public uint QueueTime { get; }
        /// <summary>
        /// Slopes connected to the lift
        /// </summary>
        public List<SlopeDTO> ConnectedSlopes { get; }
    }
}