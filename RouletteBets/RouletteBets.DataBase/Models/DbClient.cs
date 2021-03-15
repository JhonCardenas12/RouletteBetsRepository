using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RouletteBets.DataBase.Modelo
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<Roulette> roulette;
        private readonly IMongoCollection<BetRoulette> betRoulette;
        public DbClient(IOptions<RouletteBetsDbConfig> rouletteBetsDbConfig)
        {
            var client = new MongoClient(rouletteBetsDbConfig.Value.ConnectionString);
            var database = client.GetDatabase(rouletteBetsDbConfig.Value.DatabaseName);
            roulette = database.GetCollection<Roulette>("Roulette");
            betRoulette = database.GetCollection<BetRoulette>("BetRoulette");
        }

        public IMongoCollection<Roulette> GetRouletteCollection() => roulette;
        public IMongoCollection<BetRoulette> GetBetRouletteCollection() => betRoulette;
    }
}
