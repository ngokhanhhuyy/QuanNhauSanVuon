using QuanNhauSanVuon.Extensions;
using QuanNhauSanVuon.Localization;

namespace QuanNhauSanVuon.Services.Exceptions;

public class OperationException : Exception
{
    public OperationException(string message) : base(message) { }

    public OperationException(object[] propertyPathElements, string message) : base(message)
    {
        PropertyPathElements = propertyPathElements;
    }
    
    public OperationException(object[] propertyPathElements, Exception exception)
        : base(exception.Message)
    {
        PropertyPathElements = propertyPathElements;
    }

    public object[] PropertyPathElements { get; init; }

    public static OperationException Duplicated(params object[] propertyPathElements)
    {
        string lastStringElement = ValidateAndGetLastStringElement(propertyPathElements);
        string displayName = DisplayNames.Get(lastStringElement);
        string errorMessage = ErrorMessages.UniqueDuplicated.ReplacePropertyName(displayName);
        return new OperationException(propertyPathElements, errorMessage);
    }

    public static OperationException NotFound(
        object[] propertyPathElements,
        string resourceDisplayName)
    {
        string lastStringElement = ValidateAndGetLastStringElement(propertyPathElements);
        string errorMessage = ErrorMessages.NotFound.ReplacePropertyName(
            resourceDisplayName ?? DisplayNames.Get(lastStringElement));

        return new OperationException(propertyPathElements, errorMessage);
    }

    public static OperationException DeleteRestricted(
            object[] propertyPathElements,
            string resourceDisplayName)
    {
        string lastStringElement = ValidateAndGetLastStringElement(propertyPathElements);
        string errorMessage = ErrorMessages.DeleteRestricted
            .ReplaceResourceName(resourceDisplayName ?? DisplayNames.Get(lastStringElement));
        return new OperationException(propertyPathElements, errorMessage);

    }

    private static string ValidateAndGetLastStringElement(object[] propertyPathElements)
    {
        if (propertyPathElements.Length == 0)
        {
            throw new ArgumentException(
                "The array containing the property path elements cannot be left empty.");
        }

        string lastStringElement = propertyPathElements
            .LastOrDefault(element => element is string)
            ?.ToString()
            ?? throw new ArgumentException("There is no string element in the given array " +
                "containing the property path elements.");

        return lastStringElement;
    }

    private readonly struct JoinedPropertyPathResult
    {
        public string PropertyPath { get; init; }
        public string LastStringElement { get; init; }
    } 
}