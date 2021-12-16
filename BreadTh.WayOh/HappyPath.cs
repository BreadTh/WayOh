using OneOf;

namespace BreadTh.WayOh;

internal readonly struct HappyPath<TInput, TError> : ITwoTrack<TInput, TError>
{
    private readonly TInput input;
    private readonly Stack<Func<Task>> compensations;

    public HappyPath(TInput input)
    {
        this.input = input;
        compensations = new Stack<Func<Task>>();
    }

    private HappyPath(TInput input, Stack<Func<Task>> compensations)
    {
        this.input = input;
        this.compensations = compensations;
    }

    public async Task<ITwoTrack<TOutput, TError>> Then<TOutput>(Func<TInput, Task<TwoTrackOutcome<TOutput, TError>>> executeMe) 
    {
        var response = await executeMe(input);
        
        if(response.WasSuccessful) 
        {
            if(response.CompensatingAction is not null)
                compensations.Push(response.CompensatingAction);

            return new HappyPath<TOutput, TError>(response.Result, compensations);
        }
        else
        {
            if(response.CompensatingAction is not null)
                await response.CompensatingAction();

            while(compensations.TryPop(out var compensation))
                await compensation();

            return new SadPath<TOutput, TError>(response.Error, response.ErrorMessage);
        }
    }

    public async Task End(Func<OneOf<TInput, TError>, Task> executeMe) =>
        await executeMe(input);
}