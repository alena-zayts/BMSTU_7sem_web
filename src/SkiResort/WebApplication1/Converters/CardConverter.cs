namespace WebApplication1.Converters
{
    public class CardConverter
    {
        public static WebApplication1.Models.CardDTO ConvertCardToCardDTO(BL.Models.Card card)
        {
            return new WebApplication1.Models.CardDTO(card.CardID, card.ActivationTime, card.Type);
        }

        public static List<WebApplication1.Models.CardDTO> ConvertCardsToCardsDTO(List<BL.Models.Card> cards)
        {
            List < WebApplication1.Models.CardDTO > cardsDTO = new List < WebApplication1.Models.CardDTO >();
            foreach (BL.Models.Card card in cards)
            {
                cardsDTO.Add(ConvertCardToCardDTO(card));
            }
            return cardsDTO;
        }
    }
}
