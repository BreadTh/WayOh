using BreadTh.WayOh;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using ValueOf;

var serviceCollection = new ServiceCollection();
serviceCollection.AddTransient<AttemptCreateInvoice>();
serviceCollection.AddTransient<FaultyGuidGenerator>();
var serviceProvider = serviceCollection.BuildServiceProvider();


var orderFlow = Railroad<Error>
    .Input<string>(serviceProvider)
    .Then(AttemptParseOrder)
    .Then(AttemptValidateOrder)
    .Then(AddOrderId)
    //.Parallel(
    //    track => track
            .Then(AttemptMakeOrder)//,
    //    track => track
            .Then(CalculateTotal)
            .Then<AttemptCreateInvoice, Guid>()
            .Then(AttemptMakeCustomerPAAY)
    //)
    .End();

async Task<string> MakeOrder(string rawOrder) 
{
    var (isError, value, error) = await orderFlow.Execute(rawOrder);

    if (isError)
        return error.ToString();

    return value;
}

var acceptableRawOrder = "{\"Items\": [{\"Quantity\": 1, \"OrderName\": \"Cappuccino\"}]}";
var malformedRawOrder =  "{\"Items\": [{\"Quantity\": 1, \"OrderName\": Cappuccino\"}]}";
var invalidRawOrder =    "{\"Items\": [{\"Quantity\":-1, \"OrderName\": \"Cappuccino\"}]}";

Console.Write("Acceptable order: ");
Console.WriteLine(await MakeOrder(acceptableRawOrder));
Console.Write("Malformed order:  ");
Console.WriteLine(await MakeOrder(malformedRawOrder));
Console.Write("invalid order:    ");
Console.WriteLine(await MakeOrder(invalidRawOrder));

Juxt<Order, Error> AttemptParseOrder(string input) 
{
    try 
    {
        var output = JsonConvert.DeserializeObject<Order>(input);
        
        if(output is null)
            return Error.OrderSyntaxError;

        return output;

    }
    catch (JsonException) 
    {
        return Error.OrderSyntaxError;
    }
}
Juxt<Order, Error> AttemptValidateOrder(Order order) 
{
    if (order.IsValid)
        return order;
    else
        return Error.OrderInvalid;
}
(Order, OrderId) AddOrderId(Order order) { return (order, OrderId.From(Guid.NewGuid())); }
Juxt<bool, Error> AttemptMakeOrder((Order, OrderId) input) { return true; }
Exception CalculateTotal(bool input) { return new Exception(); }

Juxt<string, Error> AttemptMakeCustomerPAAY(Guid input) { return "Hello"; }

public class AttemptCreateInvoice : IRailroadStep<Exception, Guid, Error>
{
    private readonly FaultyGuidGenerator generator;

    public AttemptCreateInvoice(FaultyGuidGenerator generator) 
    {
        this.generator = generator;
    }

    public Task<Juxt<Guid, Error>> Execute(Exception input)
    {
        try
        {       
            return Task.FromResult(new Juxt<Guid, Error>(generator.CreateGuidOrThrow()));
        }
        catch 
        {
            return Task.FromResult(new Juxt<Guid, Error>(Error.GuidGenerationIssue));
        }

    }
}

public class FaultyGuidGenerator 
{
    public Guid CreateGuidOrThrow() 
    {
        if(new Random().NextDouble() < 0.5)
            throw new Exception();
        else
            return Guid.NewGuid();
    }
}

public enum Error { OrderSyntaxError, OrderInvalid, GuidGenerationIssue }
public enum OrderName { Cappuccino, CaffeLatte }
public class Order 
{
    public Item[] Items { get; set; } = null!;
    
    public bool IsValid { get { return Items.All((item) => item.IsValid); } }
    public class Item 
    {
        public int Quantity { get; set; }
        public OrderName OrderName { get; set; }

        public bool IsValid { get { return Quantity > 0; } }

    }

}

class OrderId : ValueOf<Guid, OrderId>
{ }
