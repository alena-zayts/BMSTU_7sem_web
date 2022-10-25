using System.Collections.Generic;
namespace ProjectReactRedux.Converters
{
    public class CardReadingConverter
    {
        public static ProjectReactRedux.Models.CardReading ConvertCardReadingToCardReadingDTO(BL.Models.CardReading cardReading)
        {
            return new ProjectReactRedux.Models.CardReading(cardReading.RecordID, cardReading.TurnstileID, cardReading.CardID, cardReading.ReadingTime);
        }

        public static List<ProjectReactRedux.Models.CardReading> ConvertCardReadingsToCardReadingsDTO(List<BL.Models.CardReading> cardReadings)
        {
            List < ProjectReactRedux.Models.CardReading > cardReadingsDTO = new List < ProjectReactRedux.Models.CardReading >();
            foreach (BL.Models.CardReading cardReading in cardReadings)
            {
                cardReadingsDTO.Add(ConvertCardReadingToCardReadingDTO(cardReading));
            }
            return cardReadingsDTO;
        }
    }
}
