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
                ActivityLogTypeId = d.ActivityLogTypeId,
                CustomerId = d.CustomerId,
                Comment = d.Comment,
                LogCreatedOnUtc = d.LogCreatedOnUtc,
                Email = d.Email,
                HasShoppingCartItems = d.HasShoppingCartItems ,
                Active = d.Active ,
                Deleted = d.Deleted,
                LastIpAddress = d.LastIpAddress,
                AccountCreatedOnUtc = d.AccountCreatedOnUtc,
                LastLoginDateUtc = d.LastLoginDateUtc,
                LastActivityDateUtc = d.LastActivityDateUtc,
                BillingAddressId = d.BillingAddressId,
                ShippingAddressId = d.ShippingAddressId,
                Reference = d.Reference,
                AuthenticationTypeId = d.AuthenticationTypeId,
                AuthenticatedDateOnUtc = d.AuthenticatedDateOnUtc,
                SmsAuthenticatedDateOnUtc = d.SmsAuthenticatedDateOnUtc,
                SmsAuthenticationTypeId = d.SmsAuthenticationTypeId,
                MobilePlatform = d.MobilePlatform,

                // Ek alan: her yüklemede güncellenen tarih
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
