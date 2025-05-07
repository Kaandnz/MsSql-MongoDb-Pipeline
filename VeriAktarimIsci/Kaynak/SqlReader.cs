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
            SELECT  ISNULL (_id , -1) AS Id,
                    name AS Name,
                    manufacturername as ManufacturerName,
                    createdate AS CreateDate,
                    language AS Language,
                    price As Price,
                    TopCategoryName AS TopCategoryName
            FROM    dbo.Products
            ORDER BY _id
            OFFSET  @o ROWS FETCH NEXT @t ROWS ONLY;
            """;

        await using var con = new SqlConnection(_conn);
        return (await con.QueryAsync<SatirDto>(new CommandDefinition(
                    sql,
                    new { o = sayfa * boyut, t = boyut },
                    cancellationToken: token)))
                .ToList();
    }
}
