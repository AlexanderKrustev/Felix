namespace Fliex.Services
{
    using System;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Threading.Tasks;
    using Felix.Data;
    using Felix.Models;
    using Felix.Models.ViewModels;


    public class AccountService
    {
        private FContext context=new FContext();


        public async Task<User> Register(ApplicationUser appUser, string name)
        {

            User user = new User()
            {
                Name = name,
                ApplicationUserId = appUser.Id
            };

            
           
            using (var dataContext = new FContext())
            {
                try
                {
                    dataContext.Users.Add(user);
                    dataContext.SaveChanges();

                    
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
               
            }

            return user;
        }

        public async Task<User> GetUserInfo(string username)
        {
         
            return this.context.Users.FirstOrDefault(x => x.ApplicationUser.UserName == username);
        }

       
    }
}
