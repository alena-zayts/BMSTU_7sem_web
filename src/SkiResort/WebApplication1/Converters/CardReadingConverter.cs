namespace WebApplication1.Converters
{
    public class CardReadingConverter
    {
        public static WebApplication1.Models.CardReading ConvertCardReadingToCardReadingDTO(BL.Models.CardReading cardReading)
        {
            return new WebApplication1.Models.CardReading(cardReading.RecordID, cardReading.TurnstileID, cardReading.CardID, cardReading.ReadingTime);
        }

        public static List<WebApplication1.Models.CardReading> ConvertCardReadingsToCardReadingsDTO(List<BL.Models.CardReading> cardReadings)
        {
            List < WebApplication1.Models.CardReading > cardReadingsDTO = new List < WebApplication1.Models.CardReading >();
            foreach (BL.Models.CardReading cardReading in cardReadings)
            {
                cardReadingsDTO.Add(ConvertCardReadingToCardReadingDTO(cardReading));
            }
            return cardReadingsDTO;
        }
    }
}
