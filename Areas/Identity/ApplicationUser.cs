using System.Collections.Generic;
using BetterDocs.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BetterDocs.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ShareDocument> DocumentsSharing { get; set; } = new List<ShareDocument>();
    }
}