using proglog.net.server.Models;
using proglog.net.server.RecordLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRecordLog, RecordLog>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/log/{offset}", (int offset, IRecordLog recordLog) =>
{
    return new ConsumeResponse( recordLog.Read(offset));
})
.WithName("GetLog")
.WithOpenApi();

app.MapPost("/log/", (ProduceRequest request, IRecordLog recordLog) =>
{
    var offset = recordLog.Append(request.Value);
    return new ProduceResponse(offset);
})
.WithName("AppendToLog")
.WithOpenApi();

app.Run();