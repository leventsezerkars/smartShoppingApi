namespace smartShoppingProject.Domain.Enums;

/// <summary>
/// Desteklenen para birimleri. DDD'de sınırlı değer kümesi enum ile modellenir; tip güvenliği ve domain diline uyum sağlar.
/// </summary>
public enum Currency
{
    TRY = 0,
    USD = 1,
    EUR = 2,
    GBP = 3
}
