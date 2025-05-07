using MongoDB.Driver;
using VeriAktarimIsci.Kaynak;
using VeriAktarimIsci.Model;

namespace VeriAktarimIsci.Servisler;

public interface IVeriAktarimServisi
{
    Task AktarAsync(CancellationToken token);
}

public sealed class VeriAktarimServisi : IVeriAktarimServisi
{
    private readonly SqlReader _reader;
    private readonly IMongoCollection<UrunDokumani> _col;

    public VeriAktarimServisi(SqlReader reader, IMongoCollection<UrunDokumani> col)
    {
        _reader = reader;
        _col = col;
    }

    public async Task AktarAsync(CancellationToken token)
    {
        const int SayfaBoyutu = 100;     
        int sayfaNo = 0;
        int toplam = 0;

        while (true)
        {
            var satirlar = await _reader.GetSayfaAsync(sayfaNo, SayfaBoyutu, token);
            if (satirlar.Count == 0) break;          

            var docs = satirlar.Select(d => new UrunDokumani
            {
                Id = d.Id,
                Name = d.Name,
                ManufacturerName = d.ManufacturerName,
                CreateDate = d.CreateDate,
                Language = d.Language,
                Price = d.Price,
                TopCategoryName = d.TopCategoryName,
                Guncelleme = DateTime.UtcNow
            }).ToList();

            var ops = docs.Select(x =>
                new ReplaceOneModel<UrunDokumani>(
                    Builders<UrunDokumani>.Filter.Eq(y => y.Id, x.Id), x)
                { IsUpsert = true });

            await _col.BulkWriteAsync(ops, cancellationToken: token);

            toplam += docs.Count;
            sayfaNo += 1;
        }

        Console.WriteLine($"[Hangfire] Aktarım tamam: {toplam} kayıt işlendi.");
    }

}
