namespace Felix.Web.Controllers
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Felix.Models;
    using Felix.Models.BindingModels;
    using Felix.Models.ViewModels;
    using Felix.Web;
    using Fliex.Services;
    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;

    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private AccountService service;

        public AccountController()
        {
            this.service=new AccountService();
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            this.UserManager = userManager;
            this.AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this._userManager ?? this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                this._userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        // GET api/Account/UserInfo
        [Authorize]
        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public async Task<UserInfoVM> GetUserInfo(UserInfoBM userInfo)
        {
            User user = await this.service.GetUserInfo(userInfo.UserName);
           
           
           if (user == null)
           {
               return null;
           }
        
        
           return new UserInfoVM
           {
               Id = user.Id,
               Name = user.Name,
               Roles =await this.UserManager.GetRolesAsync(user.ApplicationUserId)
               
           };

         
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return this.Ok();
        }

      
        

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBM model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var appuser = new ApplicationUser() { UserName = model.Email, Email = model.Email };
           
            IdentityResult result = await this.UserManager.CreateAsync(appuser, model.Password);

            await this.UserManager.AddToRoleAsync(appuser.Id, "Operator");

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }



            User user = await this.service.Register(appuser, model.Name);

            user.ApplicationUser = await this.UserManager.FindByIdAsync(user.ApplicationUserId);

            var roles= await this.UserManager.GetRolesAsync(user.ApplicationUserId);

            return this.Ok(
                new UserInfoVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Roles = roles
                });
        }

  
        protected override void Dispose(bool disposing)
        {
            if (disposing && this._userManager != null)
            {
                this._userManager.Dispose();
                this._userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication => this.Request.GetOwinContext().Authentication;
        
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError("", error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }

       
        #endregion
    }
}
