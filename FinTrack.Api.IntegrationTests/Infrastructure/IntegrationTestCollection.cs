namespace FinTrack.Api.IntegrationTests.Infrastructure;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection
    : ICollectionFixture<PostgreSqlContainerFixture>
{
}