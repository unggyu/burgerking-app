using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HamburgerEx.Models
{
    [BsonIgnoreExtraElements]
    class Order
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("menu_item_name_kr")]
        public string MenuItemNameKR { get; set; }
        [BsonElement("menu_item_name_en")]
        public string MenuItemNameEN { get; set; }
        [BsonElement("price")]
        public int Price { get; set; }
        [BsonElement("order_time")]
        public BsonDateTime OrderTime { get; set; }
    }
}
