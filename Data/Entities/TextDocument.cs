using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BetterDocs.Areas.Identity;

namespace BetterDocs.Data.Entities
{
    public class TextDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public ApplicationUser Owner { get; set; }
        public virtual ICollection<ApplicationUser> SharedWith { get; set; }
    }
}