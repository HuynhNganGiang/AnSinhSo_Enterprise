using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using AnSinhSo.Domain.Aggregates.UserAggregate;

var optionsBuilder = new DbContextOptionsBuilder<AnSinhSoDbContext>();
optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=True;");
using var context = new AnSinhSoDbContext(optionsBuilder.Options);

var model = context.Model;
Console.WriteLine("--- Entities in IModel ---");
foreach (var entityType in model.GetEntityTypes())
{
    Console.WriteLine(entityType.Name);
}
Console.WriteLine("--------------------------");

var isUserInModel = model.GetEntityTypes().Any(e => e.ClrType == typeof(User));
Console.WriteLine($"Is User in IModel? {isUserInModel}");
