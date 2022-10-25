
namespace AccessToDB.Converters
{
    using UserDB = System.ValueTuple<uint, uint, string, string, uint>;
    using CardReadingDB = System.ValueTuple<uint, uint, uint, uint>;
    using LiftDB = System.ValueTuple<uint, string, bool, uint, uint, uint>;
    using CardDB = System.ValueTuple<uint, uint, string>;
    using LiftSlopeDB = System.ValueTuple<uint, uint, uint>;
    using SlopeDB = System.ValueTuple<uint, string, bool, uint>;
    using TurnstileDB = System.ValueTuple<uint, uint, bool>;
    using MessageDB = System.ValueTuple<uint, uint, uint, string>;


    using UserDBNoIndex = System.ValueTuple<uint, string, string, uint>;
    using CardReadingDBNoIndex = System.ValueTuple<uint, uint, uint>;
    using LiftDBNoIndex = System.ValueTuple<string, bool, uint, uint, uint>;
    using CardDBNoIndex = System.ValueTuple<uint, string>;
    using LiftSlopeDBNoIndex = System.ValueTuple<uint, uint>;
    using SlopeDBNoIndex = System.ValueTuple<string, bool, uint>;
    using TurnstileDBNoIndex = System.ValueTuple<uint, bool>;
    using MessageDBNoIndex = System.ValueTuple<uint, uint, string>;
    using System;
    public class CardConverter
    {
        public static BL.Models.Card DBToBL(CardDB user_db)
        {
            //return new BL.Models.Card(user_db.Item1, new DateTimeOffset((int) user_db.Item2, TimeSpan.Zero), user_db.Item3);
            return new BL.Models.Card(user_db.Item1, DateTimeOffset.FromUnixTimeSeconds(user_db.Item2), user_db.Item3);
        }

        public static CardDB BLToDB(BL.Models.Card card_bl)
        {
            return ValueTuple.Create(card_bl.CardID, (uint) card_bl.ActivationTime.ToUnixTimeSeconds(), card_bl.Type);
        }
        public static CardDBNoIndex BLToDBNoIndex(BL.Models.Card card_bl)
        {
            return ValueTuple.Create((uint)card_bl.ActivationTime.ToUnixTimeSeconds(), card_bl.Type);
        }
    }
}
