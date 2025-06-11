using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Services.Exceptions;

public struct MySqlExceptionHandledResult
{
    private string _violatedTableName;
    private string _violatedFieldName;
    private string _violatedConstraintName;

    public bool IsUniqueConstraintViolated { get; set; }

    public bool IsNotNullConstraintViolated { get; set; }

    public bool IsMaxLengthExceeded { get; set; }

    public bool IsForeignKeyNotFound { get; set; }

    public bool IsDeleteOrUpdateRestricted { get; set; }

    public string ViolatedTableName
    {
        get => _violatedTableName;
        set => _violatedTableName = value.SnakeCaseToPascalCase();
    }

    public string ViolatedFieldName
    {
        get => _violatedFieldName;
        set => _violatedFieldName = value
            .SnakeCaseToPascalCase()
            .Replace("Username", "UserName");
    }

    public string ViolatedConstraintName
    {
        get => _violatedConstraintName;
        set => _violatedConstraintName = value
            .SnakeCaseToPascalCase()
            .Replace("Username", "UserName");
    }

    public object ViolatedValue { get; set; }
}
