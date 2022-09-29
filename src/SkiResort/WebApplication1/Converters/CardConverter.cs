namespace WebApplication1.Converters
{
    public class CardConverter
    {
        public static WebApplication1.Models.Card ConvertCardToCardDTO(BL.Models.Card card)
        {
            return new WebApplication1.Models.Card(card.CardID, card.ActivationTime, card.Type);
        }

        public static List<WebApplication1.Models.Card> ConvertCardsToCardsDTO(List<BL.Models.Card> cards)
        {
            List < WebApplication1.Models.Card > cardsDTO = new List < WebApplication1.Models.Card >();
            foreach (BL.Models.Card card in cards)
            {
                cardsDTO.Add(ConvertCardToCardDTO(card));
            }
            return cardsDTO;
        }
    }
}
