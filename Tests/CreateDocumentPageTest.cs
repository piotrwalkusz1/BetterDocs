using BetterDocs.Areas.Identity;
using BetterDocs.Data.Entities;
using BetterDocs.Models;
using BetterDocs.Pages.Documents;
using BetterDocs.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BetterDocs.Tests
{
    public class CreateDocumentPageTest
    {
        [Fact]
        public void CreateTextDocument_Success()
        {
            var textDocument = new TextDocument
                {Id = "1", Text = "Test1", Name = "Test2", Owner = new ApplicationUser {Id = "1"}};
            var documentService = new Mock<IDocumentService>();
            documentService
                .Setup(service => service.CreateDocument(It.IsAny<TextDocumentModel>()))
                .Returns(textDocument);
            var page = new Create(documentService.Object);

            page.TextDocumentModel = new TextDocumentModel {Id = "1", Text = "Test1", Name = "Test2"};
            var result = page.OnPost();

            documentService.Verify(service => service.CreateDocument(page.TextDocumentModel), Times.Once());
            Assert.IsAssignableFrom<RedirectToPageResult>(result);
        }
    }
}