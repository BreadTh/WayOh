namespace BreadTh.WayOh;

internal class ActualRailroad<TInput, TOutput, TError> : IRailroad<TInput, TOutput, TError>
{
    readonly INode<TInput, TOutput, TError> head;
 
    internal ActualRailroad(INode<TInput, TOutput, TError> head)
    {
            this.head = head;
    }

    public Task<Juxt<TOutput, TError>> Execute(TInput input) 
    {
        return head.Execute(input);
    }
}
