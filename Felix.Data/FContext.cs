namespace Felix.Data
{
    using System.Data.Entity;
    using Felix.Models;
    using Felix.Models.Deals;
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

        public virtual DbSet<Buyer> Buyers { get; set; }

        public virtual DbSet<Consignee> Consegnees { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Deal> Deals { get; set; }

        public virtual DbSet<Formulation> Formulations { get; set; }

        public virtual DbSet<Package> Packages { get; set; }

        public virtual DbSet<Port> Ports { get; set; }

        public virtual DbSet<Product>   Products { get; set; }

        public virtual DbSet<TransportCompany> TransportCompanies { get; set; }

        public virtual DbSet<User>  Users { get; set; }


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