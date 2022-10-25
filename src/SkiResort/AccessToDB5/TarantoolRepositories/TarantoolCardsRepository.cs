using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

using BL;
using BL.Models;
using BL.IRepositories;
using AccessToDB.Converters;
using AccessToDB.Exceptions.CardExceptions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
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

namespace AccessToDB.RepositoriesTarantool
{
    public class TarantoolCardsRepository : ICardsRepository
    {
        private ISpace _space;
        private IIndex _indexPrimary;
        private IBox _box;

        public TarantoolCardsRepository(TarantoolContext context)
        {
            _box = context.box;
            _space = context.cardsSpace;
            _indexPrimary = context.cardsIndexPrimary;
        }

        public async Task<List<Card>> GetCardsAsync(uint offset = 0u, uint limit = 0)
        {
            var data = await _indexPrimary.Select<ValueTuple<uint>, CardDB>
                (ValueTuple.Create(0u), new SelectOptions { Iterator = Iterator.Ge });

            List<Card> result = new();

            for (uint i = offset; i < (uint)data.Data.Length && (i < limit || limit == 0); i++)
            {
                result.Add(CardConverter.DBToBL(data.Data[i]));
            }

            return result;
        }

        public async Task<Card> GetCardByIdAsync(uint CardID)
        {
            var data = await _indexPrimary.Select<ValueTuple<uint>, CardDB>
                (ValueTuple.Create(CardID));

            if (data.Data.Length != 1)
            {
                throw new CardNotFoundException(CardID);
            }

            return CardConverter.DBToBL(data.Data[0]);
        }

        public async Task AddCardAsync(uint cardID, DateTimeOffset activationTime, string type)
        {
            try
            {
                await _space.Insert(new CardDB(cardID, (uint) activationTime.ToUnixTimeSeconds(), type));
            }
            catch (Exception ex)
            {
                throw new CardAddException(cardID, activationTime, type);
            }
        }
        public async Task<uint> AddCardAutoIncrementAsync(DateTimeOffset activationTime, string type)
        {
            try
            {
                var result = await _box.Call_1_6<CardDBNoIndex, CardDB>("auto_increment_cards", (new CardDBNoIndex((uint) activationTime.ToUnixTimeSeconds(), type)));
                return CardConverter.DBToBL(result.Data[0]).CardID;
            }
            catch (Exception ex)
            {
                throw new CardAddAutoIncrementException(activationTime, type);
            }
        }
        public async Task UpdateCardByIDAsync(uint cardID, DateTimeOffset newActivationTime, string newType)
        {
            var response = await _space.Update<ValueTuple<uint>, CardDB>(
                ValueTuple.Create(cardID), new UpdateOperation[] {
                    UpdateOperation.CreateAssign<uint>(1, (uint) newActivationTime.ToUnixTimeSeconds()),
                    UpdateOperation.CreateAssign<string>(2, newType),
                });

            if (response.Data.Length != 1)
            {
                throw new CardUpdateException(cardID, newActivationTime, newType);
            }
        }

        public async Task DeleteCarByIDdAsync(uint cardID)
        {
            var response = await _indexPrimary.Delete<ValueTuple<uint>, CardDB>
                (ValueTuple.Create(cardID));

            if (response.Data.Length != 1)
            {
                throw new CardDeleteException(cardID);
            }

        }
    }
}
