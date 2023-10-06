using BugTrackerApi.Controllers;
using BugTrackerApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace BugTrackerApi.UnitTests;
public class BugTrackerValidationTests
{

    [Fact]
    public void BugTrackerModelHasCorrectAttributes()
    {
        var type = typeof(BugReportCreateRequest);

        var descriptionProperty = type.GetProperty(nameof(BugReportCreateRequest.Description));
        var narrativeProperty = type.GetProperty(nameof(BugReportCreateRequest.Narrative));

        descriptionProperty.Should().BeDecoratedWith<RequiredAttribute>();
        descriptionProperty.Should().BeDecoratedWith<MinLengthAttribute>(a => a.Length == 5);

    }

    [Fact]
    public void ButTrackerControllerShouldBeAnApiController()
    {
        var type = typeof(BugReportController);

        type.Should().BeDecoratedWith<ApiControllerAttribute>();
    }
}
