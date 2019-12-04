using System.Collections.Generic;
using BetterDocs.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BetterDocs.Data
{
    public class DocumentsDbContext : DbContext
    {
        public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options)
        {
        }
        public DbSet<TextDocument> TextDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var textDocument = new TextDocument
            {
                Id = "1",
                ContributorId = "7993615d-93bf-4e07-9b4f-a193e55cf1a9",
                Text = "empty",
                OwnerId = "7993615d-93bf-4e07-9b4f-a193e55cf1a9"
            };

            modelBuilder
                .Entity<TextDocument>()
                .HasData(textDocument);
        }
    }
}