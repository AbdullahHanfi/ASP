using ASP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI.WebControls;
using ASP.Services;


namespace ASP.Services
{
    public interface IUserOfApp
    {
        bool VaildUser(string Email, string password);
        bool VaildEmail(string Email);
    }

    public class UserOfApp : IUserOfApp
    {
        AS_DBEntities db;
        public UserOfApp() { db = new AS_DBEntities(); }

        public bool VaildUser(string Email, string password)
        {
            if (string.IsNullOrEmpty(Email)|| string.IsNullOrEmpty(password))
                return false;
            else
            {
                var check = db.Accounts
                    .Where(M => M.Email == Email&& M.password == password)
                    .Any();
                if (check)
                    return true;
                else
                    return false;
            }
        }

        public bool VaildEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return false;
            else
            {  
                var check=db.Accounts
                    .Where(M=>M.Email== Email)
                    .Any();
                if (check)
                    return false;
                else
                    return true;
            }
        }

    }
}