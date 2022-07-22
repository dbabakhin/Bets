using Bets.API.App;
using Bets.API.HealthCheck;
using Bets.API.Service;
using Bets.Domain.Interfaces;
using Bets.Infrastructure.Kafka;
using Bets.Infrastructure.Repositories;
using Shared.Common.Messages;
using Shared.Infrastructure.Kafka;

const string CONSUMER_SECTION = "Kafka:Consumer";
const string PRODUCER_SECTION = "Kafka:Producer";
const string BETS_CONNECTION = "BetsConnection";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks().AddCheck<BetsCheck>("BetCheck");

var betsConnectionString = builder.Configuration.GetValue<string>(BETS_CONNECTION);

var cfgConsumer = new SharedConsumerConfig<BetConfirmResultMessage>();
builder.Configuration.GetSection(CONSUMER_SECTION).Bind(cfgConsumer);
builder.Services.AddSingleton(a =>
{
    return cfgConsumer.Build();
});

var cfgProducer = new SharedProducerConfig<BetConfirmRequestMessage>();
builder.Configuration.GetSection(PRODUCER_SECTION).Bind(cfgProducer);
builder.Services.AddSingleton(a =>
{
    return cfgProducer.Build();
});


builder.Services.AddScoped<IUsersRepository>(a => new UsersRepository(betsConnectionString));
builder.Services.AddScoped<IBetsRepository>(a => new BetsRepository(betsConnectionString));

builder.Services.AddScoped<BetsProcessor>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<CWTResultReceiver>();


var app = builder.Build();

app.MapHealthChecks("/healthz");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
