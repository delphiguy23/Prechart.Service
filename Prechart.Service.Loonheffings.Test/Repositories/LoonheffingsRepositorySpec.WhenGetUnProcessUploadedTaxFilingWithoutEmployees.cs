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
    public class WhenGetUnProcessUploadedTaxFilingWithoutEmployees : LoonheffingsRepositorySpec
    {
        public class AndException : WhenGetUnProcessUploadedTaxFilingWithoutEmployees
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.UpdateManyAsync(
                        It.IsAny<FilterDefinition<XmlLoonaangifteUpload>>(),
                        It.IsAny<UpdateDefinition<XmlLoonaangifteUpload>>(),
                        It.IsAny<UpdateOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new LoonheffingsRepository.GetUnProcessUploadedTaxFilingWithoutEmployees(), CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.UpdateManyAsync(
                    It.IsAny<FilterDefinition<XmlLoonaangifteUpload>>(),
                    It.IsAny<UpdateDefinition<XmlLoonaangifteUpload>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()), Times.Once);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Any(_ => _.Contains("exception")).Should().BeTrue();
            }
        }

        public class AndUpdated : WhenGetUnProcessUploadedTaxFilingWithoutEmployees
        {
            [Fact]
            public async void ThenUpdated()
            {
                //Arrange

                _mockUpdateResult.Setup(_ => _.IsAcknowledged).Returns(true);
                _mockUpdateResult.Setup(_ => _.ModifiedCount).Returns(1);

                _mockCollection
                    .Setup(op => op.UpdateManyAsync(
                        It.IsAny<FilterDefinition<XmlLoonaangifteUpload>>(),
                        It.IsAny<UpdateDefinition<XmlLoonaangifteUpload>>(),
                        It.IsAny<UpdateOptions>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_mockUpdateResult.Object);

                //Act
                var result = await Subject.HandleAsync(new LoonheffingsRepository.GetUnProcessUploadedTaxFilingWithoutEmployees(), CancellationToken.None);

                //Assert
                _logger.ReceivedLogMessages
                    .Where(log => log.LogLevel == LogLevel.Information)
                    .Any(log => log.Message.StartsWith("Start")).Should().BeTrue();

                _mockCollection.Verify(c => c.UpdateManyAsync(
                    It.IsAny<FilterDefinition<XmlLoonaangifteUpload>>(),
                    It.IsAny<UpdateDefinition<XmlLoonaangifteUpload>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()), Times.Once);

                _logger.ReceivedLogMessages
                    .Where(log => log.LogLevel == LogLevel.Information)
                    .Any(log => log.Message.StartsWith("End")).Should().BeTrue();

                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
