using BugTrackerApi.Models;
using BugTrackerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerApi.Controllers;

[ApiController]
public class BugReportController : ControllerBase
{

    private readonly BugReportManager _bugManager;

    public BugReportController(BugReportManager bugManager)
    {
        _bugManager = bugManager;
    }

    [Authorize]
    [HttpPost("/catalog/{software}/bugs")]
    public async Task<ActionResult<BugReportCreateResponse>> AddABugReport([FromBody] BugReportCreateRequest request, [FromRoute] string software)
    {

        var user = User.GetName();
        var response = await _bugManager.CreateBugReportAsync(user, software, request);

        return response.Match<ActionResult>(
            report => CreatedAtRoute("bugreportcontroller#getthebugreport", new { software = software, id = report.Id }, report),
            _ => NotFound("That software is not supported")
            );
    }

    [Authorize]
    [HttpGet("/catalog/{software}/bugs/{id}", Name = "bugreportcontroller#getthebugreport")]
    public async Task<ActionResult> GetTheBugReport([FromRoute] string software, [FromRoute] string id)
    {

        var response = await _bugManager.GetBugReportByIdAsync(id);
        return response.Match<ActionResult>(
            report => Ok(report),
            _ => NotFound()
            );
    }

    [Authorize]
    [HttpGet("/catalog/{software}/bugs")]
    public async Task<ActionResult> GetTheBugsForAPieceOfSoftware([FromRoute] string software)
    {
        var response = await _bugManager.GetBugsForSoftwareAsync(software);
        return response.Match<ActionResult>(
            bugs => Ok(bugs.data),
            _ => NotFound()
            );

    }


}
