
using Microsoft.EntityFrameworkCore;
using NETAPILabb2.DataContext;
using NETAPILabb2.Models;

namespace NETAPILabb2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DBContext>();
            builder.Services.AddCors();

            var app = builder.Build();

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyHeader()
            );

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            //CREATE
            app.MapPost("/player", async (DBContext context, Players player) =>
            {
                context.players.Add(player);
                await context.SaveChangesAsync();
                return Results.Ok(await context.players.ToListAsync());
            });

            //READ
            app.MapGet("/players", async (DBContext context) =>
            {
                return Results.Ok(await context.players.ToListAsync());
            }); 

            //UPDATE
            app.MapPut("/updateplayer/{id}", async (DBContext context, int id, Players players) => 
            {
                var playerToUpdate = await context.players.FindAsync(id);
                if (playerToUpdate == null) 
                {
                    return Results.NotFound("Players does not exist");
                }
                playerToUpdate.FirstName = players.FirstName;
                playerToUpdate.LastName = players.LastName;
                playerToUpdate.Position = players.Position;
                playerToUpdate.Height = players.Height;
                playerToUpdate.Weight = players.Weight;

                await context.SaveChangesAsync();

                return Results.Ok(await context.players.ToListAsync());
            });

            //DELETE
            app.MapDelete("/deleteplayer/{id}", async (DBContext context, int id) =>
            {
                var playerToDelete = await context.players.FindAsync(id);
                if (playerToDelete == null)
                {
                    return Results.NotFound("This player does not exist");
                }

                context.players.Remove(playerToDelete);
                await context.SaveChangesAsync();

                return Results.Ok(await context.players.ToListAsync());
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}