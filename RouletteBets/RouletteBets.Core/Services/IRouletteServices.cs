using Microsoft.Extensions.Caching.Distributed;
using RouletteBets.DataBase.Modelo;
using System.Collections.Generic;

namespace RouletteBets.Core.Services
{
    public interface IRouletteServices
    {
        List<Roulette> GetRoulette();
        Roulette GetRoulette(string id);
        Roulette CreateRoulette(Roulette roulette);
        Roulette UpdateRoulette(Roulette roulette);
        string FormatDateIso8601();
        DistributedCacheEntryOptions DistributedCacheEntry(int time);
    }
}
