using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RouletteBets.DataBase.Modelo
{
    public class BetRoulette
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public int NumberPosition { get; set; }
        public Double MoneyBet { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Color { get; set; }
        public double BetProfit { get; set; } = 0d;
        public bool Winner { get; set; } = false;
        public string IdRoulette { get; set; }
    }
}
