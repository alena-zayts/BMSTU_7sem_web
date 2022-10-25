using System.Collections.Generic;

namespace ProjectReactRedux.Converters
{
    public class SlopeConverter
    {
        public static ProjectReactRedux.Models.Slope ConvertSlopeToSlopeDTO(BL.Models.Slope slope)
        {
            return new ProjectReactRedux.Models.Slope(slope.SlopeID, slope.SlopeName, slope.IsOpen, slope.DifficultyLevel);
        }
        public static List<ProjectReactRedux.Models.Slope> ConvertSlopesToSlopesDTO(List<BL.Models.Slope> slopes)
        {
            List<ProjectReactRedux.Models.Slope> resultList = new List<ProjectReactRedux.Models.Slope>();
            foreach (var slope in slopes)
            {
                resultList.Add(ConvertSlopeToSlopeDTO(slope));
            }
            return resultList;
        }
        public static ProjectReactRedux.Models.SlopeWithLifts ConvertSlopeToSlopeWithLiftsDTO(BL.Models.Slope slope)
        {
            return new ProjectReactRedux.Models.SlopeWithLifts(slope.SlopeID, slope.SlopeName, slope.IsOpen, slope.DifficultyLevel, slope.ConnectedLifts);
        }

    }
}
