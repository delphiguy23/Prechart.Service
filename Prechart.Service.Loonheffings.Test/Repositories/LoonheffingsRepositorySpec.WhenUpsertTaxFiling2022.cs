using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Loonheffings.Models;
using Prechart.Service.Loonheffings.Repositories;
using Xunit;

namespace Prechart.Service.Loonheffings.Test.Repositories;

public partial class LoonheffingsRepositorySpec
{
    public class WhenUpsertTaxFiling2022 : LoonheffingsRepositorySpec
    {
        public class AndInserted : WhenUpsertTaxFiling2022
        {
            [Fact]
            public async void ThenInserted()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<XmlLoonaangifteUpload>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult((object) null));

                //Act
                var result = await Subject.HandleAsync(new LoonheffingsRepository.UpsertTaxFiling2022
                {
                    TaxFiling = taxFiling,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.InsertOneAsync(taxFiling, null, default), Times.Once);
                result.IsSuccess().Should().BeTrue();
            }
        }

        public class AndException : WhenUpsertTaxFiling2022
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.InsertOneAsync(
                        It.IsAny<XmlLoonaangifteUpload>(),
                        It.IsAny<InsertOneOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                var result = await Subject.HandleAsync(new LoonheffingsRepository.UpsertTaxFiling2022
                {
                    TaxFiling = taxFiling,
                }, CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.InsertOneAsync(taxFiling, null, default), Times.Once);
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error).Should().BeTrue();
                result.IsFailure().Should().BeTrue();
            }
        }
    }
}
