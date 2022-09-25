﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    /// <summary>
    /// Information about a slope
    /// </summary>
    public class SlopeWithLiftsDTO
    {
        public SlopeWithLiftsDTO(uint slopeID, string slopeName, bool isOpen, uint difficultyLevel, List<BL.Models.Lift> connectedLifts)
        {
            SlopeID = slopeID;
            SlopeName = slopeName;
            IsOpen = isOpen;
            DifficultyLevel = difficultyLevel;
            ConnectedLifts = Converters.LiftConverter.ConvertLiftsToLiftsDTO(connectedLifts);
        }

        /// <summary>
        /// Slope ID
        /// </summary>
        //[Required] or [DefaultValue("Jon Doe")]
        public uint SlopeID { get; }
        /// <summary>
        /// Slope name
        /// </summary>
        [Required]
        public string SlopeName { get; }
        /// <summary>
        /// Is the slope working right now or not
        /// </summary>
        [Required]
        public bool IsOpen { get; }
        /// <summary>
        /// The difficulty level of the slope
        /// </summary>
        [Required]
        public uint DifficultyLevel { get; }

        /// <summary>
        /// Lifts connected to the slope
        /// </summary>
        public List<LiftDTO> ConnectedLifts { get; }
    }
}