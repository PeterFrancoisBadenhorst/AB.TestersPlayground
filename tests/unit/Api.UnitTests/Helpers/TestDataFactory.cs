namespace Api.UnitTests.Helpers;

/// <summary>
/// Test data builders and factories for creating test objects
/// Demonstrates the Builder pattern for test data creation
/// </summary>
public class WeatherForecastTestBuilder
{
    private DateOnly _date = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
    private int _temperatureC = 20;
    private string _summary = "Mild";

    public static WeatherForecastTestBuilder Create() => new();

    public WeatherForecastTestBuilder WithDate(DateOnly date)
    {
        _date = date;
        return this;
    }

    public WeatherForecastTestBuilder WithTemperature(int temperatureC)
    {
        _temperatureC = temperatureC;
        return this;
    }

    public WeatherForecastTestBuilder WithSummary(string summary)
    {
        _summary = summary;
        return this;
    }

    public WeatherForecastTestBuilder ForFreezing()
    {
        _temperatureC = -5;
        _summary = "Freezing";
        return this;
    }

    public WeatherForecastTestBuilder ForHot()
    {
        _temperatureC = 35;
        _summary = "Hot";
        return this;
    }

    public TestWeatherForecast Build() => new()
    {
        Date = _date,
        TemperatureC = _temperatureC,
        Summary = _summary
    };

    public IEnumerable<TestWeatherForecast> BuildMany(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => WithDate(_date.AddDays(i)).Build());
    }
}

/// <summary>
/// Factory for creating common test scenarios
/// </summary>
public static class TestDataFactory
{
    public static IEnumerable<TestWeatherForecast> CreateWeeklyForecast()
    {
        return WeatherForecastTestBuilder.Create().BuildMany(7);
    }

    public static TestWeatherForecast CreateFreezingDay()
    {
        return WeatherForecastTestBuilder.Create()
            .ForFreezing()
            .WithDate(DateOnly.FromDateTime(DateTime.Today))
            .Build();
    }

    public static TestWeatherForecast CreateHotDay()
    {
        return WeatherForecastTestBuilder.Create()
            .ForHot()
            .WithDate(DateOnly.FromDateTime(DateTime.Today.AddDays(1)))
            .Build();
    }

    public static IEnumerable<object[]> TemperatureTestCases()
    {
        yield return new object[] { -20, "Freezing" };
        yield return new object[] { 0, "Chilly" };
        yield return new object[] { 20, "Mild" };
        yield return new object[] { 35, "Hot" };
    }

    public static IEnumerable<object[]> InvalidTemperatureCases()
    {
        yield return new object[] { -100 };
        yield return new object[] { 100 };
        yield return new object[] { int.MinValue };
        yield return new object[] { int.MaxValue };
    }
}

public class TestWeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}
