using System.Collections.Generic;
namespace ProjectReactRedux.Converters
{
    public class CardConverter
    {
        public static ProjectReactRedux.Models.Card ConvertCardToCardDTO(BL.Models.Card card)
        {
            return new ProjectReactRedux.Models.Card(card.CardID, card.ActivationTime, card.Type);
        }

        public static List<ProjectReactRedux.Models.Card> ConvertCardsToCardsDTO(List<BL.Models.Card> cards)
        {
            List < ProjectReactRedux.Models.Card > cardsDTO = new List < ProjectReactRedux.Models.Card >();
            foreach (BL.Models.Card card in cards)
            {
                cardsDTO.Add(ConvertCardToCardDTO(card));
            }
            return cardsDTO;
        }
    }
}
