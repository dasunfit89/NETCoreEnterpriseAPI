using System.Collections.Generic;
using System.Linq;

namespace EnterpriseApp.API.Core.Authorizations
{
    public class Permissions
    {
        #region PublicMessage

        public const string ManagePublicMessages = "ManagePublicMessages";

        #endregion

        #region ViewPartyInformation

        public const string ViewPartyInformation = "ViewPartyInformation";

        public const string ViewPublicInformation = "ViewPublicInformation";

        #endregion

        #region PartyDesignation 

        public const string ManagePartyDesignation = "ManagePartyDesignation";

        public const string ViewPartyDesignation = "ViewPartyDesignation";

        #endregion

        #region Admin

        public const string SuperUser = "SuperUser";

        public const string ManagePosts = "ManagePosts";

        #endregion

        protected Permissions()
        {
        }

        public static List<string> GetAll()
        {
            var permission = new Permissions();

            var fieldValues = permission.GetType()
                           .GetFields()
                           .Select(field => field.GetValue(permission).ToString())
                           .ToList();

            return fieldValues;
        }
    }

    public class UserPermissions
    {
        #region SendInformation

        public const string SendPartyInformation = "PublicSendPartyInformation";

        public const string SendPublicInformation = "PublicSendInformation";

        public const string SendNews = "PublicSendNews";

        #endregion

        protected UserPermissions()
        {
        }

        public static List<string> GetAll()
        {
            var permission = new UserPermissions();

            var fieldValues = permission.GetType()
                           .GetFields()
                           .Select(field => field.GetValue(permission).ToString())
                           .ToList();

            return fieldValues;
        }
    }
}