using System.Web;
using aspnetmvc_starter.Main.Core.Domain;

namespace aspnetmvc_starter.Helpers
{
    public class Auth
    {
        public static User User()
        {
            return (User) HttpContext.Current.Session["UserInfo"];
        }
        
        public static long Id()
        {
            return User().Id;
        }
        
        public static bool Guest()
        {
            return User() == null;
        }
        
        public static void DistroySession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}