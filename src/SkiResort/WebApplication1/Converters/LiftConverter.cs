namespace WebApplication1.Converters
{
    public class LiftConverter
    {
        public static WebApplication1.Models.Lift ConvertLiftToLiftDTO(BL.Models.Lift lift)
        {
            return new WebApplication1.Models.Lift(lift.LiftID, lift.LiftName, lift.IsOpen, lift.SeatsAmount, lift.LiftingTime, lift.QueueTime);
        }
        public static List<WebApplication1.Models.Lift> ConvertLiftsToLiftsDTO(List<BL.Models.Lift> lifts)
        {
            List<WebApplication1.Models.Lift> resultList = new List<WebApplication1.Models.Lift>();
            foreach (var lift in lifts)
            {
                resultList.Add(ConvertLiftToLiftDTO(lift));
            }
            return resultList;
        }
        public static WebApplication1.Models.LiftWithSlopes ConvertLiftToLiftWithSlopesDTO(BL.Models.Lift lift)
        {
            return new WebApplication1.Models.LiftWithSlopes(lift.LiftID, lift.LiftName, lift.IsOpen, lift.SeatsAmount, lift.LiftingTime, lift.ConnectedSlopes, lift.QueueTime);
        }

    }
}
