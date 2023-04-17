using System.Text;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Extensions.Logging;

namespace Prechart.Service.Globals.Helper;

public class XsdHelper : IXsdHelper
{
    private readonly List<string> _errors;
    private readonly ILogger<XsdHelper> _logger;

    public XsdHelper(ILogger<XsdHelper> logger)
    {
        _logger = logger;
        _errors = new List<string>();
    }

    private List<string> Errors
    {
        get => _errors;
        set => value = _errors;
    }

    public Stream GetSchemaStream(string xsdFileName)
    {
        return GetType().Assembly.GetManifestResourceStream(xsdFileName);
    }

    public async Task<List<string>> Validate(byte[] xmlStream, int xsdYear)
    {
        try
        {
            var xsdResourceName = $"Prechart.Service.Globals.Models.Loonheffings.Xsd.Files.Loonaangifte{xsdYear}v2.0.xsd";
            var schemaStream = GetSchemaStream(xsdResourceName);
            if (schemaStream is null)
            {
                Errors.Add($"No XSD Schema found for {xsdYear}");
                return Errors;
            }

            var schemaUri = new Uri($"http://xml.belastingdienst.nl/schemas/Loonaangifte/{xsdYear}/01");
            var readerSettings = new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                ValidationType = ValidationType.Schema,
                Async = true,
            };

            using (var schemaXml = XmlReader.Create(schemaStream))
            {
                readerSettings.Schemas.Add(schemaUri.ToString(), schemaXml);
                readerSettings.ValidationEventHandler += ValidationEventHandler;

                var xmlstr = new StringBuilder(Encoding.UTF8.GetString(xmlStream));
                var document = new XmlDocument();
                document.LoadXml(xmlstr.ToString());

                using (var xmlReader = XmlReader.Create(new StringReader(document.InnerXml), readerSettings))
                {
                    Errors.Clear();

                    while (await xmlReader.ReadAsync())
                    {
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            Errors.Add(ex.Message);
        }

        return Errors;
    }

    private void ValidationEventHandler(object sender, ValidationEventArgs e)
    {
        if (e.Severity == XmlSeverityType.Error)
        {
            Errors.Add(e.Exception != null ? $"Error in line {e.Exception.LineNumber}, position {e.Exception.LinePosition}: {e.Message}" : e.Message);
        }
    }
}
