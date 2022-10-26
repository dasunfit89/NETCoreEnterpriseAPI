using System;
namespace EnterpriseApp.API.Core.Exceptions
{
    public class APIBaseException : Exception
    {
        public StatusCode StatusCode { get; }

        public APIBaseException(StatusCode code)
        {
            StatusCode = code;
        }

        public APIBaseException(StatusCode code, string message) : base(message)
        {
            StatusCode = code;
        }

        public APIBaseException(StatusCode code, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = code;
        }

        public static string MapError(StatusCode code)
        {
            string errorMessage = string.Empty;

            switch (code)
            {
                case StatusCode.SUCCESS:
                    break;
                case StatusCode.ERROR_Common:
                    errorMessage = "Common Error";
                    break;
                case StatusCode.ERROR_InvalidParameters:
                    errorMessage = "Invalid Parameters";
                    break;
                case StatusCode.ERROR_InvalidUser:
                    errorMessage = "Invalid User";
                    break;
                case StatusCode.ERROR_UserExists:
                    errorMessage = "User alredy exists in the system";
                    break;
                case StatusCode.ERROR_PasswordsMismatch:
                    errorMessage = "Passwords Mismatch";
                    break;
                case StatusCode.ERROR_InvalidCredentials:
                    errorMessage = "Invalid Credentials";
                    break;
                case StatusCode.ERROR_UserDeactivated:
                    errorMessage = "User Deactivated";
                    break;
                case StatusCode.ERROR_InvalidArticle:
                    errorMessage = "Invalid Article";
                    break;
                case StatusCode.ERROR_InvalidArticleComment:
                    errorMessage = "Invalid Article Comment";
                    break;
                case StatusCode.ERROR_ArticleDeactivated:
                    errorMessage = "Article Deactivated";
                    break;
                case StatusCode.ERROR_PasswordRequired:
                    errorMessage = "Password required";
                    break;
                case StatusCode.ERROR_UConfirmPasswordRequired:
                    errorMessage = "Confirm password required";
                    break;
                case StatusCode.ERROR_FBSingupRequired:
                    errorMessage = "Please signup through facebook ";
                    break;
                case StatusCode.ERROR_InvalidCountry:
                    errorMessage = "Invalid Country";
                    break;
                case StatusCode.ERROR_CountryDeactivated:
                    errorMessage = "Country Deactivated";
                    break;
                case StatusCode.ERROR_ListExists:
                    errorMessage = "List Exists";
                    break;
                case StatusCode.ERROR_InvalidFile:
                    errorMessage = "Invalid File";
                    break;
                case StatusCode.ERROR_FileNotFound:
                    errorMessage = "File Not Found";
                    break;
                case StatusCode.ERROR_InvalidFileUpload:
                    break;
                case StatusCode.ERROR_FileDeactivated:
                    errorMessage = "File Deactivated";
                    break;
                case StatusCode.ERROR_InvalidUserType:
                    errorMessage = "Invalid User Type";
                    break;
                case StatusCode.ERROR_InvalidArticleList:
                    errorMessage = "Invalid Article List";
                    break;
                case StatusCode.ERROR_InvalidArticleListEntry:
                    errorMessage = "Invalid Article List Entry";
                    break;
                case StatusCode.ERROR_NotFBUser:
                    errorMessage = "User is not a Facebook user";
                    break;
                case StatusCode.ERROR_InvalidArticleCategory:
                    break;
                case StatusCode.ERROR_DuplicateUserName:
                    errorMessage = "Username already in use";
                    break;
                default:
                    errorMessage = code.ToString();
                    break;
            }

            return errorMessage;
        }

    }

    public class ApplicationDataException : APIBaseException
    {
        public ApplicationDataException(StatusCode code) : base(code, APIBaseException.MapError(code))
        {

        }

        public ApplicationDataException(StatusCode code, string message) : base(code, message)
        {

        }


    }

    public class ApplicationLogicException : APIBaseException
    {
        public ApplicationLogicException(StatusCode code) : base(code, APIBaseException.MapError(code))
        {

        }

        public ApplicationLogicException(StatusCode code, string message) : base(code, message)
        {

        }
    }

    public class UnhandledException : APIBaseException
    {
        public UnhandledException(string message, Exception innerException) : base(StatusCode.ERROR_Common, message, innerException)
        {

        }
    }

    public class UnauthorizedException : APIBaseException
    {
        public UnauthorizedException() : base(StatusCode.ERROR_Common)
        {

        }
    }

    public class UnAuthorizeAccessResourceException : APIBaseException
    {
        public UnAuthorizeAccessResourceException() : base(StatusCode.ERROR_Common)
        {

        }

        public UnAuthorizeAccessResourceException(string message) : base(StatusCode.ERROR_Common, message)
        {

        }
    }
}
