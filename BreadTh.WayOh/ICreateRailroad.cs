namespace BreadTh.WayOh;

internal interface ICreateRailroad<TOriginalInput, TOutput, TError>
{
    IRailroad<TOriginalInput, TFinalOutput, TError> CreateRailroad<TFinalOutput>(INode<TOutput, TFinalOutput, TError> next);
}
