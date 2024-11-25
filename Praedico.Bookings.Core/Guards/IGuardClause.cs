namespace Praedico.Guards;

public interface IGuardClause{}

/// <summary>
/// Guarnds - based on Steve Ardalis guards
/// </summary>
public sealed class Guard : IGuardClause
{
    public static IGuardClause Against { get; } = new Guard();

    private Guard() { }
}
