namespace WebApplication1.Converters
{
    public class SlopeConverter
    {
        public static WebApplication1.Models.Slope ConvertSlopeToSlopeDTO(BL.Models.Slope slope)
        {
            return new WebApplication1.Models.Slope(slope.SlopeID, slope.SlopeName, slope.IsOpen, slope.DifficultyLevel);
        }
        public static List<WebApplication1.Models.Slope> ConvertSlopesToSlopesDTO(List<BL.Models.Slope> slopes)
        {
            List<WebApplication1.Models.Slope> resultList = new List<WebApplication1.Models.Slope>();
            foreach (var slope in slopes)
            {
                resultList.Add(ConvertSlopeToSlopeDTO(slope));
            }
            return resultList;
        }
        public static WebApplication1.Models.SlopeWithLifts ConvertSlopeToSlopeWithLiftsDTO(BL.Models.Slope slope)
        {
            return new WebApplication1.Models.SlopeWithLifts(slope.SlopeID, slope.SlopeName, slope.IsOpen, slope.DifficultyLevel, slope.ConnectedLifts);
        }

    }
}
