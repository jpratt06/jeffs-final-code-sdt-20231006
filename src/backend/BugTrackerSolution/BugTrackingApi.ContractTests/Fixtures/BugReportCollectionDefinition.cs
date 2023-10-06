namespace BugTrackingApi.ContractTests.Fixtures;

[CollectionDefinition("FilingABugReport")]
public class BugReportCollectionDefinition : ICollectionFixture<FilingBugReportFixture>
{
}

[CollectionDefinition("SeededDatabaseCollection")]
public class BugReportSeededCollection : ICollectionFixture<SeededDatabaseFixture> { }