using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace BetterDocs.Controllers
{
    [Route("documents")]
    public class DocumentController : Controller
    {
        [HttpGet("{id}/pdf")]
        public IActionResult GetDocumentAsPdf(int id)
        {
            // TODO: Pobrać dokument z bazy danych
            var content = IronPdf.HtmlToPdf.StaticRenderHtmlAsPdf("Test").Stream;
            var contentType = "APPLICATION/octet-stream";
            // TODO: nazwa dokumentu
            var fileName = "Document.pdf";
            return File(content, contentType, fileName);
        }
    }
}