using Praedico.Domain;
using Praedico.Guards;

namespace Praedico.Bookings.Domain;

public abstract class Entity : IEntity, IEquatable<Entity>
{
    public Guid Id { get; }
    
    private readonly List<object> _domainEvents = new();

    protected Entity(Guid id)
    {
        Guard.Against.Null(id, nameof(id));
        
        Id = id;
    }
    
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void RaiseDomainEvent(object domainEvent)
    {
        Guard.Against.Null(domainEvent, nameof(domainEvent));
        
        _domainEvents.Add(domainEvent);
    }
    
    public void ClearDomainEvents() => _domainEvents.Clear();
    

    public override bool Equals(object? obj) => 
        obj is Entity other && Equals(other);

    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;

        if (GetType() != other.GetType())
            return false;

        return EqualityComparer<Guid>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => EqualityComparer<Guid>.Default.GetHashCode(Id!);

    public static bool operator ==(Entity? left, Entity? right) =>
        Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right) =>
        !Equals(left, right);
}
