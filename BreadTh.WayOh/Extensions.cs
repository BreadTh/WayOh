using OneOf;

namespace BreadTh.WayOh;

public static class Extensions 
{
    public static async Task<ITwoTrack<TOutput, TError>> Then<TInput, TOutput, TError>(
        this Task<ITwoTrack<TInput, TError>> task, Func<TInput, Task<TwoTrackOutcome<TOutput, TError>>> executeMe)
    {
        var actual = await task;
        return await actual.Then(executeMe);
    }

    public static async Task End<TInput, TError>(this Task<ITwoTrack<TInput, TError>> task, Func<OneOf<TInput, TError>, Task> executeMe)
    {
        var actual = await task;
        await actual.End(executeMe);
    }
}

