namespace WebApplication1.Converters
{
    public class LiftConverter
    {
        public static WebApplication1.Models.LiftDTO ConvertLiftToLiftDTO(BL.Models.Lift lift)
        {
            return new WebApplication1.Models.LiftDTO(lift.LiftID, lift.LiftName, lift.IsOpen, lift.SeatsAmount, lift.LiftingTime, lift.QueueTime);
        }
        public static List<WebApplication1.Models.LiftDTO> ConvertLiftsToLiftsDTO(List<BL.Models.Lift> lifts)
        {
            List<WebApplication1.Models.LiftDTO> resultList = new List<WebApplication1.Models.LiftDTO>();
            foreach (var lift in lifts)
            {
                resultList.Add(ConvertLiftToLiftDTO(lift));
            }
            return resultList;
        }
        public static WebApplication1.Models.LiftWithSlopesDTO ConvertLiftToLiftWithSlopesDTO(BL.Models.Lift lift)
        {
            return new WebApplication1.Models.LiftWithSlopesDTO(lift.LiftID, lift.LiftName, lift.IsOpen, lift.SeatsAmount, lift.LiftingTime, lift.ConnectedSlopes, lift.QueueTime);
        }

    }
}
