using OneOf;

namespace BreadTh.WayOh;

internal readonly struct SadPath<TInput, TError> : ITwoTrack<TInput, TError>
{
    private readonly TError error;
    private readonly string? message;
    public SadPath(TError error, string? message = null)
    {
        this.error = error;
        this.message = message;
    }
    
    public Task<ITwoTrack<TOutput, TError>> Then<TOutput>(Func<TInput, Task<TwoTrackOutcome<TOutput, TError>>> executeMe) =>    
        Task.FromResult<ITwoTrack<TOutput, TError>>(new SadPath<TOutput, TError>(error, message));
    
    public async Task End(Func<OneOf<TInput, TError>, Task> executeMe) =>
        await executeMe(error);
}

