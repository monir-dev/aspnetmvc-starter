using System;
using System.Web.Security;

namespace aspnetmvc_starter.Web.Utility
{
    public class CustomMembershipProvider : MembershipProvider
    {
        #region Fields
        //private readonly AIMSUserManagementServiceClient _userAgent;
        #endregion

        #region Ctor
        public CustomMembershipProvider()
        {
           // _userAgent = new AIMSUserManagementServiceClient();
        }
        #endregion

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //User user = UserMgtAgent.GetUserByLoginId(username);


            //if (Common.verifyMd5Hash(oldPassword, user.Password) == false)
            //{
            //    return false;
            //}
            //user.ChangePasswordAtFirstLogin = false;
            //user.Password = Common.getMd5Hash(newPassword);
            //if (UserMgtAgent.UpdateUserData(user) > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        //public User GetUser(string username)
        //{
        //    return UserMgtAgent.GetUserByLoginId(username);
        //}

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 3; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            //User user = new User();
            //user.LoginId = username;
            //user.Password = Common.getMd5Hash(password);
            
            //if (_userAgent.GetUserListByCraiteria(user).Count > 0)
            //{
            //    return true;
            //}
            //else
            //{
                return false;
            //}

           
        }

        //public bool ValidateUser(string username, string password, out User user)
        //{
        //    user = new User();
        //    user.LoginId = username;
        //    user.Password = Common.getMd5Hash(password);

        //    var userList = _userAgent.GetUserListByCraiteria(user);
        //    if (userList.Count > 0)
        //    {
        //        user = userList.FirstOrDefault();
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}