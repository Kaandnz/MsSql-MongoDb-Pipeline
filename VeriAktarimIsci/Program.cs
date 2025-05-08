using Dapper;
using Hangfire;
using Hangfire.SqlServer;
using MongoDB.Driver;
using VeriAktarimIsci;
using VeriAktarimIsci.Kaynak;
using VeriAktarimIsci.Model;
using VeriAktarimIsci.Servisler;

var builder = Host.CreateApplicationBuilder(args);

// ─── Connection strings ───
var sqlConn = builder.Configuration.GetConnectionString("KaynakDb")!;
var mongoConn = builder.Configuration.GetConnectionString("MongoDb")!;

// ─── DI kayıtları ───
builder.Services.AddSingleton(_ => new SqlReader(sqlConn));

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConn));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>()
                                      .GetDatabase("AktarimTestDb")
                                      .GetCollection<UrunDokumani>("Urunler"));

builder.Services.AddSingleton<IVeriAktarimServisi, VeriAktarimServisi>();

// Varsayılan Worker (şablondaki) – istersen silebilirsin
builder.Services.AddHostedService<Worker>();

// ─── Hangfire ───
builder.Services.AddHangfire(cfg =>
    cfg.UseSimpleAssemblyNameTypeSerializer()
       .UseRecommendedSerializerSettings()
       .UseSqlServerStorage(
            builder.Configuration.GetConnectionString("HangfireDb")!,
            new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(5) }));

builder.Services.AddHangfireServer();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var jobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    jobs.AddOrUpdate<IVeriAktarimServisi>(
        recurringJobId: "sqlden-mongoya-aktarim",
        methodCall: s => s.AktarAsync(CancellationToken.None),
        cronExpression: "*/5 * * * *",          
        timeZone: TimeZoneInfo.Local);
    Console.WriteLine("[INIT] Hangfire job kaydedildi (*/5d)");

    // 3️⃣  Uygulama açılır açılmaz deneme çalıştırmak istersen – TRIGGER
    jobs.Trigger("sqlden-mongoya-aktarim");
    Console.WriteLine("[INIT] İlk tetik gönderildi");
}


await host.RunAsync();
