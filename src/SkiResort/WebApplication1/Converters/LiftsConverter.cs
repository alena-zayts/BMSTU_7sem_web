namespace WebApplication1.Converters
{
    public class LiftsConverter
    {
        public static List<WebApplication1.Models.LiftDTO> ConvertLiftsToLiftsDTO(List<BL.Models.Lift> lifts)
        {
            List<WebApplication1.Models.LiftDTO> resultList = new List<WebApplication1.Models.LiftDTO>();
            foreach (var lift in lifts)
            {
                resultList.Add(LiftConverter.ConvertLiftToLiftDTO(lift));
            }
            return resultList;        
        }

    }
}
