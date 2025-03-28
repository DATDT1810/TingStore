﻿using Polly;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using User.Infrastructure.Data;

namespace User.Api.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation($"Started Db Migration: {typeof(TContext).Name}");
                    // retry strategy
                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, span, cont) =>
                        {
                            logger.LogError("Retrying because of {exception} {retry}", exception, span);
                        });
                    retry.Execute(() => CallSeeder(seeder, context, services));
                    logger.LogInformation($"Migration Completed: {typeof(TContext).Name}");
                }
                catch(SqlException ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating db: {typeof(TContext).Name}");
                }
            }
            return host;
        }

        private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
