namespace BreadTh.WayOh;

public interface IRailroad<TInput, TOutput, TError>
{
    public Task<Juxt<TOutput, TError>> Execute(TInput input);
}
