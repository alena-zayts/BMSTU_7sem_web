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
    public class CardReadingConverter
    {
        public static BL.Models.CardReading DBToBL(CardReadingDB db_model)
        {
            return new BL.Models.CardReading(db_model.Item1, db_model.Item2, db_model.Item3, DateTimeOffset.FromUnixTimeSeconds((long) db_model.Item4));
        }

        public static CardReadingDB BLToDB(BL.Models.CardReading bl_model)
        {
            return ValueTuple.Create(bl_model.RecordID, bl_model.TurnstileID, bl_model.CardID, (uint) bl_model.ReadingTime.ToUnixTimeSeconds());
        }
        public static CardReadingDBNoIndex BLToDBNoIndex(BL.Models.CardReading bl_model)
        {
            return ValueTuple.Create(bl_model.TurnstileID, bl_model.CardID, (uint)bl_model.ReadingTime.ToUnixTimeSeconds());
        }

    }
}
