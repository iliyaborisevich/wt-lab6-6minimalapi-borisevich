using wt_lab6_6minimalapi_borisevich.Data;
using wt_lab6_6minimalapi_borisevich.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Блок 1: Приветствие
app.MapGet("/", () => "Minimal API работает!");

// Блок 1: Информация о приложении
app.MapGet("/api/info", () => {
    return new {
        Application = "Аренда автомобилей",
        Technology = "ASP.NET Core Minimal API",
        Version = "1.0"
    };
});

// Блок 3.1: Получить все машины (с фильтрацией по марке)
app.MapGet("/api/cars", (string? search) => {
    var cars = CarStore.Cars.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        cars = cars.Where(x => x.Brand.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
    return Results.Ok(cars);
});

// Блок 3.2: Получить машину по ID
app.MapGet("/api/cars/{id}", (int id) => {
    var car = CarStore.Cars.FirstOrDefault(x => x.Id == id);
    if (car == null) return Results.NotFound("Автомобиль не найден");
    return Results.Ok(car);
});

// Блок 4: Добавить машину (POST)
app.MapPost("/api/cars", (Car car) => {
    // Валидация (Блок 6)
    if (string.IsNullOrWhiteSpace(car.Brand)) 
        return Results.BadRequest("Марка обязательна");

    // Авто-генерация ID
    car.Id = CarStore.Cars.Any() ? CarStore.Cars.Max(x => x.Id) + 1 : 1;
    CarStore.Cars.Add(car);
    
    // Возврат 201 Created с указанием URL нового объекта
    return Results.Created($"/api/cars/{car.Id}", car);
});

// Блок 5.1: Обновить машину (PUT)
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

// Блок 5.2: Удалить машину (DELETE)
app.MapDelete("/api/cars/{id}", (int id) => {
    var car = CarStore.Cars.FirstOrDefault(x => x.Id == id);
    if (car == null) return Results.NotFound();
    
    CarStore.Cars.Remove(car);
    
    // Возврат 204 No Content
    return Results.NoContent();
});

// Блок 7.2: Настройка порта для Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");