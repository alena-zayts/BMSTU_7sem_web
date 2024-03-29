﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IRepositories;
using BL.Models;
using BL.Exceptions.LiftExceptions;

namespace BL.Services
{
    public class LiftsService
    {
        private ILiftsRepository _liftsRepository;
        private IUsersRepository _usersRepository;
        private ILiftsSlopesRepository _liftsSlopesRepository;
        private ITurnstilesRepository _turnstilesRepository;

        public LiftsService(ILiftsRepository liftsRepository, IUsersRepository usersRepository, ILiftsSlopesRepository liftsSlopesRepository, ITurnstilesRepository turnstilesRepository)
        {
            _liftsRepository = liftsRepository;
            _usersRepository = usersRepository;
            _liftsSlopesRepository = liftsSlopesRepository;
            _turnstilesRepository = turnstilesRepository;
        }

        public async Task<Lift> GetLiftInfoAsync(uint requesterUserID, string LiftName)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);

            Lift lift = await _liftsRepository.GetLiftByNameAsync(LiftName);
            Lift liftFull = new(lift, await _liftsSlopesRepository.GetSlopesByLiftIdAsync(lift.LiftID));

            return liftFull;

        }

        public async Task<List<Lift>> GetLiftsInfoAsync(uint requesterUserID, uint offset = 0, uint limit = 0)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);

            List<Lift> lifts = await _liftsRepository.GetLiftsAsync(offset, limit);
            List<Lift> liftsFull = new();

            foreach (Lift lift in lifts)
            {
                liftsFull.Add(new(lift, await _liftsSlopesRepository.GetSlopesByLiftIdAsync(lift.LiftID)));

            }
            return liftsFull;
        }

        public async Task UpdateLiftInfoAsync(uint requesterUserID, string liftName, bool isOpen, uint seatsAmount, uint liftingTime)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);

            uint liftID = (await _liftsRepository.GetLiftByNameAsync(liftName)).LiftID;
            await _liftsRepository.UpdateLiftByIDAsync(liftID, liftName, isOpen, seatsAmount, liftingTime);
        }

        public async Task AdminDeleteLiftAsync(uint requesterUserID, string liftName)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);

            Lift lift = await _liftsRepository.GetLiftByNameAsync(liftName);
            List<Turnstile> connected_turnstiles = await _turnstilesRepository.GetTurnstilesByLiftIdAsync(lift.LiftID);
            if (connected_turnstiles == null)
            {
                throw new LiftDeleteException("Cannot delete lift because it has connected turnstiles");
            }

            List<LiftSlope> lift_slopes = await _liftsSlopesRepository.GetLiftsSlopesAsync();
            foreach (LiftSlope lift_slope in lift_slopes)
            {
                if (lift_slope.LiftID == lift.LiftID)
                {
                    await _liftsSlopesRepository.DeleteLiftSlopesByIDAsync(lift_slope.RecordID);
                }
            }


            await _liftsRepository.DeleteLiftByIDAsync(lift.LiftID);
        }


        public async Task<uint> AdminAddAutoIncrementLiftAsync(uint requesterUserID, string liftName, bool isOpen, uint seatsAmount, uint liftingTime)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _liftsRepository.AddLiftAutoIncrementAsync(liftName, isOpen, seatsAmount, liftingTime);
        }

        public async Task AdminAddLiftAsync(uint requesterUserID, Lift lift)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _liftsRepository.AddLiftAsync(lift.LiftID, lift.LiftName, lift.IsOpen, lift.SeatsAmount, lift.LiftingTime, lift.QueueTime);
        }

    }
}
