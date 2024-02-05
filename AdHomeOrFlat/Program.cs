using AdHomeOrFlat.Models;
using AdHomeOrFlat.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;


var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var botCon = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<ConfigureWeb>();
builder.Services.AddScoped<HandleUpdateService>();


builder.Services.AddHttpClient("tgwebhook").AddTypedClient<ITelegramBotClient>(httpClient =>
                    new TelegramBotClient(botCon.Token, httpClient));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseCors();//bitta hostingda turgan API ga boshqa joydan murojat bo'lsa qabul qilmaydi, ignore qiladi

app.UseEndpoints(endpoints =>
{
    var token = botCon.Token;
    endpoints.MapControllerRoute(name: "tgwebhook",
            pattern: $"bot/{token}",
            new { controller = "Web", action = "Post" });
    endpoints.MapControllers();
});

app.Run();
