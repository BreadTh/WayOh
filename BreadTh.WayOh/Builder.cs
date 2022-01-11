using Microsoft.Extensions.DependencyInjection;

namespace BreadTh.WayOh;

internal class Builder<TOriginalInput, TInput, TOutput, TError>
    : IRailroadBuilder<TOriginalInput, TOutput, TError>,
    ICreateRailroad<TOriginalInput, TOutput, TError>
{
    readonly ICreateRailroad<TOriginalInput, TInput, TError> previous;
    readonly Func<TInput, Task<Juxt<TOutput, TError>>> step;
    readonly IServiceProvider serviceProvider;

    internal Builder(Func<TInput, Task<Juxt<TOutput, TError>>> step, ICreateRailroad<TOriginalInput, TInput, TError> previous, IServiceProvider serviceProvider) 
    {
        this.step = step;
        this.previous = previous;
        this.serviceProvider = serviceProvider;
    }
    public IRailroadBuilder<TOriginalInput, TNextOutput, TError> Then<TNextOutput>(Func<TOutput, Task<Juxt<TNextOutput, TError>>> nextStep) =>
        new Builder<TOriginalInput, TOutput, TNextOutput, TError>(nextStep, this, serviceProvider);

    public IRailroadBuilder<TOriginalInput, TNextOutput, TError> Then<TNextOutput>(Func<TOutput, TNextOutput> nextStep) =>
        Then((TOutput output) => Task.FromResult(new Juxt<TNextOutput, TError>(nextStep(output))));

    public IRailroadBuilder<TOriginalInput, TNextOutput, TError> Then<TNextOutput>(Func<TOutput, Task<TNextOutput>> nextStep) =>
        Then(async (TOutput output) => new Juxt<TNextOutput, TError>(await nextStep(output)));

    public IRailroadBuilder<TOriginalInput, TNextOutput, TError> Then<TNextOutput>(Func<TOutput, Juxt<TNextOutput, TError>> nextStep) =>
        Then((TOutput output) => Task.FromResult(nextStep(output)));

    public IRailroadBuilder<TOriginalInput, TNextOutput, TError> Then<TStep, TNextOutput>()
        where TStep : IRailroadStep<TOutput, TNextOutput, TError>
    {
        var testResolution = serviceProvider.GetService<TStep>();

        if (testResolution is null)
        {
            var name = typeof(TStep).Name;
            throw new Exception($"{name} could not be found in the given IServiceProvider. Did you forget to register {name} in the ServiceCollection?");
        }

        async Task<Juxt<TNextOutput, TError>> FunctionalWithDelayedResolution(TOutput value) =>
            await serviceProvider.GetRequiredService<TStep>().Execute(value);

        return Then(FunctionalWithDelayedResolution);
    }

    public IRailroad<TOriginalInput, TOutput, TError> End() =>
        CreateRailroad(new NodeTail<TOutput, TError>());
    
    public IRailroad<TOriginalInput, TFinalOutput, TError> CreateRailroad<TFinalOutput>(INode<TOutput, TFinalOutput, TError> next) =>
        previous.CreateRailroad(new Node<TInput, TOutput, TFinalOutput, TError>(step, next));
}
