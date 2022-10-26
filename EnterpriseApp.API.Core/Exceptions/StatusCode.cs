namespace EnterpriseApp.API.Core.Exceptions
{
    public enum StatusCode
    {
        SUCCESS = 1000,
        ERROR_Common = 1001,
        ERROR_InvalidParameters = 1002,
        ERROR_InvalidUser = 1003,
        ERROR_UserExists = 1004,
        ERROR_PasswordsMismatch = 1005,
        ERROR_InvalidCredentials = 1006,
        ERROR_UserDeactivated = 1007,
        ERROR_PasswordRequired = 1008,
        ERROR_UConfirmPasswordRequired = 1009,
        ERROR_FBSingupRequired = 1010,
        ERROR_NotFBUser = 1011,
        ERROR_DuplicateUserName = 1012,
        ERROR_DuplicateUserPhone = 1013,

        ERROR_InvalidArticle = 1101,
        ERROR_InvalidArticleComment = 1102,
        ERROR_ArticleDeactivated = 1103,
        ERROR_InvalidCountry = 1104,
        ERROR_CountryDeactivated = 1105,
        ERROR_ListExists = 1106,
        ERROR_InvalidArticleCategory = 1107,

        ERROR_InvalidFile = 1201,
        ERROR_FileNotFound = 1202,
        ERROR_InvalidFileUpload = 1203,
        ERROR_FileDeactivated = 1204,
        ERROR_InvalidArticleList = 1205,
        ERROR_InvalidArticleListEntry = 1206,

        ERROR_InvalidUserType = 1301,
        ERROR_InvalidCategory = 1302,
        ERROR_InvalidDesignation = 1302,
        ERROR_InvalidSubCategory = 1303,
        ERROR_DuplicateUserNic = 1304,
        ERROR_InvalidData = 1305,
        ERROR_InvalidOTP = 1306,
    }
}
