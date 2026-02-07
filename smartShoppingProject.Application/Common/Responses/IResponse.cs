namespace smartShoppingProject.Application.Common.Responses;

/// <summary>
/// Tüm response modellerinin ortak sözleşmesi. Yeni response tipleri bu arayüzü uygulayarak tutarlı success/error semantiğini korur.
/// </summary>
public interface IResponse
{
    bool Success { get; }
    string? ErrorMessage { get; }
}
