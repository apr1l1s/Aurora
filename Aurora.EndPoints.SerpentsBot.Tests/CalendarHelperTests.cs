using System.Runtime.CompilerServices;
using Aurora.EndPoints.SerpensBot.Helpers;
using Microsoft.AspNetCore.Http.Features;

namespace Aurora.EndPoints.SerpentsBot.Tests;

public class CalendarHelperTests
{
    [Fact]
    public void IsWorkingDay_ShouldReturnTrue()
    {
        //Arrange
        var week = new List<DateTime>
        {
            new(new DateOnly(2024, 02, 17), new TimeOnly(10, 0, 0)),
            new(new DateOnly(2024, 02, 18), new TimeOnly(10, 0, 0)),
            new(new DateOnly(2024, 02, 19), new TimeOnly(10, 0, 0)),
            new(new DateOnly(2024, 02, 20), new TimeOnly(10, 0, 0)),
            new(new DateOnly(2024, 02, 21), new TimeOnly(10, 0, 0)),
        };

        var weekBool = new List<bool>(7);

        //Act
        foreach (var day in week)
        {
            weekBool.Add(CalendarHelper.IsWorkingDay(day));
        }

        //Assert
        var result = weekBool.Any(x => !x);

        Assert.True(result);
    }

    [Fact]
    public void IsWorkingDay_ShouldReturnFalse()
    {
        //Arrange
        var week = new List<DateTime>
        {
            new(new DateOnly(2024, 02, 17), new TimeOnly(6, 0, 0)),
            new(new DateOnly(2024, 02, 18), new TimeOnly(7, 0, 0)),
            new(new DateOnly(2024, 02, 19), new TimeOnly(8, 0, 0)),
            new(new DateOnly(2024, 02, 20), new TimeOnly(21, 0, 0)),
            new(new DateOnly(2024, 02, 21), new TimeOnly(22, 0, 0)),
            new(new DateOnly(2024, 02, 22), new TimeOnly(10, 0, 0)),
            new(new DateOnly(2024, 02, 23), new TimeOnly(10, 0, 0))
        };

        var weekBool = new List<bool>(7);

        //Act
        foreach (var day in week)
        {
            weekBool.Add(CalendarHelper.IsWorkingDay(day));
        }

        //Assert
        var result = weekBool.Any(x => x);

        Assert.True(result);
    }
}