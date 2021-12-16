using OneOf;

namespace BreadTh.WayOh;

public interface ITwoTrack<TInput, TError>
{
    Task<ITwoTrack<TOutput, TError>> Then<TOutput>(Func<TInput, Task<TwoTrackOutcome<TOutput, TError>>> executeMe);
    Task End(Func<OneOf<TInput, TError>, Task> executeMe);
}
