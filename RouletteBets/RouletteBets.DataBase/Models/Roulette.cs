using MongoDB.Bson.Serialization.Attributes;

namespace RouletteBets.DataBase.Modelo
{
    public class Roulette
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string NameRoulette { get; set; }
        public string CreationDate { get; set; }
        public string ClosingDate { get; set; }
        public bool OpenRoulette { get; set; } = false;
    }
}
