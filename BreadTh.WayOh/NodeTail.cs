namespace BreadTh.WayOh;

internal class NodeTail<TValue, TError> : INode<TValue, TValue, TError>
{
    public Task<Juxt<TValue, TError>> Execute(Juxt<TValue, TError> input) =>
        Task.FromResult(input);
}
