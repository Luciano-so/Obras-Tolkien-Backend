using Npgsql;
using System.Diagnostics.CodeAnalysis;

namespace TerraMedia.Api.Configuration;

[ExcludeFromCodeCoverage]
public static class PostgresDatabaseCreator
{
    public static void CreateDatabasePostgresIfNotExists(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"));

        var databaseName = builder.Database;

        builder.Database = "postgres";

        using var connection = new NpgsqlConnection(builder.ConnectionString);
        connection.Open();

        using var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'";
        var exists = checkCmd.ExecuteScalar() != null;

        if (!exists)
        {
            using var createCmd = connection.CreateCommand();
            createCmd.CommandText = $"CREATE DATABASE \"{databaseName}\"";
            createCmd.ExecuteNonQuery();
        }
    }
}
