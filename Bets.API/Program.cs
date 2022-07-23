using Bets.API.App;
using Bets.API.HealthCheck;
using Bets.API.Service;
using Bets.Infrastructure.DI;
using Shared.Common.Messages;
using Shared.Infrastructure.DI;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafkaConsumer<BetConfirmResultMessage>(builder.Configuration);
builder.Services.AddKafkaProducer<BetConfirmRequestMessage>(builder.Configuration);

builder.Services.AddBetsDataAccess(builder.Configuration);

builder.Services.AddScoped<BetsProcessor>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<CWTResultReceiver>();

builder.Services.AddHealthChecks().AddCheck<BetsCheck>("BetCheck");

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
