using System.Text;
using FluentAssertions;
using NSubstitute;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Core.TestBase;
using Prechart.Service.Globals.Interfaces.Documents;
using Prechart.Service.Loonheffings.Models;
using Prechart.Service.Loonheffings.Repositories;
using Prechart.Service.Loonheffings.Service;
using Xunit;

namespace Prechart.Service.Loonheffings.Test.Service;

public partial class LoonheffingsServiceSpec
{
    public class WhenValidateXml : LoonheffingsServiceSpec
    {
        private readonly List<IXmlStream> files = new();
        private IFluentResults<UploadXmlResults> result;
        private int year;

        public When LoonheffingsProcessedResult => async () => result = await Subject.HandleAsync(new LoonheffingsService.ValidateXml
        {
            Files = files,
            XsdYear = year,
        }, CancellationToken.None);

        public class AndException : WhenValidateXml
        {
            public And Data => () =>
            {
                files.Add(_xmlDataBytes);
                year = 2022;
            };

            public And GetData => () => _helper.GetSchemaStream($"Prechart.Service.Globals.Models.Loonheffings.Xsd.Files.Loonaangifte{2022}v2.0.xsd")
                .Returns(e => throw new Exception("exception"));

            [Fact]
            public void ThenShouldReturnFailure()
            {
                result.IsFailure().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnException()
            {
                result.Messages.Any(m => m.Contains("exception")).Should().BeTrue();
            }

            [Fact]
            public void ThenLogger()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Error && log.Message == "exception").Should().BeTrue();
            }
        }

        public class AndNoFiles : WhenValidateXml
        {
            public And Data => () => { year = 2022; };

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenLogger()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.Contains("No Files")).Should().BeTrue();
            }
        }

        public class AndNoYear : WhenValidateXml
        {
            public And Data => () =>
            {
                files.Add(_xmlDataBytes);
                year = 0;
            };

            [Fact]
            public void ThenShouldReturnNone()
            {
                result.IsNotFoundOrBadRequest().Should().BeTrue();
            }

            [Fact]
            public void ThenLogger()
            {
                _logger.ReceivedLogMessages.Any(log => log.LogLevel == LogLevel.Information && log.Message.Contains("No Files")).Should().BeTrue();
            }
        }

        public class AndNoXmlSchema : WhenValidateXml
        {
            public And Data => () =>
            {
                files.Add(_xmlDataBytes);
                year = 2022;
            };

            public And GetData => () => _helper.GetSchemaStream($"Prechart.Service.Globals.Models.Loonheffings.Xsd.Files.Loonaangifte{2022}v2.0.xsd")
                .Returns((Stream) null);

            [Fact]
            public void ThenShouldReturnFailure()
            {
                result.IsFailure().Should().BeTrue();
            }

            [Fact]
            public void ThenShouldReturnError()
            {
                result.Messages.Any(m => m.Contains("No XSD Schema found for 2022")).Should().BeTrue();
            }
        }

        public class AndValidation : WhenValidateXml
        {
            public And Data => () =>
            {
                files.Add(_xmlDataBytes);
                year = 2022;
            };

            public And GetScehamaData => () => _helper.GetSchemaStream($"Prechart.Service.Globals.Models.Loonheffings.Xsd.Files.Loonaangifte{2022}v2.0.xsd")
                .Returns(s =>
                {
                    var byteArray = Encoding.ASCII.GetBytes("data");
                    var stream = new MemoryStream(byteArray);
                    return stream;
                });

            public class AndNullValid : AndValidation
            {
                [Fact]
                public void ThenShouldReturnSome()
                {
                    result.Value.Status
                        .FirstOrDefault(s => s.IsValid == false)
                        .Errors.Any(e => e.Contains("Validator returns null.")).Should().BeTrue();
                }
            }

            public class AndNotValid : AndValidation
            {
                public And ValidatedData => () => _helper.Validate(Arg.Any<byte[]>(), Arg.Any<int>())
                    .Returns<List<string>>(s => new List<string> {"error1", "error2", "error3"});

                [Fact]
                public void ThenShouldReturnSome()
                {
                    result.Value.Status
                        .FirstOrDefault(s => s.IsValid == false)
                        .Errors.Should().BeEquivalentTo(new List<string> {"error1", "error2", "error3"});
                }
            }

            public class AndIsValid : AndValidation
            {
                public And ValidatedData => () => _helper.Validate(Arg.Any<byte[]>(), Arg.Any<int>())
                    .Returns<List<string>>(new List<string>());

                [Fact]
                public void ThenShouldReturnSome()
                {
                    result.Value.Status
                        .FirstOrDefault(s => s.IsValid)
                        .FileName.Should().Be(_xmlDataBytes.FileName);
                }

                [Fact]
                public async Task ThenHandlerIsCalled()
                {
                    await _repository.Received(1).HandleAsync(Verify.That<LoonheffingsRepository.UpsertTaxFiling2022>(i => i.Should()
                        .BeEquivalentTo(new LoonheffingsRepository.UpsertTaxFiling2022
                            {
                                TaxFiling = new XmlLoonaangifteUpload
                                {
                                    FileName = _xmlDataBytes.FileName,
                                    IsValid = true,
                                    Errors = null,
                                    EmployeesInserted = 0,
                                    EmployeesUpdated = 0,
                                    Processed = false,
                                },
                            }, option =>
                                option
                                    .Excluding(exclude => exclude.TaxFiling.Loonaangifte)
                                    .Excluding(exclude => exclude.TaxFiling.UploadedDate)
                        )), CancellationToken.None);
                }
            }
        }
    }
}
