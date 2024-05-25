using Cwiczenia9.Repositories;
using Cwiczenia9.Services;

namespace Cwiczenia9;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddScoped<ITripsRepository, TripsRepository>();
        builder.Services.AddScoped<ITripsService, TripsService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}