namespace smartShoppingProject.Application.Common.Responses;

/// <summary>
/// Sayfalı liste sonuçları için standart tip. Response&lt;IReadOnlyList&lt;T&gt;&gt; sınıfından türer;
/// ek olarak PageNumber, PageSize, TotalCount, TotalPages ve Items (Data ile aynı referans) taşır.
/// </summary>
public sealed record PagedResponse<T>(
    bool Success,
    string? ErrorMessage,
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages) : Response<IReadOnlyList<T>>(Success, ErrorMessage, Items)
{
    public static PagedResponse<T> Ok(
        IReadOnlyList<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        var totalPages = pageSize > 0 ? (int)Math.Ceiling(totalCount / (double)pageSize) : 0;
        return new PagedResponse<T>(true, null, items, pageNumber, pageSize, totalCount, totalPages);
    }

    public static PagedResponse<T> Fail(string errorMessage) => new(
        false,
        errorMessage,
        Array.Empty<T>(),
        0,
        0,
        0,
        0);
}
