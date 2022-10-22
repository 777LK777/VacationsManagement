using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql;
using VacationService.CustomExceptions;

namespace VacationService.DB.Migrations;

public class MigrationEngine
{
    private const string ExecutedScriptsTableName = "_EXECUTED_SCRIPTS";
    private const string ScriptsFolderName = "VacationService.DB.Migrations.Scripts";

    private readonly IConfiguration _configuration;

    public MigrationEngine(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Migrate()
    {
        var connectionStringToPostgres = _configuration.GetConnectionString("master") ?? string.Empty;
        
        var allScripts = GetAllScripts();
        if (allScripts?.Count == 0)
            return;

        CreateExecutedScriptsTable(connectionStringToPostgres);
        CreateOrganizationDatabase(connectionStringToPostgres);
        CreateVacationsManagementDatabase(connectionStringToPostgres);

        var executedScripts = GetAlreadyExecutedScriptsFromDatabase(connectionStringToPostgres).Result;

        var scriptsToExecute = allScripts
            .Where(s => executedScripts.All(es => es.Name != s.Name))
            .ToList();

        CommitScripts(connectionStringToPostgres, scriptsToExecute);
    }

    private async Task CommitScripts(string connectionStringToPostgres, List<ScriptFile> scriptsToExecute)
    {
        NpgsqlTransaction transaction = null;
        var scriptsToSave = new List<ScriptFile>();

        try
        {
            using var conn = new NpgsqlConnection(connectionStringToPostgres);
            conn.Open();

            transaction = conn.BeginTransaction();

            foreach (var script in scriptsToExecute)
            {
                var queries = SplitBySemicolon(script.Content);
                foreach (var query in queries)
                {
                    using var cmd = new NpgsqlCommand(query, conn, transaction);
                    await cmd.ExecuteNonQueryAsync();
                }
                
                scriptsToSave.Add(script);
            }

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            throw new InternalServerException("Can't execute migration scripts.", e);
        }
        finally
        {
            await transaction.DisposeAsync();
            RecordExecutedScripts(connectionStringToPostgres, scriptsToSave);
        }
    }

    private async Task RecordExecutedScripts(string connectionStringToPostgres, List<ScriptFile> scriptsToSave)
    {
        var insertScript = 
            $"INSERT INTO public.{ExecutedScriptsTableName} (FILE_NAME, EXECUTION_TIME, CODE)" +
             "VALUES (@fileName, @executionTime, @content)";

        NpgsqlTransaction transaction = null;

        try
        {
            using var conn = new NpgsqlConnection(connectionStringToPostgres);
            transaction = conn.BeginTransaction();

            foreach (var script in scriptsToSave)
            {
                using var cmd = new NpgsqlCommand(insertScript, conn, transaction);
                cmd.Parameters.AddWithValue("@fileName", script.Name);
                cmd.Parameters.AddWithValue("@executionTime", DateTime.Now);
                cmd.Parameters.AddWithValue("@content", script.Content);

                cmd.ExecuteNonQuery();
            }

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            throw new InternalServerException("Can't execute migration scripts.", e);
        }
        finally
        {
            await transaction.DisposeAsync();
        }

    }

    private List<string> SplitBySemicolon(string scriptContent)
    {
        var statements = Regex.Split(
            scriptContent,
            @"^\s*;\s*\d*\s*($|\-\-.*$)",
            RegexOptions.Multiline |
            RegexOptions.IgnorePatternWhitespace |
            RegexOptions.IgnoreCase);

        return statements
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim(' ', '\r', '\n'))
            .ToList();
    }

    private async Task<List<ScriptFile>> GetAlreadyExecutedScriptsFromDatabase(string connectionStringToPostgres)
    {
        var result = new List<ScriptFile>();

        var script = $"SELECT FILE_NAME FROM public.{ExecutedScriptsTableName}";

        try
        {
            using var conn = new NpgsqlConnection(connectionStringToPostgres);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand(script, conn);
            using var queryResultReader = await cmd.ExecuteReaderAsync();

            while (queryResultReader.Read())
            {
                result.Add(new ScriptFile
                {
                    Name = queryResultReader["FILE_NAME"].ToString()
                });
            }
        }
        catch (Exception e)
        {
            throw new InternalServerException("Can't execute migration scripts.", e);
        }
        
        return result;
    }

    private void CreateVacationsManagementDatabase(string connectionStringToPostgres)
    {
        throw new NotImplementedException();
    }

    private void CreateOrganizationDatabase(string connectionStringToPostgres)
    {
        throw new NotImplementedException();
    }

    private void CreateExecutedScriptsTable(string connectionString)
    {
        var createScript =
            $"CREATE TABLE IF NOT EXISTS public.{ExecutedScriptsTableName}" +
             "(ID SERIAL PRIMARY KEY, " +
             "FILE_NAME VARCHAR NOT NULL, " +
             "EXECUTION_TIME TIMESTAMP NOT NULL, " +
             "CONTENT VARCHAR NOT NULL);";

        try
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var command = new NpgsqlCommand(createScript);
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw new InternalServerException("Can't execute migration scripts.", e);
        }
    }

    private List<ScriptFile> GetAllScripts()
    {
        return Assembly
            .GetExecutingAssembly()
            .GetManifestResourceNames()
            .Where(s => s.EndsWith(".sql"))
            .OrderBy(r => r)
            .Select(r => new ScriptFile
            {
                Name = r.Replace(ScriptsFolderName + ".", string.Empty),
                Content = GetScriptContent(r)
            })
            .ToList();
    }

    private string GetScriptContent(string scriptFileName)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(scriptFileName);
        if (stream == null)
            throw new InternalServerException();

        using var streamReader = new StreamReader(stream);

        return streamReader.ReadToEnd();
    }
}