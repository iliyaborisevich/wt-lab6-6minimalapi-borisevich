using wt_lab6_6minimalapi_borisevich.Models;

namespace wt_lab6_6minimalapi_borisevich.Data;

public static class CarStore
{
    public static List<Car> Cars { get; } = new()
    {
        new Car { Id = 1, Brand = "Toyota", Model = "Camry", Year = 2022, PricePerDay = 75, IsAvailable = true },
        new Car { Id = 2, Brand = "BMW", Model = "X5", Year = 2021, PricePerDay = 150, IsAvailable = true },
        new Car { Id = 3, Brand = "Kia", Model = "Rio", Year = 2020, PricePerDay = 45, IsAvailable = false },
        new Car { Id = 4, Brand = "Tesla", Model = "Model 3", Year = 2023, PricePerDay = 120, IsAvailable = true },
        new Car { Id = 5, Brand = "Audi", Model = "A6", Year = 2019, PricePerDay = 100, IsAvailable = false }
    };
}