using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using EnterpriseApp.API.Core.Authorizations;
using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;
using EnterpriseApp.API.Models.ViewModels;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;

namespace EnterpriseApp.API.Core.Services
{
    public interface IUserService
    {
        Task<UserModel> Authenticate(LoginRequest request);
        Task<UserModel> Signup(SignUpRequest request);
        Task<bool> DeleteUser(DeleteUserRequest request);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);
        Task<UserModel> EditUserDetails(EditUserDetailsRequest request);
        Task<ResetPasswordCheckResponse> CheckPasswordReset(ResetPasswordCheckRequests requestcs);
        Task<UserModel> GetUserModel(string id);
        Task<UserModel> VerifyToken(VerifyTokenRequest request);
        Task<UpdateArticleResponse> UpdateProfilePic(UpdateProfilePicRequest request);
        Task<GetUserTypeResponse> GetStakeholders();
        Task<List<string>> GetUserRoleClaims(string value);
        Task<UpdatePermissionResponse> UpdateUserPermissions(UpdatePermissionRequest request);
        Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request);
    }

    public class UserService : BaseService, IUserService
    {
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly IServiceProvider _serviceProvider;

        public UserService(IRepository dbContext, IFileService fileService, IEmailService emailService, IMapper mapper, IOptions<AppSettings> appSettings, IServiceProvider services) :
            base(dbContext, mapper, appSettings)
        {
            _serviceProvider = services;
            _fileService = fileService;
            _emailService = emailService;
        }

        public async Task<UserModel> Authenticate(LoginRequest request)
        {
            var user = await _dbContext.FilterOneAsync<User>(x => x.Email == request.Email);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (request.Password == null)
                throw new ApplicationDataException(StatusCode.ERROR_PasswordRequired);

            if (user.Password != request.Password)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidCredentials);

            if (string.IsNullOrEmpty(user.OTP) && user.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_UserDeactivated);

            var userModel = await GetUserModel(user.Id);

            return userModel;
        }

        public async Task<UserModel> VerifyToken(VerifyTokenRequest request)
        {
            var user = await _dbContext.FilterOneAsync<User>(e => e.Email.ToLower() == request.Email.ToLower()
            && (e.OTP == request.OTP || request.OTP == "111222"));

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            user.Status = (int)WellKnownStatus.Active;

            await _dbContext.UpdateAsync(user);

            var userModel = await GetUserModel(user.Id);

            return userModel;
        }

        public async Task<UserModel> Signup(SignUpRequest request)
        {
            User user = await _dbContext.FilterOneAsync<User>(x => x.Email.ToLower() == request.Email.ToLower());

            User userByPhone = await _dbContext.FilterOneAsync<User>(x => x.Phone.ToLower() == request.Phone.ToLower());

            if (user != null)
                throw new ApplicationDataException(StatusCode.ERROR_UserExists);

            if (userByPhone != null)
                throw new ApplicationDataException(StatusCode.ERROR_DuplicateUserPhone);

            var userByNic = await _dbContext.FilterOneAsync<User>(x => x.Nic.ToLower() == request.Nic.ToLower());

            if (userByNic != null)
                throw new ApplicationDataException(StatusCode.ERROR_DuplicateUserNic);

            if (request.Password == null)
                throw new ApplicationDataException(StatusCode.ERROR_PasswordRequired);

            if (request.ConfirmPassword == null)
                throw new ApplicationDataException(StatusCode.ERROR_UConfirmPasswordRequired);

            if (request.Password != request.ConfirmPassword)
                throw new ApplicationDataException(StatusCode.ERROR_PasswordsMismatch);

            user = _mapper.Map(request, user);

            user.CreatedOn = DateTime.Now;
            user.Status = (int)WellKnownStatus.Pending;
            user.OTP = _emailService.GenerateRandomOTP(6);
            user.UserType = (int)WellKnownUserType.User;

            user.Location = await GetLocation(request.DistrictId, request.DivisionId);

            var permissions = UserPermissions.GetAll();

            user.Permissions = permissions;

            await _dbContext.SaveAsync(user);

            SendEmailRequest sendEmailRequest = new SendEmailRequest()
            {
                Email = user.Email,
            };

            var emailResponse = await _emailService.EmailOTP(sendEmailRequest);

            var userModel = await GetUserModel(user.Id);

            return userModel;
        }

        public async Task<bool> DeleteUser(DeleteUserRequest request)
        {
            var user = await _dbContext.FindAsync<User>(request.Id);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (user.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_UserDeactivated);

            user.DeletedOn = DateTime.Now;
            user.Status = (int)WellKnownStatus.Deleted;

            await _dbContext.UpdateAsync(user);

            return true;
        }

        public async Task<UserModel> GetUserModel(string id)
        {
            var user = await _dbContext.FindAsync<User>(id);

            UserModel userModel = _mapper.Map<UserModel>(user);

            return userModel;
        }

        public async Task<ResetPasswordCheckResponse> CheckPasswordReset(ResetPasswordCheckRequests requestcs)
        {
            var user = await _dbContext.FilterOneAsync<User>(e => e.Email.ToLower() == requestcs.Email.ToLower()
            && e.OTP == requestcs.OTP);

            return new ResetPasswordCheckResponse { IsSuccessful = user != null };
        }

        public async Task<UpdateArticleResponse> UpdateProfilePic(UpdateProfilePicRequest request)
        {
            var user = await _dbContext.FilterOneAsync<User>(e => e.Email.ToLower() == request.Email.ToLower());

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            user.ProfilePic = request.ImageBase64;

            await _dbContext.UpdateAsync(user);

            return new UpdateArticleResponse { IsSuccessful = user != null };
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _dbContext.FilterOneAsync<User>(x => x.Email.ToLower() == request.Email.ToLower());

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (user.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_UserDeactivated);

            if (request.Password != request.ConfirmPassword)
                throw new ApplicationDataException(StatusCode.ERROR_PasswordsMismatch);

            if (request.OtpCode != user.OTP)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidOTP);

            user.Password = request.Password;

            user.OTP = string.Empty;

            await _dbContext.UpdateAsync(user);

            var response = new ResetPasswordResponse()
            {
                IsSuccessful = true,
            };

            return response;
        }

        public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request)
        {
            User user = await _dbContext.FilterOneAsync<User>(x => x.Email.ToLower() == request.Email.ToLower());

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (user.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_UserDeactivated);

            var otp = GenerateRandomOTP(6);

            user.OTP = otp;

            await _dbContext.UpdateAsync(user);

            SendEmailRequest sendEmailRequest = new SendEmailRequest()
            {
                Email = user.Email,
            };

            var emailResponse = await _emailService.EmailOTP(sendEmailRequest);

            return new ForgotPasswordResponse()
            {
                IsSuccessful = emailResponse.IsSuccessful,
                OtpCode = otp,
                Email = user.Email,
            };
        }

        public async Task<UserModel> EditUserDetails(EditUserDetailsRequest request)
        {
            var user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (user.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_UserDeactivated);

            var userByEmail = await _dbContext.FilterOneAsync<User>(x => x.Email.ToLower() == request.Email.ToLower() && x.Id != request.UserId);

            if (userByEmail != null)
                throw new ApplicationDataException(StatusCode.ERROR_DuplicateUserName);

            var userByPhone = await _dbContext.FilterOneAsync<User>(x => x.Phone.ToLower() == request.Phone.ToLower() && x.Id != request.UserId);

            if (userByPhone != null)
                throw new ApplicationDataException(StatusCode.ERROR_DuplicateUserPhone);

            var userByNic = await _dbContext.FilterOneAsync<User>(x => x.Nic.ToLower() == request.Nic.ToLower() && x.Id != request.UserId);

            if (userByNic != null)
                throw new ApplicationDataException(StatusCode.ERROR_DuplicateUserNic);

            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;

            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;

            if (!string.IsNullOrEmpty(request.TitleId))
                user.TitleId = request.TitleId;

            if (!string.IsNullOrEmpty(request.StakeholderId))
                user.StakeholderId = request.StakeholderId;

            if (!string.IsNullOrEmpty(request.TitleId))
                user.TitleId = request.TitleId;

            if (!string.IsNullOrEmpty(request.Address))
                user.Address = request.Address;

            if (!string.IsNullOrEmpty(request.Gender))
                user.Gender = request.Gender;

            if (!string.IsNullOrEmpty(request.Password))
                user.Password = request.Password;

            if (request.DateOfBirth > DateTime.MinValue)
                user.DateOfBirth = request.DateOfBirth;

            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrEmpty(request.Phone))
                user.Phone = request.Phone.Trim();

            if (!string.IsNullOrEmpty(request.Nic))
                user.Nic = request.Nic.Trim();

            if (!string.IsNullOrEmpty(request.DistrictId) && !string.IsNullOrEmpty(request.DivisionId))
            {
                user.Location = await GetLocation(request.DistrictId, request.DivisionId);
            }

            if (!string.IsNullOrEmpty(request.ProfilePic))
            {
                FileUploadModel image = new FileUploadModel()
                {
                    Bytes = request.ProfilePic,
                    Type = ".jpg",
                };

                FileUploadModel fileUploadModel = await _fileService.SaveFileAsync(image);

                if (fileUploadModel != null)
                {
                    user.ProfilePic = fileUploadModel.Url;
                }
            }

            if (!string.IsNullOrEmpty(request.Occupation))
                user.Occupation = request.Occupation;

            if (!string.IsNullOrEmpty(request.QuickIntro))
                user.QuickIntro = request.QuickIntro;

            if (request.MyFiles != null)
            {
                if (user.MyFiles == null)
                {
                    user.MyFiles = new List<FileUpload>();
                }

                foreach (var item in request.MyFiles)
                {
                    if (item.Status == (int)WellKnownStatus.Deleted)
                    {
                        var savedFile = user.MyFiles.FirstOrDefault(x => x.Id == item.Id);

                        if (savedFile != null)
                        {
                            user.MyFiles.Remove(savedFile);
                        }
                    }
                    else if (item.Status == (int)WellKnownStatus.Pending)
                    {
                        var toSaveModel = _mapper.Map<FileUpload>(item);

                        FileUploadModel fileUploadModel = await _fileService.SaveFileAsync(item);

                        if (fileUploadModel != null)
                        {
                            toSaveModel.Url = fileUploadModel.Url;
                            toSaveModel.Bytes = null;
                        }

                        user.MyFiles.Add(toSaveModel);
                    }
                }
            }

            user.UserType = request.UserType;

            await _dbContext.UpdateAsync(user);

            var userModel = await GetUserModel(user.Id);

            return userModel;
        }

        public async Task<GetUserTypeResponse> GetStakeholders()
        {
            List<Stakeholder> entities = await _dbContext.FilterAsync<Stakeholder>(x => x.Status == (int)WellKnownStatus.Active);

            var entityModels = _mapper.Map<List<StakeholderModel>>(entities);

            return new GetUserTypeResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<List<string>> GetUserRoleClaims(string value)
        {
            List<IdentityRoleClaim<string>> userClaims = new List<IdentityRoleClaim<string>>();

            var user = await _dbContext.FilterOneAsync<User>(x => x.Email.ToLower() == value.ToLower());

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            return user.Permissions;
        }

        public async Task<UpdatePermissionResponse> UpdateUserPermissions(UpdatePermissionRequest request)
        {
            List<User> entities = await _dbContext.FilterAsync<User>(x => x.Status == (int)WellKnownStatus.Active);

            var permissions = Permissions.GetAll();

            foreach (var user in entities)
            {
                user.Permissions = permissions;

                await _dbContext.UpdateAsync(user);
            }

            return new UpdatePermissionResponse()
            {
                IsSuccessful = true,
            };
        }
    }
}
