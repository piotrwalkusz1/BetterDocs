using BetterDocs.Areas.Identity;

namespace BetterDocs.Data.Entities
{
    public class ShareDocument
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string DocumentId { get; set; }
        public TextDocument Document { get; set; }
    }
}