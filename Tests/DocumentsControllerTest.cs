using System.Collections.Generic;
using BetterDocs.Areas.Identity;
using BetterDocs.Controllers;
using BetterDocs.Data.Entities;
using BetterDocs.Models;
using BetterDocs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace BetterDocs.Tests
{
    public class DocumentsControllerTest
    {
        [Fact]
        public void GetTextDocuments()
        {
            var textDocuments = new List<TextDocument>
            {
                new TextDocument {Id = "1", Text = "Test1", Name = "Test2", Owner = new ApplicationUser {Id = "1"}},
                new TextDocument {Id = "2", Text = "Test3", Name = "Test4", Owner = new ApplicationUser {Id = "2"}},
            };
            var documentService = new Mock<IDocumentService>();
            documentService
                .Setup(service => service.GetDocumentsForUser())
                .Returns(textDocuments);
            var controller = new DocumentsController(documentService.Object);

            var result = controller.GetTextDocuments();

            Assert.Equal(textDocuments, result);
        }

        [Fact]
        public void GetTextDocument()
        {
            var textDocument = new TextDocument
                {Id = "1", Text = "Test1", Name = "Test2", Owner = new ApplicationUser {Id = "1"}};
            var documentService = new Mock<IDocumentService>();
            documentService
                .Setup(service => service.GetDocument("1"))
                .Returns(textDocument);
            var controller = new DocumentsController(documentService.Object);

            var result = controller.GetTextDocument("1");

            Assert.Equal(textDocument, result);
        }

        [Fact]
        public void CreateTextDocument()
        {
            var textDocumentModel = new TextDocumentModel
                {Id = "1", Text = "Test1", Name = "Test2"};
            var textDocument = new TextDocument
                {Id = "1", Text = "Test1", Name = "Test2", Owner = new ApplicationUser {Id = "1"}};
            var documentService = new Mock<IDocumentService>();
            documentService
                .Setup(service => service.CreateDocument(textDocumentModel))
                .Returns(textDocument);
            var controller = new DocumentsController(documentService.Object);

            var result = controller.CreateTextDocument(textDocumentModel);

            Assert.Equal("GetTextDocument", result.ActionName);
            Assert.Equal(RouteValueDictionary.FromArray(new[] {new KeyValuePair<string, object>("id", "1")}),
                result.RouteValues);
            Assert.Equal(textDocument, result.Value);
        }
        
        [Fact]
        public void GetDocumentAsPdf()
        {
            var textDocument = new TextDocument
                {Id = "1", Text = "Test1", Name = "Test2", Owner = new ApplicationUser {Id = "1"}};
            var documentService = new Mock<IDocumentService>();
            documentService
                .Setup(service => service.GetDocument("1"))
                .Returns(textDocument);
            var controller = new DocumentsController(documentService.Object);

            var result = controller.GetDocumentAsPdf("1");

            Assert.IsType<FileStreamResult>(result);
        }
    }
}