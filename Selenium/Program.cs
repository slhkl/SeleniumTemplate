using Selenium.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/search", (string key) =>
{
    Search.SeacrhOnHepsiBurada(key);
});

app.MapGet("/SteamGameList", () => {
    return Search.GetAllSteamGame();
});

app.MapGet("/SteamGameListAsync", () => {
    return Search.GetAllSteamGameAsync();
});


app.Run();
