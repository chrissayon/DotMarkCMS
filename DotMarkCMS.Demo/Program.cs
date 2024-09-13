using DotMarkCMS.Registration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDotMarkCMS();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/Blog1", () =>
{
    var rootFolder = builder.Configuration.GetValue<string>("DotMarkCMS:RootDirectory");
    var dasd = File.ReadAllTextAsync($"{AppDomain.CurrentDomain.BaseDirectory}/{rootFolder}/Blog1.md");
    return dasd;
})
.WithName("Blog1")
.WithOpenApi();

app.Run();
