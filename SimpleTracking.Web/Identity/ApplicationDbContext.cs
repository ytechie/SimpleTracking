using ElCamino.AspNet.Identity.AzureTable;

namespace SimpleTracking.Web.Identity
{
    public class ApplicationDbContext : IdentityCloudContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("StorageConnectionString")
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}