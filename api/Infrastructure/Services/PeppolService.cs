using System.Text;
using System.Xml;
using System.Xml.Serialization;
using api.Contracts.Responses;

namespace api.Infrastructure.Services
{
    public class PeppolService
    {
        public string GenerateInvoicePeppolXml(InvoiceResponseDto invoice)
        {
            var xmlSerializer = new XmlSerializer(typeof(InvoiceResponseDto));
            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true });
            xmlSerializer.Serialize(xmlWriter, invoice);
            return stringWriter.ToString();
        }
    }
}
