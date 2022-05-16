using Pokemon.Application;
using Pokemon.Application.Contracts;
using Pokemon.Application.Helpers;
using Pokemon.Application.Interfaces;
using Pokemon.Application.Queries;
using Pokemon.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddHttpClient("PokeApi", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("PokeApiUrl"));
});

builder.Services.AddHttpClient("ShakespeareTranslatorApi", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ShakespeareTranslatorApiUrl"));
});

// TODO: Consider injection type e.g., Singleton
builder.Services.AddScoped<ICharacterDescriptionQuery, CharacterDescriptionQuery>();
builder.Services.AddScoped<IPokeApiClientService, PokeApiClientService>();
builder.Services.AddScoped<IShakespeareTranslatorApiClientService, ShakespeareTranslatorApiClientService>();

var app = builder.Build();

//if (app.Environment.IsDevelopment())

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthcheck");

app.Run();