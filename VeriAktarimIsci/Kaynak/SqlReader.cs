using Dapper;
using System.Data.SqlClient;
using VeriAktarimIsci.Model;

namespace VeriAktarimIsci.Kaynak;

public sealed class SqlReader
{
    private readonly string _conn;

    public SqlReader(string conn) => _conn = conn;

    public async Task<IReadOnlyList<SatirDto>> GetSayfaAsync(int sayfa, int boyut, CancellationToken token)
    {
        const string sql = """
    SELECT  
        Id,
        ActivityLogTypeId,
        CustomerId,
        Comment,
        LogCreatedOnUtc,
        Email,
        HasShoppingCartItems,
        Active,
        Deleted,
        LastIpAddress,
        AccountCreatedOnUtc,
        LastLoginDateUtc,
        LastActivityDateUtc,
        BillingAddress_Id AS BillingAddressId,
        ShippingAddress_Id AS ShippingAddressId,
        Reference,
        AuthenticationTypeID AS AuthenticationTypeId,
        AuthenticatedDateOnUTC AS AuthenticatedDateOnUtc,
        SMSAuthenticatedDateOnUTC AS SmsAuthenticatedDateOnUtc,
        SMSAuthenticationTypeID AS SmsAuthenticationTypeId,
        MobilePlatform
    FROM dbo.Logs
    ORDER BY Id
    OFFSET @o ROWS FETCH NEXT @t ROWS ONLY;
    """;

        await using var con = new SqlConnection(_conn);
        return (await con.QueryAsync<SatirDto>(new CommandDefinition(
                    sql,
                    new { o = sayfa * boyut, t = boyut },
                    cancellationToken: token)))
                .ToList();
    }
}
