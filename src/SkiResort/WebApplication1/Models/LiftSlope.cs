namespace WebApplication1.Models
{
    /// <summary>
    /// Information about connections between lifts and slopes
    /// </summary>
    public record class LiftSlope
    {
        /// <summary>
        /// Record ID
        /// </summary>
        public uint RecordID { get; }
        /// <summary>
        /// Lift ID
        /// </summary>
        public uint LiftID { get; }
        /// <summary>
        /// Slope ID
        /// </summary>
        public uint SlopeID { get; }

        public LiftSlope(uint recordID, uint lLiftID, uint slopeID)
        {
            this.RecordID = recordID;
            this.LiftID = lLiftID;
            this.SlopeID = slopeID;

        }
    }

}

