namespace BreadTh.WayOh;

public interface IRailroadStep<TInput, TOutput, TError>
{
    Task<Juxt<TOutput, TError>> Execute(TInput input);
}
