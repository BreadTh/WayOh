namespace BreadTh.WayOh;

internal interface INode<TInput, TFinalOutput, TError> 
{
    public Task<Juxt<TFinalOutput, TError>> Execute(Juxt<TInput, TError> input);
}
