namespace BreadTh.WayOh;

public static class TwoTrack<TError>
{
    public static ITwoTrack<TInput, TError> Start<TInput>(TInput seed) =>
        new HappyPath<TInput, TError>(seed);
}
