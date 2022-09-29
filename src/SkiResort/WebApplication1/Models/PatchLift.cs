using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    /// <summary>
    /// Information about a lift that is sent for update
    /// </summary>
    public class PatchLift : PatchDtoBase
    {
        /// <summary>
        /// Is the lift working right now or not
        /// </summary>
        public bool IsOpen { get; }
        /// <summary>
        /// The amount of seats in the lift
        /// </summary>
        public uint SeatsAmount { get; }
        /// <summary>
        /// The time lift needs to lift from the beginning to the end
        /// </summary>
        public uint LiftingTime { get; }
    }
}
