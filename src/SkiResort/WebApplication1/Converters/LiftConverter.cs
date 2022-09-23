namespace WebApplication1.Converters
{
    public class LiftConverter
    {
        public static WebApplication1.Models.LiftDTO ConvertLiftToLiftDTO(BL.Models.Lift lift)
        {
            return new WebApplication1.Models.LiftDTO(lift.LiftID, lift.LiftName, lift.IsOpen, lift.SeatsAmount, lift.LiftingTime, lift.QueueTime);
        }

    }
}
