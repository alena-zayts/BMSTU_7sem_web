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
    public class TurnstileConverter
    {
        public static BL.Models.Turnstile DBToBL(TurnstileDB db_model)
        {
            return new BL.Models.Turnstile(db_model.Item1, db_model.Item2, db_model.Item3);
        }

        public static TurnstileDB BLToDB(BL.Models.Turnstile bl_model)
        {
            return ValueTuple.Create(bl_model.TurnstileID, bl_model.LiftID, bl_model.IsOpen);
        }

        public static TurnstileDBNoIndex BLToDBNoIndex(BL.Models.Turnstile bl_model)
        {
            return ValueTuple.Create(bl_model.LiftID, bl_model.IsOpen);
        }

    }
}
