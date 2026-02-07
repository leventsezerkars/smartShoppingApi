namespace smartShoppingProject.Application.Events.Notifications;

using MediatR;

/// <summary>
/// OrderCreatedEvent için MediatR notification; handler'lar bu tipi dinler.
/// </summary>
public sealed record OrderCreatedNotification(Guid OrderId, DateTime OccurredOn) : INotification;
