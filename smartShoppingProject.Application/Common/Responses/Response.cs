namespace smartShoppingProject.Application.Common.Responses;

/// <summary>
/// Tüm command ve query handler'ların standart dönüş tipi. IResponse uygular; PagedResponse bu sınıftan türer.
/// </summary>
public record Response<T>(bool Success, string? ErrorMessage, T? Data) : IResponse
{
    public static Response<T> Ok(T data) => new(true, null, data);

    public static Response<T> Fail(string errorMessage) => new(false, errorMessage, default);
}
