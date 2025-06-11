using QuanNhauSanVuon.Localization;

namespace QuanNhauSanVuon.Services.Exceptions;

public class DuplicatedException : Exception
{
    public DuplicatedException(string propertyName)
        : base(ErrorMessages.UniqueDuplicated.Replace(
            "{PropertyName}",
            DisplayNames.Get(propertyName)))
    {
        PropertyName = propertyName;
    }

    public string PropertyName { get; set; }
}