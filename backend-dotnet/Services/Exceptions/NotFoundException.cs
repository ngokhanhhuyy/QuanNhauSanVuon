using QuanNhauSanVuon.Localization;

namespace QuanNhauSanVuon.Services.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(
        string resourceName,
        object[] propertyPathElements,
        string attemptedValue)
        : base(ErrorMessages.NotFoundByProperty
            .Replace("{ResourceName}", DisplayNames.Get(resourceName))
            .Replace("{AttemptedValue}", attemptedValue))
    {
        ResourceName = resourceName;
        PropertyPathElements = propertyPathElements;
        AttemptedValue = attemptedValue;
    }

    public string ResourceName { get; set; }
    public object[] PropertyPathElements { get; set; }
    public object AttemptedValue { get; set; }
}