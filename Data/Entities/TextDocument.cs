using System.Collections.Generic;

namespace BetterDocs.Data.Entities
{
    public class TextDocument
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        // TODO: Change to list (?)
        public string ContributorId { get; set; }
        public string Text { get; set; }
    }
}