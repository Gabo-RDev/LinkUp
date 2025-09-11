using LinkUp.Infrastructure.Persistence;
using LinkUp.Presentation.API.Extensions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddSerilogExtension();

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddInfrastructurePersistence(builder.Configuration);

    //Service extensions
    builder.Services.AddVersioning();
    builder.Services.AddSwaggerExtension();

    var app = builder.Build();

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
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex}");
    Log.Fatal("server terminated unexpectedly");
}

finally
{
    Log.CloseAndFlush();
}