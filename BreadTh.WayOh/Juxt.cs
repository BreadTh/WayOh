namespace BreadTh.WayOh;

public class Juxt<TValue, TError>
{
    public bool IsError { get; init; }
    public TValue Value { get; init; }
    public TError Error { get; init; }
    
    public Juxt(TValue value) 
    {
        IsError = false;
        Value = value;
        Error = default!;
    }

    public Juxt(TError error)
    {
        IsError = true;
        Value = default!;
        Error = error;
    }

    public static implicit operator Juxt<TValue, TError>(TValue value) =>
        new(value);

    public static implicit operator Juxt<TValue, TError>(TError error) =>
        new(error);

    public void Deconstruct(out bool isError, out TValue value, out TError error) 
    {
        isError = IsError;
        value = Value;
        error = Error;
    }
}