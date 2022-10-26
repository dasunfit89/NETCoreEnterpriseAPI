using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnterpriseApp.API.Core.Authorizations;
using EnterpriseApp.API.Core.Constants;
using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Core.Extensions;
using EnterpriseApp.API.Core.Helpers;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;
using GoogleApi.Entities.Search.Common;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;

namespace EnterpriseApp.API.Core.Services
{
    public interface IApplicationDataService
    {
        Task<GetArticleCategoriesResponse> GetArticleCategories(GetArticleCategoriesRequest request);
        Task<bool> UploadLocationData(List<DivisionExcelItem> items);
        Task<GetAreaListResponse> GetAreas(GetAreasRequest request);
        Task<ArticleModel> GetArticle(GetArticleRequest request);
        Task<GetInformationCategoriesResponse> GetInformationCategories(int type);
        Task<PublicSentNewsAddUpdateResponse> AddPublicSentNews(PublicSentNewsInsertModel request);
        Task<GetNewChatResponse> GetNewChat(GetNewChatRequest request);
        Task<GetPublicSentNewsResponse> GetPublicSentNews(GetArticleListRequest request);
        Task<PublicSentInformationAddUpdateResponse> AddPublicSentInformation(PublicSentInformationInsertModel request);
        Task<SendChatResponse> SendChat(ChatInsertModel model);
        Task<GetPublicSentInformationResponse> GetPublicSentInformation(GetArticleListRequest request);
        Task<PublicSentPartyInformationAddUpdateResponse> AddPublicSentPartyInformation(PublicSentPartyInformationInsertModel request);
        Task<GetPublicSentPartyNewsResponse> GetPublicSentPartyInformation(GetArticleListRequest request);
        Task<UpdateChatResponse> UpdateChatDeliveryStatus(UpdateChatRequest request);
        Task<PublicMessageAddUpdateResponse> AddPublicMessage(PublicMessageInsertModel request);
        Task<GetPublicMessagesResponse> GetPublicMessages(GetPublicMessageRequest request);
        Task<GetDesignationCategoryResponse> GetDesignationCategories();
        Task<UpdateDeviceTokenResponse> UpdateDeviceToken(UpdateDeviceTokenRequest request);
        Task<GetPartyDesignationResponse> GetPartyDesignations(GetArticleListRequest request);
        Task<PartyDesignationAddUpdateResponse> AddPartyDesignation(PartyDesignationInsertModel model);
        Task<ChangePostStatusResponse> ChangePostStatus(ChangePostStatusRequest request);
        Task<GetAppUsersResponse> GetAdminUsers(GetAppUsersRequest request);
        Task<GetAppUsersResponse> GetAppUsers(GetAppUsersRequest request);
        Task<UpdateAppUserResponse> UpdateAppUser(UpdateAppUserRequest request);
        Task<AddUserCommentResponse> AddUserComment(UserCommentInsertModel request);
        Task<PartyDesignationAddUpdateResponse> UpdatePartyDesignation(PartyDesignationUpdateModel request);
        Task<PublicMessageAddUpdateResponse> UpdatePublicMessage(PublicMessageUpdateModel request);
        Task<PublicSentNewsAddUpdateResponse> UpdatePublicSentNews(PublicSentNewsUpdateModel request);
        Task<PublicSentPartyInformationAddUpdateResponse> UpdatePublicSentPartyInformation(PublicSentPartyInformationUpdateModel request);
        Task<PublicSentInformationAddUpdateResponse> UpdatePublicSentInformation(PublicSentInformationUpdateModel request);
        Task<ConfigUpdateResponse> UpdateConfig(ConfigUpdateRequest request);
        Task<GetConfigurationResponse> GetConfiguration(GetConfigurationRequest request);
    }

    public class ApplicationDataService : BaseService, IApplicationDataService
    {
        protected IFileService _fileService;

        public ApplicationDataService(IRepository dbContext, IMapper mapper, IFileService fileService, IOptions<AppSettings> appSettings) :
            base(dbContext, mapper, appSettings)
        {
            _fileService = fileService;
        }

        public async Task<GetArticleCategoriesResponse> GetArticleCategories(GetArticleCategoriesRequest request)
        {
            List<CategoryModel> entityModels = new List<CategoryModel>();

            List<Category> entities = await _dbContext.FilterAsync<Category>(x => x.Status == (int)WellKnownStatus.Active);

            entityModels = _mapper.Map<List<CategoryModel>>(entities);

            GetArticleCategoriesResponse response = new GetArticleCategoriesResponse()
            {
                CategoryList = entityModels,
            };

            return response;
        }

        public async Task<bool> UploadLocationData(List<DivisionExcelItem> items)
        {
            string province = items.First().Province;

            Area area = await _dbContext.FilterOneAsync<Area>(x => x.Name.ToLower() == province.ToLower());

            if (area == null)
            {
                area = new Area()
                {
                    Name = province,
                };

                await _dbContext.SaveAsync(area);

                area = await _dbContext.FindAsync<Area>(area.Id);
            }

            foreach (var item in items)
            {
                District district = area.Districts.FirstOrDefault(x => x.Name == item.District);

                if (district == null)
                {
                    district = new District()
                    {
                        Name = item.District,
                    };

                    area.Districts.Add(district);
                }

                Division division = district.Divisions.FirstOrDefault(x => x.Name == item.Divisional_Secretariat);

                if (division == null)
                {
                    division = new Division()
                    {
                        Name = item.Divisional_Secretariat,
                    };

                    district.Divisions.Add(division);
                }

                Village village = division.Villages.FirstOrDefault(x => x.GN_Code == item.GN_Code);

                if (village == null)
                {
                    village = new Village()
                    {
                        GN_Code = item.GN_Code,
                        Name_In_Sinhala = item.Name_In_Sinhala,
                        Name_In_English = item.Name_In_English,
                        Name_In_Tamil = item.Name_In_Tamil,
                    };

                    division.Villages.Add(village);
                }
            }

            await _dbContext.UpdateAsync(area);

            return true;
        }

        public async Task<GetAreaListResponse> GetAreas(GetAreasRequest request)
        {
            List<AreaModel> entityModels = new List<AreaModel>();

            List<Area> entities = await _dbContext.FilterAsync<Area>(x => x.Status == (int)WellKnownStatus.Active);

            entityModels = _mapper.Map<List<AreaModel>>(entities);

            GetAreaListResponse response = new GetAreaListResponse()
            {
                AreaList = entityModels,
            };

            return response;
        }

        public async Task<ArticleModel> GetArticle(GetArticleRequest request)
        {
            var entity = await _dbContext.FilterOneAsync<Article>(r => r.Id == request.ArticleId);

            if (entity == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

            var entityModel = _mapper.Map<ArticleModel>(entity);

            var category = await _dbContext.FilterOneAsync<Category>(r => r.Id == entity.CategoryId);
            var categoryModel = _mapper.Map<CategoryModel>(category);
            entityModel.Category = categoryModel;

            var user = await _dbContext.FilterOneAsync<User>(r => r.Id == entity.UserId);
            var userModel = _mapper.Map<UserModel>(user);
            entityModel.Reporter = userModel;

            return entityModel;
        }

        public async Task<GetInformationCategoriesResponse> GetInformationCategories(int type)
        {
            List<InformationCategory> entities = await _dbContext.FilterAsync<InformationCategory>(x => x.Status == (int)WellKnownStatus.Active && x.Type == type);

            var entityModels = _mapper.Map<List<InformationCategoryModel>>(entities);

            return new GetInformationCategoriesResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<PublicSentNewsAddUpdateResponse> AddPublicSentNews(PublicSentNewsInsertModel request)
        {
            var user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            var category = await _dbContext.FindAsync<InformationCategory>(request.CategoryId);

            if (category == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidCategory);

            foreach (var image in request.Images)
            {
                FileUploadModel fileUploadModel = await _fileService.SaveFileAsync(image);

                if (fileUploadModel != null)
                {
                    image.Url = fileUploadModel.Url;
                    image.Bytes = null;
                }
            }

            var entity = _mapper.Map<PublicSentNews>(request);

            entity.ThumbUrl = entity.Images.FirstOrDefault()?.Url;

            entity.Location = await GetLocation(entity.DistrictId, entity.DivisionId);
            entity.Reporter = user;

            bool result = await _dbContext.SaveAsync<PublicSentNews>(entity);

            var insertedModel = _mapper.Map<PublicSentNewsModel>(entity);

            return new PublicSentNewsAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<GetPublicSentNewsResponse> GetPublicSentNews(GetArticleListRequest request)
        {
            List<PublicSentNews> newsEntities = null;

            List<PublicSentNewsModel> newsModels = null;

            List<PublicMessageModel> messsageModels = new List<PublicMessageModel>();

            if (request.IsAdmin)
            {
                newsEntities = await _dbContext.FilterAsync<PublicSentNews>(x => request.Status == 0 || x.Status == request.Status);

                newsEntities = newsEntities.OrderBy(x => x.Status).ToList();

                newsModels = _mapper.Map<List<PublicSentNewsModel>>(newsEntities).OrderByDescending(x => x.CreatedOn).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(request.UserId))
                {
                    //my reports
                    newsEntities = await _dbContext.FilterAsync<PublicSentNews>(x => x.Status == (int)WellKnownStatus.Active || (x.Status == (int)WellKnownStatus.Pending) &&
                                                                                          x.UserId == request.UserId);

                }
                else
                {
                    newsEntities = await _dbContext.FilterAsync<PublicSentNews>(x => x.Status == (int)WellKnownStatus.Active &&
                                                                                        (
                                                                                            string.IsNullOrEmpty(request.DistrictId) ||
                                                                                            string.IsNullOrEmpty(x.DistrictId) ||
                                                                                            x.DistrictId == request.DistrictId
                                                                                        ));
                }

                newsModels = _mapper.Map<List<PublicSentNewsModel>>(newsEntities).OrderByDescending(x => x.CreatedOn).ToList();

                if (string.IsNullOrEmpty(request.UserId) && false)
                {
                    var categories = await _dbContext.FilterAsync<InformationCategory>(x => x.NameSi == "පුවත්/News");

                    var newsCategory = categories.FirstOrDefault();

                    List<PublicMessage> messageEntities = await _dbContext.FilterAsync<PublicMessage>(x => x.CategoryId == newsCategory.Id && x.Status == (int)WellKnownStatus.Active);

                    messsageModels = _mapper.Map<List<PublicMessageModel>>(messageEntities).OrderByDescending(x => x.CreatedOn).ToList();
                }
            }

            return new GetPublicSentNewsResponse()
            {
                Items = newsModels,
                MessageItems = messsageModels,
            };
        }

        public async Task<PublicSentInformationAddUpdateResponse> AddPublicSentInformation(PublicSentInformationInsertModel request)
        {
            var user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            var category = await _dbContext.FindAsync<InformationCategory>(request.CategoryId);

            if (category == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidCategory);

            foreach (var image in request.Images)
            {
                FileUploadModel fileUploadModel = await _fileService.SaveFileAsync(image);

                if (fileUploadModel != null)
                {
                    image.Url = fileUploadModel.Url;
                    image.Bytes = null;
                }
            }

            var entity = _mapper.Map<PublicSentInformation>(request);

            entity.ThumbUrl = entity.Images.FirstOrDefault()?.Url;

            entity.Location = await GetLocation(entity.DistrictId, entity.DivisionId);
            entity.Reporter = user;

            bool result = await _dbContext.SaveAsync<PublicSentInformation>(entity);

            var insertedModel = _mapper.Map<PublicSentInformationModel>(entity);

            return new PublicSentInformationAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<GetPublicSentInformationResponse> GetPublicSentInformation(GetArticleListRequest request)
        {
            List<PublicSentInformation> entities = null;

            if (request.IsAdmin)
            {
                entities = await _dbContext.FilterAsync<PublicSentInformation>(x => request.Status == 0 || x.Status == request.Status);

                entities = entities.OrderBy(x => x.Status).ToList();
            }
            else
            {
                entities = await _dbContext.FilterAsync<PublicSentInformation>(x => x.Status == (int)WellKnownStatus.Active || (x.Status == (int)WellKnownStatus.Pending) &&
                                                                                          x.UserId == request.UserId);
            }

            var entityModels = _mapper.Map<List<PublicSentInformationModel>>(entities).OrderByDescending(x => x.CreatedOn).ToList();

            return new GetPublicSentInformationResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<PublicSentPartyInformationAddUpdateResponse> AddPublicSentPartyInformation(PublicSentPartyInformationInsertModel request)
        {
            var user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            var category = await _dbContext.FindAsync<InformationCategory>(request.CategoryId);

            if (category == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidCategory);

            foreach (var image in request.Images)
            {
                FileUploadModel fileUploadModel = await _fileService.SaveFileAsync(image);

                if (fileUploadModel != null)
                {
                    image.Url = fileUploadModel.Url;
                    image.Bytes = null;
                }
            }

            var entity = _mapper.Map<PublicSentPartyInformation>(request);

            entity.ThumbUrl = entity.Images.FirstOrDefault()?.Url;

            entity.Location = await GetLocation(entity.DistrictId, entity.DivisionId);
            entity.Reporter = user;

            bool result = await _dbContext.SaveAsync<PublicSentPartyInformation>(entity);

            var insertedModel = _mapper.Map<PublicSentPartyInformationModel>(entity);

            return new PublicSentPartyInformationAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<GetPublicSentPartyNewsResponse> GetPublicSentPartyInformation(GetArticleListRequest request)
        {
            List<PublicSentPartyInformation> entities = null;

            if (request.IsAdmin)
            {
                entities = await _dbContext.FilterAsync<PublicSentPartyInformation>(x => request.Status == 0 || x.Status == request.Status);

                entities = entities.OrderBy(x => x.Status).ToList();
            }
            else
            {
                entities = await _dbContext.FilterAsync<PublicSentPartyInformation>(x => x.Status == (int)WellKnownStatus.Active || (x.Status == (int)WellKnownStatus.Pending) &&
                                                                                          x.UserId == request.UserId);
            }

            var entityModels = _mapper.Map<List<PublicSentPartyInformationModel>>(entities).OrderByDescending(x => x.CreatedOn).ToList();

            return new GetPublicSentPartyNewsResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<GetNewChatResponse> GetNewChat(GetNewChatRequest request)
        {
            DateTime lastMessageTime = DateTime.MinValue;

            if (!string.IsNullOrEmpty(request.LastMessageId))
            {
                var lastMessage = await _dbContext.FindAsync<Chat>(request.LastMessageId);

                if (lastMessage != null)
                {
                    lastMessageTime = lastMessage.SendDate;
                }
            }

            List<Chat> entities = await _dbContext.FilterAsync<Chat>(x => x.ReceiverId == request.UserId &&
                                                                    x.ChatType == request.ChatType &&
                                                                    x.SendDate > lastMessageTime &&
                                                                    x.Status == (int)WellKnownStatus.Active);

            var entityModels = _mapper.Map<List<ChatModel>>(entities);

            return new GetNewChatResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<SendChatResponse> SendChat(ChatInsertModel request)
        {
            var sender = await _dbContext.FindAsync<User>(request.SenderId);

            if (sender == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (string.IsNullOrEmpty(request.Text) && request.Attachments.IsEmpty())
            {
                throw new ApplicationDataException(StatusCode.ERROR_InvalidData, "Invalid chat message content.");
            }

            foreach (var image in request.Attachments)
            {
                FileUploadModel fileUploadModel = await _fileService.SaveFileAsync(image);

                if (fileUploadModel != null)
                {
                    image.Url = fileUploadModel.Url;
                    image.Bytes = null;
                }
            }

            var entity = _mapper.Map<Chat>(request);

            entity.ThumbUrl = entity.Attachments.FirstOrDefault()?.Url;

            entity.DeliveryStatus = (int)WellknownChatDeliveryStatus.RECEIVED;

            bool result = await _dbContext.SaveAsync<Chat>(entity);

            var insertedModel = _mapper.Map<ChatModel>(entity);

            List<UserDevice> devices = await GetDevices(request.ReceiverId);

            var pushRequest = new PushNotificationRequest()
            {
                Devices = devices,
                Title = "SLFP App",
                Body = "You have new messages",
                MessageType = PushNotificationKeys.CHAT_NOTIFICATION,
                MessageParam = string.Empty,
            };

            bool pushResult = await PushHelper.SendPush(pushRequest);

            return new SendChatResponse()
            {
                Item = insertedModel,
            };
        }

        private async Task<List<UserDevice>> GetDevices(string userId)
        {
            List<UserDevice> devices = await _dbContext.FilterAsync<UserDevice>(x => x.Status == (int)WellKnownStatus.Active && (string.IsNullOrEmpty(userId) || x.UserId == userId));

            return devices;
        }

        public async Task<UpdateChatResponse> UpdateChatDeliveryStatus(UpdateChatRequest request)
        {
            var user = await _dbContext.FindAsync<User>(request.SenderId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            List<Chat> chatEntities = await _dbContext.FilterAsync<Chat>(x => request.MessageIds.Any(y => x.Id == y));

            foreach (var entity in chatEntities)
            {
                entity.DeliveryStatus = request.Status;
                bool result = await _dbContext.SaveAsync<Chat>(entity);
            }

            return new UpdateChatResponse()
            {
                IsSuccessful = true,
            };
        }

        public async Task<PublicMessageAddUpdateResponse> AddPublicMessage(PublicMessageInsertModel request)
        {
            var user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            var category = await _dbContext.FindAsync<InformationCategory>(request.CategoryId);

            if (category == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidCategory);

            foreach (var image in request.Images)
            {
                FileUploadModel fileUploadModel = await _fileService.SaveFileAsync(image);

                if (fileUploadModel != null)
                {
                    image.Url = fileUploadModel.Url;
                    image.Bytes = null;
                }
            }

            var entity = _mapper.Map<PublicMessage>(request);

            entity.ThumbUrl = entity.Images.FirstOrDefault()?.Url;

            entity.Location = await GetLocation(entity.DistrictId, entity.DivisionId);
            entity.Reporter = user;

            bool result = await _dbContext.SaveAsync<PublicMessage>(entity);

            var insertedModel = _mapper.Map<PublicMessageModel>(entity);

            List<UserDevice> devices = await GetDevices(string.Empty);

            if (!string.IsNullOrEmpty(request.DistrictId))
            {
                devices = devices.FindAll(x => x.Location?.DistrictId == request.DistrictId);
            }

            if (!string.IsNullOrEmpty(request.DivisionId))
            {
                devices = devices.FindAll(x => x.Location?.DivisionId == request.DivisionId);
            }

            var pushRequest = new PushNotificationRequest()
            {
                Devices = devices,
                Title = "SLFP App",
                Body = "You have new messages",
                MessageType = PushNotificationKeys.PUBLIC_NOTIFICATION,
                MessageParam = string.Empty,
            };

            bool pushResult = await PushHelper.SendPush(pushRequest);

            return new PublicMessageAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<GetPublicMessagesResponse> GetPublicMessages(GetPublicMessageRequest request)
        {
            string categoryId = null;

            if (request.MessageType > 0)
            {
                string categoryName = request.MessageType == 1 ? "පුවත්/News" : "විශේෂ නිවේදන/Special Announcements";

                var categories = await _dbContext.FilterAsync<InformationCategory>(x => x.NameSi == categoryName);

                var newsCategory = categories.FirstOrDefault();

                categoryId = newsCategory?.Id;
            }

            List<PublicMessage> entities = await _dbContext.FilterAsync<PublicMessage>(x => x.Status == (int)WellKnownStatus.Active
                                                                                            && (string.IsNullOrEmpty(categoryId) || x.CategoryId == categoryId)
                                                                                            && (request.DistrictId == null || string.IsNullOrEmpty(x.DistrictId) || x.DistrictId == request.DistrictId)
                                                                                            );

            var entityModels = _mapper.Map<List<PublicMessageModel>>(entities).OrderByDescending(x => x.CreatedOn).ToList();

            return new GetPublicMessagesResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<UpdateDeviceTokenResponse> UpdateDeviceToken(UpdateDeviceTokenRequest request)
        {
            var user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            List<UserDevice> entities = await _dbContext.FilterAsync<UserDevice>(x => x.UserId == request.UserId &&
                                                                  x.DeviceId == request.DeviceId);
            UserDevice entity = entities.FirstOrDefault();

            if (entity == null)
            {
                entity = _mapper.Map<UserDevice>(request);
                entity.Location = user.Location;
                bool result = await _dbContext.SaveAsync<UserDevice>(entity);
            }
            else
            {
                entity.PushToken = request.PushToken;
                entity.UpdatedOn = DateTime.Now;
                entity.Location = user.Location;
                await _dbContext.UpdateAsync<UserDevice>(entity);
            }

            return new UpdateDeviceTokenResponse()
            {
                IsSaved = true,
            };
        }

        public async Task<GetDesignationCategoryResponse> GetDesignationCategories()
        {
            List<DesignationCategory> entities = await _dbContext.FilterAsync<DesignationCategory>(x => x.Status == (int)WellKnownStatus.Active);

            var entityModels = _mapper.Map<List<DesignationCategoryModel>>(entities).OrderBy(x => x.Order).ToList();

            return new GetDesignationCategoryResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<PartyDesignationAddUpdateResponse> AddPartyDesignation(PartyDesignationInsertModel request)
        {
            var designation = await _dbContext.FindAsync<DesignationCategory>(request.DesignationId);

            if (designation == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidCategory);

            foreach (var image in request.Images)
            {
                FileUploadModel fileUploadModel = await _fileService.SaveFileAsync(image);

                if (fileUploadModel != null)
                {
                    image.Url = fileUploadModel.Url;
                    image.Bytes = null;
                }
            }

            var entity = _mapper.Map<PartyDesignation>(request);

            entity.ThumbUrl = entity.Images.FirstOrDefault()?.Url;

            entity.Order = int.MaxValue;

            bool result = await _dbContext.SaveAsync<PartyDesignation>(entity);

            var insertedModel = _mapper.Map<PartyDesignationModel>(entity);

            return new PartyDesignationAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<GetPartyDesignationResponse> GetPartyDesignations(GetArticleListRequest request)
        {
            List<PartyDesignation> entities = await _dbContext.FilterAsync<PartyDesignation>(x => x.Status == (int)WellKnownStatus.Active);

            List<DesignationCategory> designationCategories = await _dbContext.GetAllAsync<DesignationCategory>();

            var entityModels = _mapper.Map<List<PartyDesignationModel>>(entities).OrderBy(x => x.Order).ToList();

            var designationModels = _mapper.Map<List<DesignationCategoryModel>>(designationCategories);

            foreach (var entityModel in entityModels)
            {
                DesignationCategoryModel designationCategory = designationModels.FirstOrDefault(x => x.Id == entityModel.DesignationId);

                if (designationCategory != null)
                {
                    entityModel.Designation = designationCategory;
                }
            }

            return new GetPartyDesignationResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<ChangePostStatusResponse> ChangePostStatus(ChangePostStatusRequest request)
        {
            if (request.PostType == nameof(PublicSentInformation))
            {
                var entity = await _dbContext.FindAsync<PublicSentInformation>(request.Id);

                if (entity == null)
                    throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

                entity.Status = request.Status;

                var user = await _dbContext.FindAsync<User>(entity.UserId);

                var updated = await _dbContext.UpdateAsync<PublicSentInformation>(entity);

            }
            else if (request.PostType == nameof(PublicSentNews))
            {
                var entity = await _dbContext.FindAsync<PublicSentNews>(request.Id);

                if (entity == null)
                    throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

                entity.Status = request.Status;

                var updated = await _dbContext.UpdateAsync<PublicSentNews>(entity);
            }
            else if (request.PostType == nameof(PublicSentPartyInformation))
            {
                var entity = await _dbContext.FindAsync<PublicSentPartyInformation>(request.Id);

                if (entity == null)
                    throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

                entity.Status = request.Status;

                var updated = await _dbContext.UpdateAsync<PublicSentPartyInformation>(entity);
            }
            else if (request.PostType == nameof(PartyDesignation))
            {
                var entity = await _dbContext.FindAsync<PartyDesignation>(request.Id);

                if (entity == null)
                    throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

                entity.Status = request.Status;

                var updated = await _dbContext.UpdateAsync<PartyDesignation>(entity);
            }
            else if (request.PostType == nameof(PublicMessage))
            {
                var entity = await _dbContext.FindAsync<PublicMessage>(request.Id);

                if (entity == null)
                    throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

                entity.Status = request.Status;

                var updated = await _dbContext.UpdateAsync<PublicMessage>(entity);
            }
            else
            {
                throw new ApplicationDataException(StatusCode.ERROR_InvalidData);
            }

            return new ChangePostStatusResponse()
            {
                IsSuccessful = true,
            };
        }

        public async Task<GetAppUsersResponse> GetAdminUsers(GetAppUsersRequest request)
        {
            List<User> entities = await _dbContext.FilterAsync<User>(x => x.Status != (int)WellKnownStatus.Deleted &&
            !x.Permissions.Any(y => y == Permissions.SuperUser) && x.UserType == (int)WellKnownUserType.Admin);

            var entityModels = _mapper.Map<List<LightUserModel>>(entities).OrderBy(x => x.UserType).ToList();

            return new GetAppUsersResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<GetAppUsersResponse> GetAppUsers(GetAppUsersRequest request)
        {
            List<User> entities = await _dbContext.FilterAsync<User>(x => x.Status != (int)WellKnownStatus.Deleted && x.UserType == (int)WellKnownUserType.User);

            var entityModels = _mapper.Map<List<LightUserModel>>(entities).OrderBy(x => x.UserType).ToList();

            return new GetAppUsersResponse()
            {
                Items = entityModels,
            };
        }

        public async Task<UpdateAppUserResponse> UpdateAppUser(UpdateAppUserRequest request)
        {
            User entity = await _dbContext.FindAsync<User>(request.UserId);

            if (entity == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (request.Status > 0)
            {
                entity.Status = request.Status;
            }

            if (request.Permissions != null && !request.Permissions.IsEmpty())
            {
                entity.Permissions = request.Permissions;
            }

            if (request.UserType > 0)
            {
                entity.UserType = request.UserType;
            }

            var updated = await _dbContext.UpdateAsync<User>(entity);

            return new UpdateAppUserResponse()
            {
                IsSuccessful = true,
            };
        }

        public async Task<AddUserCommentResponse> AddUserComment(UserCommentInsertModel request)
        {
            User user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            var userName = $"{user.FirstName} {user.LastName}";

            UserComment entity = null;

            if (request.ArticleType == nameof(PublicSentInformation))
            {
                PublicSentInformation parentEntity = await _dbContext.FindAsync<PublicSentInformation>(request.ArticleId);

                if (parentEntity == null)
                    throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

                if (string.IsNullOrEmpty(request.Id))
                {
                    entity = _mapper.Map<UserComment>(request);

                    entity.UserName = userName;

                    parentEntity.UserComments.Add(entity);
                }
                else
                {
                    entity = parentEntity.UserComments.FirstOrDefault(x => x.Id == request.Id);

                    parentEntity.UserComments.Remove(entity);
                }

                var updated = await _dbContext.UpdateAsync<PublicSentInformation>(parentEntity);
            }
            else if (request.ArticleType == nameof(PublicSentNews))
            {
                PublicSentNews parentEntity = await _dbContext.FindAsync<PublicSentNews>(request.ArticleId);

                if (parentEntity == null)
                    throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

                if (string.IsNullOrEmpty(request.Id))
                {
                    entity = _mapper.Map<UserComment>(request);

                    entity.UserName = userName;

                    parentEntity.UserComments.Add(entity);
                }
                else
                {
                    entity = parentEntity.UserComments.FirstOrDefault(x => x.Id == request.Id);

                    parentEntity.UserComments.Remove(entity);
                }

                var updated = await _dbContext.UpdateAsync<PublicSentNews>(parentEntity);
            }
            else if (request.ArticleType == nameof(PublicSentPartyInformation))
            {
                PublicSentPartyInformation parentEntity = await _dbContext.FindAsync<PublicSentPartyInformation>(request.ArticleId);

                if (parentEntity == null)
                    throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

                if (string.IsNullOrEmpty(request.Id))
                {
                    entity = _mapper.Map<UserComment>(request);

                    entity.UserName = userName;

                    parentEntity.UserComments.Add(entity);
                }
                else
                {
                    entity = parentEntity.UserComments.FirstOrDefault(x => x.Id == request.Id);

                    parentEntity.UserComments.Remove(entity);
                }

                var updated = await _dbContext.UpdateAsync<PublicSentPartyInformation>(parentEntity);
            }
            else if (request.ArticleType == nameof(PublicMessage))
            {
                PublicMessage parentEntity = await _dbContext.FindAsync<PublicMessage>(request.ArticleId);

                if (parentEntity == null)
                    throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

                if (string.IsNullOrEmpty(request.Id))
                {
                    entity = _mapper.Map<UserComment>(request);

                    entity.UserName = userName;

                    parentEntity.UserComments.Add(entity);
                }
                else
                {
                    entity = parentEntity.UserComments.FirstOrDefault(x => x.Id == request.Id);

                    parentEntity.UserComments.Remove(entity);
                }

                var updated = await _dbContext.UpdateAsync<PublicMessage>(parentEntity);
            }

            var insertedModel = _mapper.Map<UserCommentModel>(entity);

            return new AddUserCommentResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<PartyDesignationAddUpdateResponse> UpdatePartyDesignation(PartyDesignationUpdateModel request)
        {
            PartyDesignation entity = await _dbContext.FindAsync<PartyDesignation>(request.Id);

            if (entity == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

            entity = _mapper.Map<PartyDesignationUpdateModel, PartyDesignation>(request, entity);

            if (request.Images != null)
            {
                if (entity.Images == null)
                {
                    entity.Images = new List<FileUpload>();
                }

                foreach (var item in request.Images)
                {
                    if (item.Status == (int)WellKnownStatus.Deleted)
                    {
                        var savedFile = entity.Images.FirstOrDefault(x => x.Id == item.Id);

                        if (savedFile != null)
                        {
                            entity.Images.Remove(savedFile);
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

                        entity.Images.Add(toSaveModel);
                    }
                }
            }

            var updated = await _dbContext.UpdateAsync(entity);

            var insertedModel = _mapper.Map<PartyDesignationModel>(updated);

            return new PartyDesignationAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<PublicMessageAddUpdateResponse> UpdatePublicMessage(PublicMessageUpdateModel request)
        {
            PublicMessage entity = await _dbContext.FindAsync<PublicMessage>(request.Id);

            if (entity == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

            User user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            entity = _mapper.Map<PublicMessageUpdateModel, PublicMessage>(request, entity);

            if (request.Images != null)
            {
                if (entity.Images == null)
                {
                    entity.Images = new List<FileUpload>();
                }

                foreach (var item in request.Images)
                {
                    if (item.Status == (int)WellKnownStatus.Deleted)
                    {
                        var savedFile = entity.Images.FirstOrDefault(x => x.Id == item.Id);

                        if (savedFile != null)
                        {
                            entity.Images.Remove(savedFile);
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

                        entity.Images.Add(toSaveModel);
                    }
                }
            }

            entity.ThumbUrl = entity.Images.FirstOrDefault()?.Url;

            entity.Location = await GetLocation(entity.DistrictId, entity.DivisionId);

            entity.Reporter = user;

            var updated = await _dbContext.UpdateAsync(entity);

            var insertedModel = _mapper.Map<PublicMessageModel>(updated);

            return new PublicMessageAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<PublicSentNewsAddUpdateResponse> UpdatePublicSentNews(PublicSentNewsUpdateModel request)
        {
            PublicSentNews entity = await _dbContext.FindAsync<PublicSentNews>(request.Id);

            if (entity == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

            User user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            entity = _mapper.Map<PublicSentNewsUpdateModel, PublicSentNews>(request, entity);

            if (request.Images != null)
            {
                if (entity.Images == null)
                {
                    entity.Images = new List<FileUpload>();
                }

                foreach (var item in request.Images)
                {
                    if (item.Status == (int)WellKnownStatus.Deleted)
                    {
                        var savedFile = entity.Images.FirstOrDefault(x => x.Id == item.Id);

                        if (savedFile != null)
                        {
                            entity.Images.Remove(savedFile);
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

                        entity.Images.Add(toSaveModel);
                    }
                }
            }

            entity.ThumbUrl = entity.Images.FirstOrDefault()?.Url;

            entity.Location = await GetLocation(entity.DistrictId, entity.DivisionId);

            entity.Reporter = user;

            var updated = await _dbContext.UpdateAsync(entity);

            var insertedModel = _mapper.Map<PublicSentNewsModel>(updated);

            return new PublicSentNewsAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<PublicSentPartyInformationAddUpdateResponse> UpdatePublicSentPartyInformation(PublicSentPartyInformationUpdateModel request)
        {
            PublicSentPartyInformation entity = await _dbContext.FindAsync<PublicSentPartyInformation>(request.Id);

            if (entity == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

            User user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            entity = _mapper.Map<PublicSentPartyInformationUpdateModel, PublicSentPartyInformation>(request, entity);

            if (request.Images != null)
            {
                if (entity.Images == null)
                {
                    entity.Images = new List<FileUpload>();
                }

                foreach (var item in request.Images)
                {
                    if (item.Status == (int)WellKnownStatus.Deleted)
                    {
                        var savedFile = entity.Images.FirstOrDefault(x => x.Id == item.Id);

                        if (savedFile != null)
                        {
                            entity.Images.Remove(savedFile);
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

                        entity.Images.Add(toSaveModel);
                    }
                }
            }

            entity.ThumbUrl = entity.Images.FirstOrDefault()?.Url;

            entity.Location = await GetLocation(entity.DistrictId, entity.DivisionId);

            entity.Reporter = user;

            var updated = await _dbContext.UpdateAsync(entity);

            var insertedModel = _mapper.Map<PublicSentPartyInformationModel>(updated);

            return new PublicSentPartyInformationAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<PublicSentInformationAddUpdateResponse> UpdatePublicSentInformation(PublicSentInformationUpdateModel request)
        {
            PublicSentInformation entity = await _dbContext.FindAsync<PublicSentInformation>(request.Id);

            if (entity == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

            User user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            entity = _mapper.Map<PublicSentInformationUpdateModel, PublicSentInformation>(request, entity);

            if (request.Images != null)
            {
                if (entity.Images == null)
                {
                    entity.Images = new List<FileUpload>();
                }

                foreach (var item in request.Images)
                {
                    if (item.Status == (int)WellKnownStatus.Deleted)
                    {
                        var savedFile = entity.Images.FirstOrDefault(x => x.Id == item.Id);

                        if (savedFile != null)
                        {
                            entity.Images.Remove(savedFile);
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

                        entity.Images.Add(toSaveModel);
                    }
                }
            }

            entity.ThumbUrl = entity.Images.FirstOrDefault()?.Url;

            entity.Location = await GetLocation(entity.DistrictId, entity.DivisionId);

            entity.Reporter = user;

            var updated = await _dbContext.UpdateAsync(entity);

            var insertedModel = _mapper.Map<PublicSentInformationModel>(updated);

            return new PublicSentInformationAddUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<ConfigUpdateResponse> UpdateConfig(ConfigUpdateRequest request)
        {
            Configuration entity = await _dbContext.FilterOneAsync<Configuration>(x => x.Status == (int)WellKnownStatus.Active);

            if (entity == null)
            {
                entity = _mapper.Map<ConfigUpdateRequest, Configuration>(request);
                entity.CreatedOn = DateTime.Now;
                entity.UpdatedOn = DateTime.Now;
                entity.Status = (int)WellKnownStatus.Active;
            }
            else
            {
                entity = _mapper.Map<ConfigUpdateRequest, Configuration>(request, entity);
            }

            if (request.Images != null)
            {
                if (entity.Images == null)
                {
                    entity.Images = new List<FileUpload>();
                }

                foreach (var item in request.Images)
                {
                    if (item.Status == (int)WellKnownStatus.Deleted)
                    {
                        var savedFile = entity.Images.FirstOrDefault(x => x.Id == item.Id);

                        if (savedFile != null)
                        {
                            entity.Images.Remove(savedFile);
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

                        entity.Images.Add(toSaveModel);
                    }
                }
            }

            Configuration updated = null;

            if (string.IsNullOrEmpty(entity.Id))
            {
                await _dbContext.SaveAsync(entity);

                updated = await _dbContext.FilterOneAsync<Configuration>(x => x.Status == (int)WellKnownStatus.Active);
            }
            else
            {
                updated = await _dbContext.UpdateAsync(entity);
            }

            var insertedModel = _mapper.Map<ConfigurationModel>(updated);

            return new ConfigUpdateResponse()
            {
                Item = insertedModel,
            };
        }

        public async Task<GetConfigurationResponse> GetConfiguration(GetConfigurationRequest request)
        {
            var user = await _dbContext.FindAsync<User>(request.UserId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (user.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_UserDeactivated);

            Configuration entity = await _dbContext.FilterOneAsync<Configuration>(x => x.Status == (int)WellKnownStatus.Active);

            var configurationModel = _mapper.Map<ConfigurationModel>(entity);

            var userModel = _mapper.Map<UserModel>(user);

            configurationModel.User = userModel;

            return new GetConfigurationResponse()
            {
                AppConfig = configurationModel,
            };
        }
    }
}
