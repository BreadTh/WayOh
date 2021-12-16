namespace BreadTh.WayOh;

public class TwoTrackOutcome<TResult, TError>
{
    public TwoTrackOutcome(TResult result, Func<Task>? compensatingAction = null)
    {
        this.WasSuccessful = true;
        this.Result = result;
        this.CompensatingAction = compensatingAction;
        this.Error = default!;
        this.ErrorMessage = null!;
    }

    public TwoTrackOutcome(TError error, string? errorMessage = null)
    {
        this.WasSuccessful = false;
        this.Result = default!;
        this.CompensatingAction = null!;
        this.Error = error;
        this.ErrorMessage = errorMessage;
    }

    public bool WasSuccessful { get; init; }
    public TResult Result { get; init; }
    public Func<Task>? CompensatingAction { get; init; }
    public TError Error { get; init; }
    public string? ErrorMessage { get; init; }
}
