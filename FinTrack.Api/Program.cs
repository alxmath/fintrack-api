using FinTrack.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddInfraestructure(
    builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

//-----------------
// PIPELINE
//-----------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
