namespace WebApplication1.Converters
{
    public class SlopeConverter
    {
        public static WebApplication1.Models.SlopeDTO ConvertSlopeToSlopeDTO(BL.Models.Slope slope)
        {
            return new WebApplication1.Models.SlopeDTO(slope.SlopeID, slope.SlopeName, slope.IsOpen, slope.DifficultyLevel);
        }
        public static List<WebApplication1.Models.SlopeDTO> ConvertSlopesToSlopesDTO(List<BL.Models.Slope> slopes)
        {
            List<WebApplication1.Models.SlopeDTO> resultList = new List<WebApplication1.Models.SlopeDTO>();
            foreach (var slope in slopes)
            {
                resultList.Add(ConvertSlopeToSlopeDTO(slope));
            }
            return resultList;
        }
        public static WebApplication1.Models.SlopeWithLiftsDTO ConvertSlopeToSlopeWithLiftsDTO(BL.Models.Slope slope)
        {
            return new WebApplication1.Models.SlopeWithLiftsDTO(slope.SlopeID, slope.SlopeName, slope.IsOpen, slope.DifficultyLevel, slope.ConnectedLifts);
        }

    }
}
