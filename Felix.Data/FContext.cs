namespace Felix.Data
{
    using Felix.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class FContext : IdentityDbContext<ApplicationUser>
    {
        // Your context has been configured to use a 'FContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Felix.Data.FContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'FContext' 
        // connection string in the application configuration file.
        public FContext()
            : base("FContext", throwIfV1Schema: false)
        {
        }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public static FContext Create()
       {
           return new FContext();
       }

        //public class MyEntity
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //}
    }
}