namespace FinTrack.Api.IntegrationTests;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection
    : ICollectionFixture<PostgreSqlContainerFixture>
{
}