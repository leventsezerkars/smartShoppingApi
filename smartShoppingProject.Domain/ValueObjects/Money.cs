namespace smartShoppingProject.Domain.ValueObjects;

using smartShoppingProject.Domain.Enums;
using smartShoppingProject.Domain.Exceptions;

/// <summary>
/// Money bir Value Object'tir: kimliği yoktur, eşitlik Amount + Currency ile belirlenir; DDD'de para kavramı tek bir tip ile ifade edilir.
/// </summary>
public sealed class Money : IEquatable<Money>
{
    public Money(decimal amount, Currency currency = Currency.TRY)
    {
        if (amount < 0)
        {
            throw new InvalidPriceException("Tutar negatif olamaz.");
        }

        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }
    public Currency Currency { get; }

    public Money Add(Money other)
        => this + other;

    public Money Multiply(int multiplier)
        => this * multiplier;

    public bool Equals(Money? other)
    {
        if (other is null)
        {
            return false;
        }

        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
        => obj is Money other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Amount, Currency);

    public static Money operator +(Money left, Money right)
    {
        if (left is null || right is null)
        {
            throw new DomainException("Money değerleri null olamaz.");
        }

        if (left.Currency != right.Currency)
        {
            throw new DomainException("Farklı para birimleri toplanamaz.");
        }

        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator *(Money money, int multiplier)
    {
        if (money is null)
        {
            throw new DomainException("Money değeri null olamaz.");
        }

        if (multiplier < 0)
        {
            throw new DomainException("Çarpan negatif olamaz.");
        }

        return new Money(money.Amount * multiplier, money.Currency);
    }

    public static Money operator *(int multiplier, Money money)
        => money * multiplier;

    public static bool operator ==(Money? left, Money? right)
        => Equals(left, right);

    public static bool operator !=(Money? left, Money? right)
        => !Equals(left, right);

    public override string ToString()
        => $"{Amount} {Currency}";
}
