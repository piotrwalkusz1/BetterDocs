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
    }
}