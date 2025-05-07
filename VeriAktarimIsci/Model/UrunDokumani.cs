using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VeriAktarimIsci.Model;

// Kolon adlarını tablo DTO'sundakiyle eşleştirdik.
public class UrunDokumani
{
    // Kaynaktaki _id veya Id, Mongo'da _id olacak (unique).
    [BsonId]
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string ManufacturerName { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public string Language { get; set; } = null!;
    public decimal Price { get; set; }
    public string TopCategoryName { get; set; } = null!;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Guncelleme { get; set; }   // aktarım zamanı
}
