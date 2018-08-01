using System;
using System.Linq;

using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;

using Microsoft.Extensions.DependencyInjection;
using Xunit;


namespace MigrationTest.XUnit
{
  public class DatabaseFixture : IDisposable
  {
    public DatabaseFixture()
    {
      var serviceProvider = CreateServices();

      // Put the database update into a scope to ensure
      // that all resources will be disposed.
      using (var scope = serviceProvider.CreateScope())
      {
        CreateDatabase(scope.ServiceProvider);
      }
    }

    public void Dispose()
    {
      var serviceProvider = CreateServices();

      // Put the database update into a scope to ensure
      // that all resources will be disposed.
      using (var scope = serviceProvider.CreateScope())
      {
        DestroyDatabase(scope.ServiceProvider);
      }
    }

    /// <summary>
    /// Configure the dependency injection services
    /// </sumamry>
    private static IServiceProvider CreateServices()
    {
      return new ServiceCollection()
          // Add common FluentMigrator services
          .AddFluentMigratorCore()
          .ConfigureRunner(rb => rb
              // Add SQLite support to FluentMigrator
              .AddSqlServer()
              // Set the connection string
              .WithGlobalConnectionString("Data Source=(LocalDb)\\MSSQLLocalDB;Database=MigrationTestUnitTest")
              // Define the assembly containing the migrations
              .ScanIn(typeof(AddLogTable).Assembly).For.Migrations())
          // Enable logging to console in the FluentMigrator way
          .AddLogging(lb => lb.AddFluentMigratorConsole())
          // Build the service provider
          .BuildServiceProvider(false);
    }

    /// <summary>
    /// Update the database
    /// </sumamry>
    private static void CreateDatabase(IServiceProvider serviceProvider)
    {
      // Instantiate the runner
      var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

      // Execute the migrations
      runner.MigrateUp();
    }

    private static void DestroyDatabase(IServiceProvider serviceProvider)
    {
      // Instantiate the runner
      var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

      // Execute the migrations
      runner.MigrateDown(0);
    }
  }
}
