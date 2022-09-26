namespace WebApplication1.Models
{
    /// <summary>
    /// Information about connections between lifts and slopes
    /// </summary>
    public record class LiftSlopeDTO
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

        public LiftSlopeDTO(uint recordID, uint lLiftID, uint slopeID)
        {
            this.RecordID = recordID;
            this.LiftID = lLiftID;
            this.SlopeID = slopeID;

        }
    }

}

