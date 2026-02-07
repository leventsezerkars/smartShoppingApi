namespace smartShoppingProject.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using smartShoppingProject.Application.Abstractions.Repositories;
using smartShoppingProject.Domain.Entities;
using Persistence;

internal sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public override async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task<OrderReadModel?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null) return null;

        var items = order.Items
            .Select(i => new OrderItemReadModel(i.Id, i.ProductId, i.UnitPrice.Amount, i.Quantity, i.TotalPrice.Amount))
            .ToList();

        return new OrderReadModel(
            order.Id,
            order.CustomerId,
            order.Status.ToString(),
            order.TotalAmount.Amount,
            order.TotalAmount.Currency.ToString(),
            order.CreatedAt,
            items);
    }

    public async Task<(IReadOnlyList<OrderReadModel> Items, int TotalCount)> GetOrdersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Orders.AsNoTracking().OrderByDescending(o => o.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OrderReadModel(
                o.Id,
                o.CustomerId,
                o.Status.ToString(),
                o.TotalAmount.Amount,
                o.TotalAmount.Currency.ToString(),
                o.CreatedAt,
                o.Items.Select(i => new OrderItemReadModel(i.Id, i.ProductId, i.UnitPrice.Amount, i.Quantity, i.TotalPrice.Amount)).ToList()))
            .ToListAsync(cancellationToken);

        return (orders, totalCount);
    }
}
