namespace smartShoppingProject.Application.Events.Notifications;

using MediatR;

/// <summary>
/// OrderCancelledEvent için MediatR notification.
/// </summary>
public sealed record OrderCancelledNotification(Guid OrderId, DateTime OccurredOn) : INotification;
