namespace WebApplication1.Converters
{
    public class CardReadingConverter
    {
        public static WebApplication1.Models.CardReadingDTO ConvertCardReadingToCardReadingDTO(BL.Models.CardReading cardReading)
        {
            return new WebApplication1.Models.CardReadingDTO(cardReading.RecordID, cardReading.TurnstileID, cardReading.CardID, cardReading.ReadingTime);
        }

        public static List<WebApplication1.Models.CardReadingDTO> ConvertCardReadingsToCardReadingsDTO(List<BL.Models.CardReading> cardReadings)
        {
            List < WebApplication1.Models.CardReadingDTO > cardReadingsDTO = new List < WebApplication1.Models.CardReadingDTO >();
            foreach (BL.Models.CardReading cardReading in cardReadings)
            {
                cardReadingsDTO.Add(ConvertCardReadingToCardReadingDTO(cardReading));
            }
            return cardReadingsDTO;
        }
    }
}
