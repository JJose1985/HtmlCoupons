var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var products = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase)
{
    ["SN-10001"] = new("SN-10001", "Wireless Mouse", "Ergonomic wireless mouse with Bluetooth connectivity.", "Contoso", "In stock", 29.99m),
    ["SN-10002"] = new("SN-10002", "Mechanical Keyboard", "Compact mechanical keyboard with backlit keys.", "Contoso", "In stock", 89.99m),
    ["SN-10003"] = new("SN-10003", "USB-C Dock", "Docking station with HDMI, Ethernet, and USB ports.", "Contoso", "Out of stock", 119.99m)
};

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/products/{serialNumber}", (string serialNumber) =>
{
    var normalizedSerialNumber = serialNumber.Trim();

    if (string.IsNullOrWhiteSpace(normalizedSerialNumber))
    {
        return Results.BadRequest(new { message = "A serial number is required." });
    }

    return products.TryGetValue(normalizedSerialNumber, out var product)
        ? Results.Ok(product)
        : Results.NotFound(new { message = $"No product was found for serial number '{normalizedSerialNumber}'." });
});

app.Run();

record Product(
    string SerialNumber,
    string Name,
    string Description,
    string Manufacturer,
    string Availability,
    decimal Price);
