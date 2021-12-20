namespace BreadTh.WayOh;

public class Railroad<TError>
{
    public static IRailroadBuilderHead<TValue, TError> Input<TValue>() =>
        new BuilderHead<TValue, TError>();
}
