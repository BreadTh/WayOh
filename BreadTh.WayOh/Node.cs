namespace BreadTh.WayOh;

internal class Node<TInput, TOutput, TFinalOutput, TError> : INode<TInput, TFinalOutput, TError>
{
    readonly Func<TInput, Task<Juxt<TOutput, TError>>> payload;
    readonly INode<TOutput, TFinalOutput, TError> next;

    internal Node(Func<TInput, Task<Juxt<TOutput, TError>>> payload, INode<TOutput, TFinalOutput, TError> next) 
    {
        this.payload = payload;
        this.next = next;
    }

    public async Task<Juxt<TFinalOutput, TError>> Execute(Juxt<TInput, TError> input) 
    {
        if(input.IsError)
            return input.Error;
        else
            return await next.Execute(await payload(input.Value));
    }
}
