using aspnetmvc_starter.Models;
using System.Web;

namespace aspnetmvc_starter.Helpers
{
    public static class Auth
    {
        private static User _user;
        
        static Auth()
        {
            _user = (User) HttpContext.Current.Session["UserInfo"];
        }

        public static User User()
        {
            return _user;
        }

        public static long Id()
        {
            return _user.Id;
        }
        
    }
}