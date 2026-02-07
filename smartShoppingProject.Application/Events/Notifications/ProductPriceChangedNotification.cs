namespace smartShoppingProject.Application.Events.Notifications;

using MediatR;

/// <summary>
/// ProductPriceChangedEvent için MediatR notification; Money Application'a taşınmaz, primitive alanlar kullanılır.
/// </summary>
public sealed record ProductPriceChangedNotification(
    Guid ProductId,
    decimal OldPriceAmount,
    string OldPriceCurrency,
    decimal NewPriceAmount,
    string NewPriceCurrency,
    DateTime OccurredOn) : INotification;
