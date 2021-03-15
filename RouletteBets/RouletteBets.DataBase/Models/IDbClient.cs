using MongoDB.Driver;

namespace RouletteBets.DataBase.Modelo
{
    public interface IDbClient
    {
        IMongoCollection<Roulette> GetRouletteCollection();
        IMongoCollection<BetRoulette> GetBetRouletteCollection();
    }
}
