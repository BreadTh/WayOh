using BreadTh.WayOh;
using Newtonsoft.Json;
using ValueOf;

var orderFlow = Railroad<Error>
    .Input<string>()
    .Then(AttemptParseOrder)
    .Then(AttemptValidateOrder)
    .Then(AddOrderId)
    //.Parallel(
    //    track => track
            .Then(AttemptMakeOrder)//,
    //    track => track
            .Then(CalculateTotal)
            .Then(AttemptCreateInvoice)
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
Juxt<Guid, Error> AttemptCreateInvoice(Exception input) { return Guid.NewGuid(); }
Juxt<string, Error> AttemptMakeCustomerPAAY(Guid input) { return "Hello"; }

public enum Error { OrderSyntaxError, OrderInvalid }
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
