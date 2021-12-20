namespace BreadTh.WayOh;

public interface IRailroadBuilderHead<TValue, TError>
{
    IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, TNextOutput> nextStep);
    IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, Juxt<TNextOutput, TError>> nextStep);
    IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, Task<TNextOutput>> nextStep);
    IRailroadBuilder<TValue, TNextOutput, TError> Then<TNextOutput>(Func<TValue, Task<Juxt<TNextOutput, TError>>> nextStep);
}
