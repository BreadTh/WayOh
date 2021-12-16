using BreadTh.WayOh;
using OneOf;

List<string> ledger = new();
var rand = new Random();

UnpeeledBanana inputBanana;
if(rand.NextDouble() < 0.1)
    inputBanana = null!;
else
    inputBanana = new UnpeeledBanana();

await TwoTrack<Error>
    .Start(new UnpeeledBanana())
    .Then(AttemptPeel)
    //.Then(AttemptReserveBlender())
    //.Then(AttemptSlice)
    //.Then(AttemptBlend)
    .End(GetBackToCustomer);

async Task<TwoTrackOutcome<BareBanana, Error>> AttemptPeel(UnpeeledBanana unpeeledBanana)
{
    if (unpeeledBanana is null)
        return new(new Error(ErrorType.OutOfFruit));

    var (bareBanana, peel) = unpeeledBanana.Peel();

    if (rand.NextDouble() < 0.1)
        return new(new Error(ErrorType.Rotten));

    return new(bareBanana);
};
async Task GetBackToCustomer(OneOf<BareBanana, Error> result) =>
    result.Switch(
        (BareBanana banana) => HandleMilkshakeOutcome(banana),
        (Error error) => ApologizeProfusely(TranslateErrorToExcuse(error))
    );

void HandleMilkshakeOutcome(BareBanana banana) =>
    Console.WriteLine("Here's your banana milkshake!");

void ApologizeProfusely(string excuse) =>
    Console.WriteLine($"We're so sorry esteemed Sir/Ma'am. We take full responsibility of course, but {excuse}!");

string TranslateErrorToExcuse(Error error) =>
    error.Type switch
    {
        ErrorType.OutOfFruit => "we ran out of fruit",
        ErrorType.Rotten => "our fresh frut wasn't so fresh after all..",
        ErrorType.MonkeyFuture => "the monkeys are coming!!",
        _ => "we don't know what happened",
    };



public enum ErrorType { OutOfFruit, Rotten, MonkeyFuture }
public record Error(ErrorType Type, string? Message = null);

public class UnpeeledBanana
{
    public (BareBanana banana, BananaPeel peel) Peel() => 
        (new BareBanana(), new BananaPeel());
}
public class BareBanana 
{

}
public class BananaPeel { }