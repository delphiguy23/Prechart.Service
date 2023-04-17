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
    public class WhenGetUnProcessUploadedTaxFiling : LoonheffingsRepositorySpec
    {
        public class AndException : WhenGetUnProcessUploadedTaxFiling
        {
            [Fact]
            public async void ThenException()
            {
                //Arrange
                _mockCollection
                    .Setup(op => op.Aggregate(
                        It.IsAny<PipelineDefinition<XmlLoonaangifteUpload, UnprocessedUploads>>(),
                        It.IsAny<AggregateOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Callback(() => throw new Exception("exception"));

                //Act
                var result = await Subject.HandleAsync(new LoonheffingsRepository.GetUnProcessUploadedTaxFiling(), CancellationToken.None);

                //Assert
                _mockCollection.Verify(c => c.Aggregate(
                    It.IsAny<PipelineDefinition<XmlLoonaangifteUpload, UnprocessedUploads>>(),
                    It.IsAny<AggregateOptions>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
                result.IsFailure().Should().BeTrue();
                result.Messages.Any(_ => _.Contains("exception")).Should().BeTrue();
            }
        }

        public class AndFound : WhenGetUnProcessUploadedTaxFiling
        {
            [Fact]
            public async void ThenDataFound()
            {
                //Arrange
                _mockAsyncCursorUnprocessedUploads.Setup(a => a.Current).Returns(unprocessedUploadsWithNormalAndCorrection);
                _mockAsyncCursorUnprocessedUploads.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);

                _mockCollection
                    .Setup(op => op.Aggregate(
                        It.IsAny<PipelineDefinition<XmlLoonaangifteUpload, UnprocessedUploads>>(),
                        It.IsAny<AggregateOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(_mockAsyncCursorUnprocessedUploads.Object);

                //Act
                var result = await Subject.HandleAsync(new LoonheffingsRepository.GetUnProcessUploadedTaxFiling(), CancellationToken.None);

                //Assert
                _logger.ReceivedLogMessages
                    .Where(log => log.LogLevel == LogLevel.Information)
                    .Any(log => log.Message.StartsWith("Start")).Should().BeTrue();

                _mockCollection.Verify(c => c.Aggregate(
                    It.IsAny<PipelineDefinition<XmlLoonaangifteUpload, UnprocessedUploads>>(),
                    It.IsAny<AggregateOptions>(),
                    It.IsAny<CancellationToken>()), Times.AtLeastOnce);

                _logger.ReceivedLogMessages
                    .Where(log => log.LogLevel == LogLevel.Information)
                    .Any(log => log.Message.StartsWith("End")).Should().BeTrue();

                result.IsSuccess().Should().BeTrue();
            }
        }
    }
}
