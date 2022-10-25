namespace BL.Models
{
    public class LiftSlope
    {
        public uint RecordID { get; }
        public uint LiftID { get; }
        public uint SlopeID { get; }

        public LiftSlope(uint recordID, uint lLiftID, uint slopeID)
        {
            this.RecordID = recordID;
            this.LiftID = lLiftID;
            this.SlopeID = slopeID;

        }
    }

}

