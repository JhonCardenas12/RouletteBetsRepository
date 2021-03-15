using RouletteBets.DataBase.Modelo;
using System.Collections.Generic;

namespace RouletteBets.Core.Services
{
    public interface IBetRouletteServices
    {
        BetRoulette CreateBetRoulette(BetRoulette betRoulette);
        BetRoulette UpdateBetRoulette(BetRoulette betRoulette);
        List<BetRoulette> GetBetRoulette(string idRoulette);
        void SetWinnerRoulette(string idRoulette, int numberWinners);
        BetRoulette ClosingRoulette(BetRoulette betRoulette);
        BetRoulette BetRoulette(Roulette roulette, string userId, string userName, int position, double money);
    }
}
