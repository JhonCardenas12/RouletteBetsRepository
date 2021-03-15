using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using RouletteBets.DataBase.Modelo;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace RouletteBets.Core.Services
{
    public class RouletteServices : IRouletteServices
    {
        private readonly IMongoCollection<Roulette> contextRoulette;

        public RouletteServices(IDbClient dbClient)
        {
            contextRoulette = dbClient.GetRouletteCollection();
        }
        public Roulette GetRoulette(string id) => contextRoulette.Find(roulette => roulette.Id == id).FirstOrDefault();
        public List<Roulette> GetRoulette() => contextRoulette.Find(roulette => true).ToList();
        public Roulette CreateRoulette(Roulette roulette)
        {
            contextRoulette.InsertOne(roulette);
            return roulette;
        }
        public Roulette UpdateRoulette(Roulette roulette)
        {
            Roulette existRoulette = GetRoulette(roulette.Id);
            if (existRoulette != null)
            {
                roulette.CreationDate = existRoulette.CreationDate;
                contextRoulette.ReplaceOne(r => r.Id == roulette.Id, roulette);
                return roulette;
            }
            else
            {
                return existRoulette;
            }

        }
        public string FormatDateIso8601()
        {
            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond, DateTimeKind.Utc);
            // ISO8601 with 3 decimal places
            string datetimeFormatIso8601 = dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            
            return datetimeFormatIso8601;
        }
        public DistributedCacheEntryOptions DistributedCacheEntry(int time)
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(time)
            };
            return cacheOptions;

        }
    }
}
