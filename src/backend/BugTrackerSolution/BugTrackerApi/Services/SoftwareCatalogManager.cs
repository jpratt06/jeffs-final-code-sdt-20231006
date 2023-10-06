using OneOf;

namespace BugTrackerApi.Services;

public class SoftwareCatalogManager
{
    public async  Task<SoftwareEntityOrNotInCatalog> IsSoftwareInCatalogAsync(string software)
    {
        var supportedSoftware = new List<SoftwareEntity>()
        {
            new SoftwareEntity { Id="excel", Name="Microsoft Excel"},
            new SoftwareEntity { Id="powerpoint", Name ="Microsoft Powerpoint"},
            new SoftwareEntity { Id="code", Name="Visual Studio Code"}
        };


        var softwareEntity = supportedSoftware.SingleOrDefault(s => s.Id == software.ToLower().Trim());

        if (softwareEntity is null)
        {
            return new SoftwareNotInCatalog();
        }
        else
        {
            return softwareEntity;
        }
    }

}

