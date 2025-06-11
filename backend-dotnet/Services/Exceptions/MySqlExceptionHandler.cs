using System.Text.RegularExpressions;
using MySqlConnector;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Services.Exceptions;

/// <summary>
/// A handler to handle the exceptions thrown by the MySql database.
/// </summary>
public partial class MySqlExceptionHandler
{
    public MySqlExceptionHandledResult Handle(MySqlException exception)
    {
        Match match;
        MySqlExceptionHandledResult result = new MySqlExceptionHandledResult();
        
        switch (exception.Number)
        {
            case 1062:
                result.IsUniqueConstraintViolated = true;
                match = UniqueConstraintRegex().Match(exception.Message);
                if (match.Success)
                {
                    result.ViolatedTableName = match.Groups["tableName"].Value;
                    result.ViolatedConstraintName = match.Groups["constraintName"].Value;
                    result.ViolatedFieldName = result
                        .ViolatedConstraintName
                        .Split("__")
                        .Last();
                    result.ViolatedValue = match.Groups["duplicatedKeyValue"].Value;
                }

                break;

            case 1364:
                result.IsNotNullConstraintViolated = true;
                match = NotNullConstraintRegex().Match(exception.Message);
                if (match.Success)
                {
                    result.ViolatedFieldName = match.Groups["columnName"].Value;
                }

                break;

            case 1406:
                result.IsMaxLengthExceeded = true;
                match = MaxLengthConstraintRegex().Match(exception.Message);
                if (match.Success)
                {
                    result.ViolatedFieldName = match.Groups["columnName"].Value;
                }

                break;

            case 1451:
                result.IsDeleteOrUpdateRestricted = true;
                match = DeleteOrUpdateRestrictedRegex().Match(exception.Message);
                if (match.Success)
                {
                    result.ViolatedConstraintName = match.Groups["constraintName"].Value;
                    result.ViolatedTableName = match.Groups["tableName"].Value;
                    result.ViolatedFieldName = match.Groups["columnName"].Value;
                }

                break;

            case 1452:
                result.IsForeignKeyNotFound = true;
                match = ForeignKeyNotFoundRegex().Match(exception.Message);
                if (match.Success)
                {
                    result.ViolatedConstraintName = match.Groups["constraintName"].Value;
                    result.ViolatedTableName = match.Groups["tableName"].Value;
                    result.ViolatedFieldName = match.Groups["columnName"].Value;
                }

                break;
        }

        return result;
    }

    [GeneratedRegex(@"Duplicate entry\s+\'(?<duplicatedKeyValue>.+)\'\s+for key\s+\'(?<tableName>\w+)\.(?<constraintName>\w+)'")]
    private static partial Regex UniqueConstraintRegex();

    [GeneratedRegex(@"Field\s+\'(?<columnName>.+)\'\s+doesn't have a default value")]
    private static partial Regex NotNullConstraintRegex();

    [GeneratedRegex(@"Data truncation: Data too long for column '(?<columnName>.+)' at row (?<rowNumber>.+)")]
    private static partial Regex MaxLengthConstraintRegex();

    [GeneratedRegex(@"(?<databaseName>.+?)\.`(?<tableName>.+)`, CONSTRAINT `(?<constraintName>.+)` FOREIGN KEY \(`(?<columnName>.+)`\) REFERENCES `(?<referencedTableName>.+)` \(`(?<referencedColumnName>.+)`\)")]
    private static partial Regex ForeignKeyNotFoundRegex();

    [GeneratedRegex(@"Cannot delete or update a parent row: a foreign key constraint fails \(`(?<databaseName>.+)`\.`(?<tableName>.+)`, CONSTRAINT `(?<constraintName>.+)` FOREIGN KEY \(`(?<columnName>.+)`\) REFERENCES `(?<referenceTableName>.+)`\)")]
    private static partial Regex DeleteOrUpdateRestrictedRegex();
}