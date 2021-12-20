namespace BreadTh.WayOh;

public interface IRailroadBuilder<TOriginalInput, TOutput, TError> 
{
    IRailroadBuilder<TOriginalInput, TNextOutput, TError> Then<TNextOutput>(Func<TOutput, TNextOutput> nextStep);
    IRailroadBuilder<TOriginalInput, TNextOutput, TError> Then<TNextOutput>(Func<TOutput, Juxt<TNextOutput, TError>> nextStep);
    IRailroadBuilder<TOriginalInput, TNextOutput, TError> Then<TNextOutput>(Func<TOutput, Task<TNextOutput>> nextStep);
    IRailroadBuilder<TOriginalInput, TNextOutput, TError> Then<TNextOutput>(Func<TOutput, Task<Juxt<TNextOutput, TError>>> nextStep);
    IRailroad<TOriginalInput, TOutput, TError> End();
}
