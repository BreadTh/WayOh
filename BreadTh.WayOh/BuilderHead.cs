using Microsoft.Extensions.DependencyInjection;

namespace BreadTh.WayOh;

internal class BuilderHead<TValue, TError>
    : IRailroadBuilderHead<TValue, TError>,
    ICreateRailroad<TValue, TValue, TError>
{
    private readonly IServiceProvider serviceProvider;

    internal BuilderHead(IServiceProvider serviceProvider) 
    {
        this.serviceProvider = serviceProvider;
    }

    public IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, Task<Juxt<TNextOutput, TError>>> nextStep) =>
        new Builder<TValue, TValue, TNextOutput, TError>(nextStep, this, serviceProvider);

    public IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, TNextOutput> nextStep) =>
        Then((TValue output) => Task.FromResult(new Juxt<TNextOutput, TError>(nextStep(output))));

    public IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, Task<TNextOutput>> nextStep) =>
        Then(async (TValue output) => new Juxt<TNextOutput, TError>(await nextStep(output)));

    public IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, Juxt<TNextOutput, TError>> nextStep) =>
        Then((TValue output) => Task.FromResult(nextStep(output)));

    public IRailroadBuilder<TValue, TNextOutput, TError> Then<TStep, TNextOutput>()
        where TStep : IRailroadStep<TValue, TNextOutput, TError>
    {
        var testResolution = serviceProvider.GetService<TStep>();

        if (testResolution is null)
        {
            var name = typeof(TStep).Name;
            throw new Exception($"{name} could not be found in the given IServiceProvider. Did you forget to register {name} in the ServiceCollection?");
        }

        async Task<Juxt<TNextOutput, TError>> FunctionalWithDelayedResolution(TValue value) =>
            await serviceProvider.GetRequiredService<TStep>().Execute(value);

        return Then(FunctionalWithDelayedResolution);
    }

    public IRailroad<TValue, TFinalOutput, TError> CreateRailroad<TFinalOutput>(INode<TValue, TFinalOutput, TError> next) =>
        new ActualRailroad<TValue, TFinalOutput, TError>(next);
}