namespace Praedico.Bookings.Domain;

/// <summary>
/// Aggregate Root - Bounded Context & Consistency Boundary
/// </summary>
public abstract class AggregateRoot(Guid id) : Entity(id);