using OneOf;

namespace BreadTh.WayOh;

public abstract class JunctionBase<TInput, TOutput, TError>
{
    public Failure<TError> Fail(TError error) =>
        new Failure<TError>(error);
}

public record Success<TOutput>(TOutput output);
public record Failure<TError>(TError Error);

public class Direction<TOutput, TError> : OneOfBase<TOutput, TError>
{
    protected Direction(OneOf<TOutput, TError> input) : base(input)
    {
    }


}
