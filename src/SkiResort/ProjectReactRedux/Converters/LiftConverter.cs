using System.Collections.Generic;
namespace ProjectReactRedux.Converters
{
    public class LiftConverter
    {
        public static ProjectReactRedux.Models.Lift ConvertLiftToLiftDTO(BL.Models.Lift lift)
        {
            List<string> connectedSlopeNames = new List<string>();
            foreach (BL.Models.Slope slope in lift.ConnectedSlopes)
                connectedSlopeNames.Add(slope.SlopeName);
            return new ProjectReactRedux.Models.Lift(lift.LiftID, lift.LiftName, lift.IsOpen, lift.SeatsAmount, lift.LiftingTime, lift.QueueTime, connectedSlopeNames);
        }
        public static List<ProjectReactRedux.Models.Lift> ConvertLiftsToLiftsDTO(List<BL.Models.Lift> lifts)
        {
            List<ProjectReactRedux.Models.Lift> resultList = new List<ProjectReactRedux.Models.Lift>();
            foreach (var lift in lifts)
            {
                resultList.Add(ConvertLiftToLiftDTO(lift));
            }
            return resultList;
        }

    }
}
