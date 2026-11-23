using wt_lab6_6minimalapi_borisevich.Data;
using wt_lab6_6minimalapi_borisevich.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// 1
app.MapGet("/", () => "Minimal API работает!");

// 1.1
app.MapGet("/api/info", () => {
    return new {
        Application = "Аренда автомобилей",
        Technology = "ASP.NET Core Minimal API",
        Version = "1.0"
    };
});


app.MapGet("/api/cars", (string? search) => {
    var cars = CarStore.Cars.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        cars = cars.Where(x => x.Brand.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
    return Results.Ok(cars);
});

// ID
app.MapGet("/api/cars/{id}", (int id) => {
    var car = CarStore.Cars.FirstOrDefault(x => x.Id == id);
    if (car == null) return Results.NotFound("Автомобиль не найден");
    return Results.Ok(car);
});

// POST
app.MapPost("/api/cars", (Car car) => {
    // Валидация (Блок 6)
    if (string.IsNullOrWhiteSpace(car.Brand)) 
        return Results.BadRequest("Марка обязательна");

    // ген ID
    car.Id = CarStore.Cars.Any() ? CarStore.Cars.Max(x => x.Id) + 1 : 1;
    CarStore.Cars.Add(car);
    
   
    return Results.Created($"/api/cars/{car.Id}", car);
});


app.MapPut("/api/cars/{id}", (int id, Car updatedCar) => {
    var car = CarStore.Cars.FirstOrDefault(x => x.Id == id);
    if (car == null) return Results.NotFound();

    car.Brand = updatedCar.Brand;
    car.Model = updatedCar.Model;
    car.Year = updatedCar.Year;
    car.PricePerDay = updatedCar.PricePerDay;
    car.IsAvailable = updatedCar.IsAvailable;

    return Results.Ok(car);
});


app.MapDelete("/api/cars/{id}", (int id) => {
    var car = CarStore.Cars.FirstOrDefault(x => x.Id == id);
    if (car == null) return Results.NotFound();
    
    CarStore.Cars.Remove(car);
    
    return Results.NoContent();
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");