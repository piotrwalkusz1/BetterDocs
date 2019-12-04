using System.ComponentModel.DataAnnotations.Schema;

namespace BetterDocs.Data.Entities
{
    public class TextDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }

        // TODO: Change to list (?)
        public string OwnerId { get; set; }
        public string ContributorId { get; set; }
        public string Text { get; set; }
    }
}