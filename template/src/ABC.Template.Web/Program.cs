using ABC.Extensions.Primitives;
using ABC.Template.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prometheus;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

#region SignalR
builder.Services.AddSignalR();
#endregion

#region Prometheus���
builder.Services.AddHealthChecks().ForwardToPrometheus();
builder.Services.AddHttpClient(Options.DefaultName)
        .UseHttpClientMetrics();
#endregion
// Add services to the container.

#region  �ļ�ϵͳ
//TODO: ע���ļ�����Ϊfileprovider���簢���ƶ���洢

#endregion



#region �����֤
var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddSingleton<IConnectionMultiplexer>(p => redis);
builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
//.PersistKeysToFileSystem(new System.IO.DirectoryInfo("d://DataProtection-Keys"));
#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{

#if DEBUG
    options.UseMySql(builder.Configuration.GetConnectionString("MySql"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql")));
    //options.UseInMemoryDatabase("ApplicationDbContext");
    options.LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();


#endif

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

#region SignalR
app.MapHub<ABC.Template.Web.Application.Hubs.ChatHub>("/chat");
#endregion
app.UseHttpMetrics();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();   // ͨ��   /metrics  ����ָ��
});

app.Run();
