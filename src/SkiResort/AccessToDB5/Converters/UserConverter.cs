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
    public class UserConverter
    {
        public static BL.Models.User DBToBL(UserDB user_db)
        {
            return new BL.Models.User(user_db.Item1, user_db.Item2, user_db.Item3, user_db.Item4, (BL.Models.PermissionsEnum) user_db.Item5);
        }

        public static UserDB BLToDB(BL.Models.User user_bl)
        {
            return ValueTuple.Create(user_bl.UserID, user_bl.CardID, user_bl.UserEmail, user_bl.Password, (uint) user_bl.Permissions);
        }

        public static UserDBNoIndex BLToDBNoIndex(BL.Models.User user_bl)
        {
            return ValueTuple.Create(user_bl.CardID, user_bl.UserEmail, user_bl.Password, (uint) user_bl.Permissions);
        }
    }
}
