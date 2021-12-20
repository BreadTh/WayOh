namespace BreadTh.WayOh;

internal class BuilderHead<TValue, TError>
    : IRailroadBuilderHead<TValue, TError>,
    ICreateRailroad<TValue, TValue, TError>
{
    public IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, Task<Juxt<TNextOutput, TError>>> nextStep) =>
        new Builder<TValue, TValue, TNextOutput, TError>(nextStep, this);

    public IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, TNextOutput> nextStep) =>
        Then((TValue output) => Task.FromResult(new Juxt<TNextOutput, TError>(nextStep(output))));

    public IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, Task<TNextOutput>> nextStep) =>
        Then(async (TValue output) => new Juxt<TNextOutput, TError>(await nextStep(output)));

    public IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, Juxt<TNextOutput, TError>> nextStep) =>
        Then((TValue output) => Task.FromResult(nextStep(output)));

    public IRailroad<TValue, TFinalOutput, TError> CreateRailroad<TFinalOutput>(INode<TValue, TFinalOutput, TError> next) =>
        new ActualRailroad<TValue, TFinalOutput, TError>(next);
}
