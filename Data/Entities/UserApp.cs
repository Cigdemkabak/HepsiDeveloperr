using HepsiDeveloper.Data.Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace HepsiDeveloper.Data.Entities
{
    public class UserApp : IdentityUser<int>, IBaseClass
    {
        public string FullName { get; set; }
        public DateTime CreatedOn { get ; set ; }
        public DateTime UpdatedOn { get ; set ; }
        public bool IsActive { get ; set ; }
    }
}
