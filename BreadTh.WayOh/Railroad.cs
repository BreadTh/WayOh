namespace BreadTh.WayOh;

public class Railroad<TError>
{
    public static IRailroadBuilderHead<TValue, TError> Input<TValue>(IServiceProvider serviceProvider) =>
        new BuilderHead<TValue, TError>(serviceProvider);
}
