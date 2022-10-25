﻿namespace AccessToDB.Converters
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
    public class MessageConverter
    {
        public static BL.Models.Message DBToBL(MessageDB db_model)
        {
            return new BL.Models.Message(db_model.Item1, db_model.Item2, db_model.Item3, db_model.Item4);
        }

        public static MessageDB BLToDB(BL.Models.Message bl_model)
        {
            return ValueTuple.Create(bl_model.MessageID, bl_model.SenderID, bl_model.CheckedByID, bl_model.Text);
        }
        public static MessageDBNoIndex BLToDBNoIndex(BL.Models.Message bl_model)
        {
            return ValueTuple.Create(bl_model.SenderID, bl_model.CheckedByID, bl_model.Text);
        }
    }
}
