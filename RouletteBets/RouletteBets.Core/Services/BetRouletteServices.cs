using MongoDB.Driver;
using System;
using System.Collections.Generic;
using RouletteBets.DataBase.Modelo;

namespace RouletteBets.Core.Services
{
    public class BetRouletteServices : IBetRouletteServices
    {
        private readonly IMongoCollection<BetRoulette> contextBetRoulette;
        private BetRoulette betRoulette;
        public BetRouletteServices(IDbClient dbClient)
        {
            betRoulette = new BetRoulette();
            contextBetRoulette = dbClient.GetBetRouletteCollection();
        }
        public List<BetRoulette> GetBetRoulette(string idRoulette) =>
           contextBetRoulette.Find(betRoulette => betRoulette.IdRoulette == idRoulette).ToList();
        public void SetWinnerRoulette(string idRoulette, int numberWinner)
        {
            List<BetRoulette> listBetRoulette = contextBetRoulette.Find(betRoulette => betRoulette.IdRoulette == idRoulette).ToList();
            string colorNumberWinner = ColorNumberPosition(numberWinner);
            foreach (var BetRoulette in listBetRoulette)
            {
                if (BetRoulette.NumberPosition == 37 && colorNumberWinner == "Black")//37 Black
                {
                    BetRoulette.Color = ColorNumberPosition(BetRoulette.NumberPosition);
                    BetRoulette.BetProfit = (BetRoulette.MoneyBet * 1.8);
                    BetRoulette.Winner = true;
                    UpdateBetRoulette(BetRoulette);
                }
                else if (BetRoulette.NumberPosition == 38 && colorNumberWinner == "Red")//38 Red
                {
                    BetRoulette.Color = ColorNumberPosition(BetRoulette.NumberPosition);
                    BetRoulette.BetProfit = (BetRoulette.MoneyBet * 1.8);
                    BetRoulette.Winner = true;
                    UpdateBetRoulette(BetRoulette);
                }
                else if (BetRoulette.NumberPosition == numberWinner)
                {
                    BetRoulette.Color = ColorNumberPosition(BetRoulette.NumberPosition);
                    BetRoulette.BetProfit = (BetRoulette.MoneyBet * 5);
                    BetRoulette.Winner = true;
                    UpdateBetRoulette(BetRoulette);
                }
            }
        }
        public BetRoulette UpdateBetRoulette(BetRoulette betRoulette)
        {
            if (betRoulette != null)
            {
                contextBetRoulette.ReplaceOne(r => r.Id == betRoulette.Id, betRoulette);
                return betRoulette;
            }
            else
            {
                return betRoulette;
            }
        }
        public BetRoulette CreateBetRoulette(BetRoulette betRoulette)
        {
            contextBetRoulette.InsertOne(betRoulette);
            return betRoulette;
        }
        public BetRoulette BetRoulette(Roulette roulette, string userId, string userName, int numberPosition, double moneyBet)
        {
            ValidateRoulette(roulette);
            ValidateBet(moneyBet);
            betRoulette.NumberPosition = numberPosition;
            betRoulette.MoneyBet = moneyBet;
            betRoulette.UserId = userId;
            betRoulette.UserName = userName;
            betRoulette.Color = ColorNumberPosition(numberPosition);
            betRoulette.IdRoulette = roulette.Id;

            return CreateBetRoulette(betRoulette);
        }
        public BetRoulette ClosingRoulette(BetRoulette betRoulette)
        {
            contextBetRoulette.InsertOne(betRoulette);
            return betRoulette;
        }
        public string ColorNumberPosition(int numberPosition)
        {
            string color = string.Empty;
            if ((numberPosition % 2) == 0)
            {
                color = "Red";
            }
            else
            {
                color = "Black";
            }
            return color;
        }
        private void ValidateRoulette(Roulette roulette)
        {

            if (roulette == null)
            {
                throw new ArgumentException("Roulette Not Found", nameof(roulette.Id));
            }
            if (roulette.OpenRoulette == false)
            {
                throw new ArgumentException("Roulette Closed", nameof(roulette.Id));
            }
        }
        private void ValidateBet(double moneyBet)
        {
            if (moneyBet > 10000 || moneyBet < 1)
            {
                throw new ArgumentException("Cash Out Range Invalid", nameof(moneyBet));
            }
        }

    }
}
