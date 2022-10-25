using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectReactRedux.Models
{
    /// <summary>
    /// Information about a slope
    /// </summary>
    public class Slope
    {
        public Slope(uint slopeID, string slopeName, bool isOpen, uint difficultyLevel)
        {
            SlopeID = slopeID;
            SlopeName = slopeName;
            IsOpen = isOpen;
            DifficultyLevel = difficultyLevel;
        }

        /// <summary>
        /// Slope ID
        /// </summary>
        //[Required] or [DefaultValue("Jon Doe")]
        public uint SlopeID { get; }
        /// <summary>
        /// Slope name
        /// </summary>
        public string SlopeName { get; }
        /// <summary>
        /// Is the slope working right now or not
        /// </summary>
        public bool IsOpen { get; }
        /// <summary>
        /// The difficulty level of the slope
        /// </summary>
        public uint DifficultyLevel { get; }
    }
}
