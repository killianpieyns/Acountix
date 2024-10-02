using NReco.PdfGenerator;

public class PdfService
{
    private readonly HtmlToPdfConverter _htmlToPdfConverter;

    public PdfService()
    {
        _htmlToPdfConverter = new HtmlToPdfConverter();
    }

    public byte[] GeneratePdfFromHtml(string htmlContent)
    {
        return _htmlToPdfConverter.GeneratePdf(htmlContent);
    }
}
