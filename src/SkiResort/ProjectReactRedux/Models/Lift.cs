using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectReactRedux.Models
{
    /// <summary>
    /// Information about a lift
    /// </summary>
    public class Lift
    {
        public Lift(uint liftID, string liftName, bool isOpen, uint seatsAmount, uint liftingTime, uint queueTime=0)
        {
            LiftID = liftID;
            LiftName = liftName;
            IsOpen = isOpen;
            SeatsAmount = seatsAmount;
            LiftingTime = liftingTime;
            QueueTime = queueTime;
        }

        /// <summary>
        /// Lift ID
        /// </summary>
        //[Required] or [DefaultValue("Jon Doe")]
        public uint LiftID { get; }
        /// <summary>
        /// Lift name
        /// </summary>
        public string LiftName { get; }
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
        /// <summary>
        /// Current time in queue to the lift
        /// </summary>
        [DefaultValue(0)]
        public uint QueueTime { get; }
    }
}
