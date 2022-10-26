using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GeoCoordinatePortable;
using GoogleApi;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Places.Details.Request;
using GoogleApi.Entities.Places.Search.NearBy.Request;
using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Core.Extensions;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EnterpriseApp.API.Core.Services
{
    public class RestaurantService : BaseService, IRestaurantService
    {
        private readonly IFileService _fileService;
        public string MapboxAccessToken { get; }
        public string GoogleApiKey { get; }

        public RestaurantService(EnterpriseAppContext dbContext, IMapper mapper, IFileService fileService, IConfiguration configuration) : base(dbContext, mapper)
        {
            _fileService = fileService;
            MapboxAccessToken = configuration["Mapbox:AccessToken"];
            GoogleApiKey = "xxxxxx";
        }

        #region Public 

        public async Task<IActionResult> SearchRestaurentDetail(SearchRequest searchRequest)
        {
            List<RestaurantModel> localRestaurants = GetRestaurantName(searchRequest).ToList();

            RestaurantModelSearch restaurantModelSearch = new RestaurantModelSearch();
            List<RestaurantModel> googleRestaurants = new List<RestaurantModel>();
            List<RestaurantModel> newGoogleRestaurants = new List<RestaurantModel>();

            PlacesNearBySearchRequest placesNearBySearchRequest = new PlacesNearBySearchRequest
            {
                Key = GoogleApiKey,
                Location = new GoogleApi.Entities.Places.Search.NearBy.Request.Location(searchRequest.latitude, searchRequest.longitude),
                Radius = 50000,
                Type = GoogleApi.Entities.Places.Search.Common.Enums.SearchPlaceType.Restaurant,
                Language = Language.French,
                Keyword = searchRequest.name.Trim(),
            };

            var nearbyResult = await GooglePlaces.NearBySearch.QueryAsync(placesNearBySearchRequest);

            foreach (var result in nearbyResult.Results)
            {
                RestaurantModel googleRestaurant = new RestaurantModel();

                var detailsResonse = await GooglePlaces.Details.QueryAsync(new PlacesDetailsRequest
                {
                    Key = GoogleApiKey,
                    PlaceId = result.PlaceId
                });

                var userCord = new GeoCoordinate(searchRequest.latitude, searchRequest.longitude);
                var detailsResult = detailsResonse.Result;

                if (detailsResult.Name.ContainsIgnoreCase(searchRequest.name.Trim()))
                {
                    googleRestaurant.RAddress = detailsResult.FormattedAddress;
                    googleRestaurant.RName = detailsResult.Name;
                    googleRestaurant.RLatitude = detailsResult.Geometry.Location.Latitude.ToString();
                    googleRestaurant.RLongitude = detailsResult.Geometry.Location.Longitude.ToString();
                    googleRestaurant.Distance = userCord.GetDistanceTo(new GeoCoordinate(detailsResult.Geometry.Location.Latitude, detailsResult.Geometry.Location.Longitude));
                    googleRestaurant.GooglePlaceId = result.PlaceId;

                    googleRestaurants.Add(googleRestaurant);
                }
            }

            foreach (var gRestaurant in googleRestaurants)
            {
                var newgoogleRestaurant = localRestaurants.Any(e => e.GooglePlaceId == gRestaurant.GooglePlaceId && e.Status == (int)WellKnownStatus.Active);

                if (!newgoogleRestaurant)
                    newGoogleRestaurants.Add(gRestaurant);
            }

            restaurantModelSearch.LocalRestaurants.AddRange(localRestaurants.Where(e => e.Status == (int)WellKnownStatus.Active));
            restaurantModelSearch.GoogleRestaurants = newGoogleRestaurants;
            restaurantModelSearch.GoogleRestaurants.AddRange(localRestaurants.Where(e => e.Status == (int)WellKnownStatus.Deactive));

            return new JsonResult(restaurantModelSearch);
        }

        public async Task<PaginatedResponse<RestaurantModel>> GetRestaurants(GetRestaurantListRequest request)
        {
            var resQuary = _dbContext.Restaurants
                .Where(q => q.Status == (int)WellKnownStatus.Active).ProjectTo<RestaurantModel>(_mapper.ConfigurationProvider);

            List<RestaurantModel> list = new List<RestaurantModel>();

            foreach (var item in resQuary)
            {
                item.Distance = GetGeoCoordinate(request.User_Location_Lat, request.User_Location_Lon, item.RLatitude, item.RLongitude);
                list.Add(item);
            }

            var models = list.AsQueryable().OrderBy(d => d.Distance);

            var filteredList = await models.AsPaginatedResponseAsync(request);

            if (filteredList.TotalDataRecords > 0)
            {

                var resOpeningHours = _dbContext.ResOpeningHours.Where(q => q.Status == (int)WellKnownStatus.Active).
                                      OrderBy(q => q.RestaurantId).ProjectTo<ResOpeningHourModel>(_mapper.ConfigurationProvider).ToList();

                foreach (var model in filteredList.Data)
                {
                    model.UserFavouriteRestaurantList = new List<UserFavouriteRestaurantModel>();

                    if (!string.IsNullOrEmpty(request.UserId))
                        FetchUserRestaurantDetails(model, request.UserId);

                    FetchExtraProperties(model, resOpeningHours);
                }
            }

            return filteredList;
        }

        public RestaurantModel GetRestaurantById(int resId, string userId = null)
        {
            var restaurant = _dbContext.Restaurants
               .Where(q => q.Status == (int)WellKnownStatus.Active && q.Id == resId)
               .ProjectTo<RestaurantModel>(_mapper.ConfigurationProvider).SingleOrDefault();

            if (restaurant == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidRestaurant);

            if (restaurant.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_RestaurantDeactivated);

            var resOpeningHours = _dbContext.ResOpeningHours.Where(q => q.Status == (int)WellKnownStatus.Active).
                                    Where(q => q.RestaurantId == restaurant.Id).ProjectTo<ResOpeningHourModel>(_mapper.ConfigurationProvider).ToList();

            FetchExtraProperties(restaurant, resOpeningHours);

            var rate = _dbContext.ResComments.FirstOrDefault(c => c.RestaurantId == resId && c.Status == (int)WellKnownStatus.Active);

            if (rate != null)
                restaurant.Rate = rate.RComment;

            if (!string.IsNullOrEmpty(userId))
                FetchUserRestaurantDetails(restaurant, userId);

            var subCat = (from sb in _dbContext.SubCategorires.Include(r => r.ResSubcategoryEntityList)
                          select new SelectListItem
                          {
                              Value = sb.Id.ToString(),
                              Text = sb.Name,
                              Selected = checkSub(sb.ResSubcategoryEntityList, resId)
                          });

            if (subCat != null)
                restaurant.SubcategoriesList = subCat.ToList();

            var resBadges = (from opt in _dbContext.Badge.Include(s => s.ResBadgeEntity)
                             select new ResBadgeModel
                             {
                                 IconId = opt.IconId,
                                 Id = opt.Id,
                                 Name = opt.Name,
                                 Selected = checkOpt(opt.ResBadgeEntity, resId)
                             });

            if (resBadges != null)
                restaurant.ResBadges = resBadges.ToList();

            var restaurantBadges = (from opt in _dbContext.Badge.Include(s => s.ResBadgeEntity)
                                    where opt.ResBadgeEntity.Any(c => c.RestaurantId == resId)
                                    select new ResBadgeModel
                                    {
                                        IconId = opt.IconId,
                                        Id = opt.Id,
                                        Name = opt.Name,
                                    });

            if (restaurantBadges != null)
                restaurant.RestaurantBadges = restaurantBadges.ToList();

            return restaurant;
        }

        public IEnumerable<ResCommentModel> GetRestaurantComments(int resId)
        {
            CheckRestaurantExists(resId);

            var models = _dbContext.ResComments.Where(q => q.Status == (int)WellKnownStatus.Active && q.RestaurantId == resId).
            OrderBy(q => q.RCDate).ProjectTo<ResCommentModel>(_mapper.ConfigurationProvider).ToList();

            return models;
        }

        public IEnumerable<RestaurantModel> GetUserRestaurants(int userId)
        {
            CheckUserExists(userId);

            var userResLists = _dbContext.UserResLists.Include(x => x.ResListRestaurants).Where(x => x.UserId == userId &&
                x.ResListRestaurants != null && x.ResListRestaurants.Count > 0 &&
                x.Status == (int)WellKnownStatus.Active);

            var resIds = new List<int>();

            foreach (var userResList in userResLists)
            {
                List<int> splitted = userResList.ResListRestaurants.Select(x => x.RestaurantId).ToList();
                resIds.AddRange(splitted);
            }

            var models = _dbContext.Restaurants.Where(x => x.Status == (int)WellKnownStatus.Active && resIds.Contains(x.Id)).
            OrderBy(q => q.RName).ProjectTo<RestaurantModel>(_mapper.ConfigurationProvider).ToList();

            foreach (var model in models)
            {
                if (!string.IsNullOrEmpty(userId.ToString()))
                    FetchUserRestaurantDetails(model, userId.ToString());
            }

            return models;
        }

        public AddUserVisitedRestaurantResponse AddUserVisitedRestaurant(UserVisitedRestaurantRequest request)
        {
            CheckUserExists(request.UserId);
            CheckRestaurantExists(request.RestaurantId);
            CheckCountryExists(request.CountryId);

            var entity = _mapper.Map<UserVisitedRestaurant>(request);

            entity.CreatedOn = DateTimeOffset.Now;
            entity.Status = (int)WellKnownStatus.Active;

            _dbContext.UserVisitedRestaurant.Add(entity);

            _dbContext.SaveChanges();

            var insertedModel = _mapper.Map<UserVisitedRestaurantModel>(entity);

            return new AddUserVisitedRestaurantResponse()
            {
                IsSuccessful = true
            };
        }

        public AddRestaurantCommentResponse AddRestaurantComment(UserCommentRequest request)
        {
            CheckUserExists(request.UserId);
            CheckRestaurantExists(request.ArticleId);

            var restaurantComment = _dbContext.ResComments.Where(c => c.RestaurantId == request.ArticleId && c.UserId == request.UserId).ToList();

            if (restaurantComment.Count > 0)
            {
                foreach (var item in restaurantComment)
                {
                    item.Status = (int)WellKnownStatus.Deactive;
                    item.DeletedOn = DateTimeOffset.Now;

                    _dbContext.ResComments.Update(item);
                    _dbContext.SaveChanges();
                }
            }

            foreach (var rProblem in request..PProblems)
            {
                _dbContext.ResComments.Add(new ResComment
                {
                    CreatedOn = DateTimeOffset.Now,
                    RCDate = DateTimeOffset.Now,
                    RComment = rProblem,
                    RestaurantId = request.ArticleId,
                    Status = (int)WellKnownStatus.Active,
                    UserId = request.UserId
                });

                _dbContext.SaveChanges();
            }

            return new AddRestaurantCommentResponse()
            {
                IsSuccessful = true,
            };
        }

        public AddUserRequestedRestaurantResponse AddUserRequestedRestaurant(UserRequestedRestaurantRequest request)
        {
            CheckUserExists(request.UserId);
            CheckCountryExists(request.CountryId);

            var entity = _mapper.Map<UserRestaurantRequest>(request);

            entity.CreatedOn = DateTimeOffset.Now;
            entity.Status = (int)WellKnownStatus.Active;

            _dbContext.ResRequests.Add(entity);

            _dbContext.SaveChanges();

            var insertedModel = _mapper.Map<UserRestaurantRequestModel>(entity);

            return new AddUserRequestedRestaurantResponse()
            {
                IsSuccessful = true,
                Request = insertedModel
            };
        }

        public AddUserRestaurantListResponse AddUserRestaurantList(AddUserRestaurantListRequest request)
        {
            CheckUserExists(request.UserId);

            var entity = _mapper.Map<UserResList>(request);

            entity.CreatedOn = DateTimeOffset.Now;
            entity.Status = (int)WellKnownStatus.Active;

            _dbContext.UserResLists.Add(entity);

            _dbContext.SaveChanges();

            return new AddUserRestaurantListResponse()
            {
                IsSuccessful = true
            };
        }

        public IEnumerable<UserRestaurantRequestModel> GetUserRestaurantRequests(int userId)
        {
            CheckUserExists(userId);

            var models = _dbContext.ResRequests.Where(q => q.Status == (int)WellKnownStatus.Active && q.UserId == userId).
            OrderBy(q => q.RName).ProjectTo<UserRestaurantRequestModel>(_mapper.ConfigurationProvider).ToList();

            return models;
        }

        IEnumerable<UserVisitedRestaurantModel> IRestaurantService.GetVisitedRestaurants(int userId)
        {
            CheckUserExists(userId);

            var models = _dbContext.UserVisitedRestaurant.Where(q => q.Status == (int)WellKnownStatus.Active && q.UserId == userId).
            OrderBy(q => q.RestaurantId).ProjectTo<UserVisitedRestaurantModel>(_mapper.ConfigurationProvider).ToList();

            return models;
        }

        public IEnumerable<RestaurantModel> GetRestaurantsByBadge(string badgeName)
        {
            if (string.IsNullOrWhiteSpace(badgeName))
                throw new ApplicationDataException(StatusCode.ERROR_InvalidParameters);

            return _dbContext.ResBadge
                    .Include(c => c.Restaurant)
                    .Include(c => c.Badge)
                    .Where(c => c.Restaurant.Status == (int)WellKnownStatus.Active && c.Badge.Name.ContainsIgnoreCase(badgeName))
                    .OrderBy(o => o.Restaurant.RName).ProjectTo<RestaurantModel>(_mapper.ConfigurationProvider).ToList();
        }

        public RestaurantModel AddUserFavouriteRestaurant(UserFavouriteRestaurantRequest request)
        {
            CheckUserExists(request.UserId);
            CheckRestaurantExists(request.RestaurantId);

            var entity = _dbContext.UserFavouriteRestaurant.FirstOrDefault(x => x.UserId == request.UserId && x.RestaurantId == request.RestaurantId);

            if (entity != null)
            {
                _dbContext.UserFavouriteRestaurant.Remove(entity);
                _dbContext.SaveChanges();
            }

            if (request.IsFavorite)
            {
                var newEntity = _mapper.Map<UserFavouriteRestaurant>(request);

                newEntity.CreatedOn = DateTimeOffset.Now;
                newEntity.Status = (int)WellKnownStatus.Active;

                _dbContext.UserFavouriteRestaurant.Add(newEntity);
                _dbContext.SaveChanges();
            }

            var restaurant = GetRestaurantById(request.RestaurantId, request.UserId.ToString());

            return restaurant;
        }

        public RestaurantModel RemoveUserFavouriteRestaurant(RemoveUserFavouriteRestaurantRequest request)
        {
            CheckUserExists(request.UserId);
            CheckRestaurantExists(request.RestaurantId);

            var entity = _dbContext.UserFavouriteRestaurant.FirstOrDefault(x => x.UserId == request.UserId && x.RestaurantId == request.RestaurantId);

            if (entity != null)
            {
                _dbContext.UserFavouriteRestaurant.Remove(entity);
                _dbContext.SaveChanges();
            }

            var restaurant = GetRestaurantById(request.RestaurantId, request.UserId.ToString());

            return restaurant;
        }

        public AddUserRestaurentChoiceResponse AddUserRestaurantChoice(UserRestaurantChoiceRequest request)
        {
            CheckUserExists(request.UserId);
            CheckRestaurantExists(request.RestaurantId);

            var entity = _dbContext.UserRestaurantChoices.FirstOrDefault(x => x.UserId == request.UserId && x.RestaurantId == request.RestaurantId);

            if (entity != null)
            {
                _dbContext.UserRestaurantChoices.Remove(entity);
                _dbContext.SaveChanges();
            }

            var newEntity = _mapper.Map<UserRestaurantChoice>(request);

            newEntity.CreatedOn = DateTimeOffset.Now;
            newEntity.Status = (int)WellKnownStatus.Active;

            _dbContext.UserRestaurantChoices.Add(newEntity);

            _dbContext.SaveChanges();

            return new AddUserRestaurentChoiceResponse()
            {
                IsSuccessful = true
            };
        }

        public IEnumerable<CountryModel> GetCountries()
        {
            var models = _dbContext.Countries.Where(q => q.Status == (int)WellKnownStatus.Active).
            OrderByDescending(q => q.SortOrder).ProjectTo<CountryModel>(_mapper.ConfigurationProvider).ToList();

            return models;
        }

        public AddUserRestaurantResponse AddUserRestaurant(AddUserRestaurantRequest request)
        {
            var restaurant = _dbContext.Restaurants.SingleOrDefault(r => r.Id == request.RestaurantId);

            if (restaurant == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidRestaurant);

            var list = _dbContext.UserResLists.SingleOrDefault(r => r.Id == request.ListId);

            if (list == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidRestaurantList);

            var entity = _dbContext.ResListRestaurants.FirstOrDefault(x => x.RestaurantId == request.RestaurantId
                && x.ListId == request.ListId);

            if (entity != null)
            {
                _dbContext.ResListRestaurants.Remove(entity);
                _dbContext.SaveChanges();
            }

            var newEntity = _mapper.Map<ResListRestaurant>(request);

            newEntity.CreatedOn = DateTimeOffset.Now;
            newEntity.Status = (int)WellKnownStatus.Active;

            _dbContext.ResListRestaurants.Add(newEntity);

            _dbContext.SaveChanges();

            return new AddUserRestaurantResponse()
            {
                IsSuccessful = true
            };
        }

        public DeleteUserRestaurantResponse DeleteUserRestaurant(DeleteUserRestaurantRequest request)
        {
            var restaurant = _dbContext.Restaurants.SingleOrDefault(r => r.Id == request.RestaurantId);

            if (restaurant == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidRestaurant);

            var list = _dbContext.UserResLists.SingleOrDefault(r => r.Id == request.ListId);

            if (list == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidRestaurantList);

            var entity = _dbContext.ResListRestaurants.FirstOrDefault(x => x.RestaurantId == request.RestaurantId
                && x.ListId == request.ListId);

            if (entity == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidRestaurantListEntry);

            _dbContext.ResListRestaurants.Remove(entity);
            _dbContext.SaveChanges();

            return new DeleteUserRestaurantResponse()
            {
                IsSuccessful = true
            };
        }

        public EditUserRestaurantListResponse EditUserRestaurantList(EditUserRestaurantListRequest request, bool isDelete = false)
        {
            var list = _dbContext.UserResLists.SingleOrDefault(r => r.Id == request.Id && r.Status == (int)WellKnownStatus.Active);

            if (list == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidRestaurantList);

            if (isDelete)
            {
                list.DeletedOn = DateTimeOffset.Now;
                list.Status = (int)WellKnownStatus.Deleted;
            }
            else
            {
                _mapper.Map<EditUserRestaurantListRequest, UserResList>(request, list);
            }

            _dbContext.UserResLists.Update(list);
            _dbContext.SaveChanges();

            return new EditUserRestaurantListResponse()
            {
                IsSuccessful = true
            };
        }

        public IEnumerable<UserResListModel> GetUserRestaurantList(int userId)
        {
            var models = _dbContext.UserResLists
                            .Include(d => d.ResListRestaurants)
                            .Where(q => q.UserId == userId && q.Status == (int)WellKnownStatus.Active)
                            .OrderBy(q => q.CreatedOn).ProjectTo<UserResListModel>(_mapper.ConfigurationProvider).ToList();

            foreach (var item in models)
            {
                item.Restaurants = new List<RestaurantModel>();

                if (item.ResListRestaurants != null)
                {
                    foreach (var restaurant in item.ResListRestaurants)
                    {
                        var restaurantModel = GetRestaurantById(restaurant.RestaurantId, userId.ToString());
                        item.Restaurants.Add(restaurantModel);
                    }
                }
            }

            return models;
        }

        public IEnumerable<RestaurantModel> GetMyRestaurants(int myListId)
        {
            var models = _dbContext.ResListRestaurants
                            .Where(q => q.Status == (int)WellKnownStatus.Active
                                && q.Restaurant.Status == (int)WellKnownStatus.Active
                                && q.ListId == myListId)
                            .Select(v => v.RestaurantId).ToList();

            List<RestaurantModel> list = new List<RestaurantModel>();

            foreach (var item in models)
            {
                list.Add(GetRestaurantById(item));
            }

            return list;
        }

        public List<UserMyListResponse> GetUserMyList(int userId)
        {
            return _dbContext.UserResLists.Include(res => res.ResListRestaurants)
                        .Where(my => my.Status == (int)WellKnownStatus.Active
                            && my.UserId == userId)
                        .Select(m => new UserMyListResponse
                        {
                            CreatedOn = m.CreatedOn,
                            IconId = m.IconId,
                            LColour = m.LColour,
                            ListId = m.Id,
                            ListName = m.ListName,
                            RestaurantCount = (m.ResListRestaurants.Where(c => c.Restaurant.Status == (int)WellKnownStatus.Active)).Count()
                        }).OrderByDescending(c => c.CreatedOn).ToList();
        }

        public async Task<PaginatedResponse<RestaurantModel>> FilterRestaurants(FilterRestaurantRequest request, PagingQueryParam param)
        {
            var query = _dbContext.Restaurants
                        .Include(r => r.ResBadgeEntity).ThenInclude(op => op.Badge)
                        .Include(s => s.ResSubcategoryEntityList).ThenInclude(sm => sm.Subcategory)
                        .Where(q => q.Status == (int)WellKnownStatus.Active).Select(c => c);

            if (request.Filter1 != null && request.Filter1.Any())
                query = query.Where(x => request.Filter1.Any(a => a == x.ResCategory.Name));

            if (request.Filter2 != null && request.Filter2.Any())
            {
                var status = false;

                foreach (var item in request.Filter2)
                {
                    if (string.IsNullOrWhiteSpace(item))
                        status = true;
                }

                if (!status)
                {
                    query = query.Where(x => x.ResSubcategoryEntityList.Any(q => q.Status == (int)WellKnownStatus.Active));
                    query = query.Where(x => x.ResSubcategoryEntityList.Any(q => request.Filter2.Contains(q.Subcategory.Name)));
                }
            }

            if (request.MyList != null && request.MyList.Any() && request.MyList.Count > 0)
            {
                var status = false;

                foreach (var item in request.MyList)
                {
                    if (item > 0)
                        status = true;
                }

                if (status)
                    query = query.Where(x => x.ResListItems.Any(q => q.Status == (int)WellKnownStatus.Active && request.MyList.Contains(q.ListId)));
            }

            if (request.UserId > 0 && request.Favourite)
                query = query.Where(x => x.UserFavouriteRestaurantList.Any(q => q.Status == (int)WellKnownStatus.Active && q.UserId == request.UserId));

            var models = query.ProjectTo<RestaurantModel>(_mapper.ConfigurationProvider).ToList();

            if (request.Filter3 != null && request.Filter3.Any())
            {
                var modelsList = models.Where(x => x.RestaurantBadges.Count >= request.Filter3.Count && x.Status == (int)WellKnownStatus.Active);

                models = modelsList.Where(x => x.RestaurantBadges.Select(r => r.IconId).Intersect(request.Filter3, StringComparer.OrdinalIgnoreCase).Count() >= request.Filter3.Count).ToList();
            }

            var modelsPagination = models.AsQueryable();

            var modelList = await modelsPagination.AsPaginatedResponseAsync(param);

            if (modelList.TotalDataRecords > 0)
            {
                var resOpeningHours = _dbContext.ResOpeningHours.
                    Where(q => q.Status == (int)WellKnownStatus.Active).
                    OrderBy(q => q.RestaurantId).ProjectTo<ResOpeningHourModel>(_mapper.ConfigurationProvider).ToList();

                foreach (var model in modelList.Data)
                {
                    if (!string.IsNullOrEmpty(request.UserId.ToString()))
                        FetchUserRestaurantDetails(model, request.UserId.ToString());

                    FetchExtraProperties(model, resOpeningHours);
                }
            }

            return modelList;
        }

        public bool CheckResturantStatus(int postalCode)
        {
            var restaurants = _dbContext.Restaurants.Where(r => r.RPostalCode == postalCode.ToString());

            foreach (var item in restaurants)
            {
                if (item.Status == (int)WellKnownStatus.Deactive)
                    return false;
            }

            return true;
        }

        public bool ChangeResturantStatus(int postalCode, bool status)
        {
            var restaurants = _dbContext.Restaurants.Where(c => c.RPostalCode == postalCode.ToString()).ToList();

            if (restaurants.Count <= 0)
                return false;

            foreach (var restaurant in restaurants)
            {
                if (status)
                    restaurant.Status = (int)WellKnownStatus.Active;
                else
                    restaurant.Status = (int)WellKnownStatus.Deactive;

                _dbContext.Restaurants.Update(restaurant);
                _dbContext.SaveChanges();
            }

            return true;
        }

        public async Task<PaginatedResponse<RestaurantResponse>> GetRestaurantList(GetRestaurantListRequest request)
        {
            //var allRestaurant = _dbContext.Restaurants
            //                       .Where(q => q.Status == (int)WellKnownStatus.Active)
            //                       .ProjectTo<RestaurantResponse>(_mapper.ConfigurationProvider);


            var allRestaurants = _dbContext.Restaurants
                                    .Where(r => r.Status == (int)WellKnownStatus.Active)
                                    .Select(d => new RestaurantResponse
                                    {
                                        Id = d.Id,
                                        RLatitude = d.RLatitude,
                                        MapIcon = d.MapIcon,
                                        RDescription = d.RDescription,
                                        RLongitude = d.RLongitude,
                                        RMetro = d.RMetro,
                                        RName = d.RName,
                                        Distance = GetGeoCoordinate(request.User_Location_Lat, request.User_Location_Lon, d.RLatitude, d.RLongitude),
                                        Images = (from image in _dbContext.FileUploads
                                                  where (image.RestaurantId == d.Id && image.Status == (int)WellKnownStatus.Active && image.Type == (int)WellKnownFileUploadType.Restaurant)
                                                  select new CommonFileUploadModel()
                                                  {
                                                      FileName = image.FileName,
                                                      Description = image.Description,
                                                      Id = image.Id,
                                                      Name = image.Name,
                                                      RestaurantId = image.RestaurantId,
                                                      Status = image.Status,
                                                      Type = image.Type
                                                  }).ToList(),
                                    }).OrderBy(c => c.Distance);

            var filteredList = await allRestaurants.AsPaginatedResponseAsync(request);

            if (filteredList.TotalDataRecords > 0)
            {
                var resOpeningHours = _dbContext.ResOpeningHours.Where(q => q.Status == (int)WellKnownStatus.Active).
                                      OrderBy(q => q.RestaurantId).ProjectTo<ResOpeningHourModel>(_mapper.ConfigurationProvider).ToList();

                foreach (var restaurant in filteredList.Data)
                {
                    restaurant.IsOpen = VerifyIsopen(restaurant.Id, resOpeningHours);
                    FetchRestaurantDetails(restaurant, request.UserId);
                }
            }

            return filteredList;
        }

        #region BackOffice

        private double GetGeoCoordinate(double user_Location_Lat, double user_Location_Lon, string rLatitude, string rLongitude)
        {
            double returnValue = 0;

            if (user_Location_Lat > 0 && user_Location_Lon > 0)
            {
                double.TryParse(rLatitude, out double lat);
                double.TryParse(rLongitude, out double lon);

                if (lat > 0 && lon > 0)
                {
                    var userCord = new GeoCoordinate(user_Location_Lat, user_Location_Lon);
                    var resCord = new GeoCoordinate(lat, lon);
                    returnValue = userCord.GetDistanceTo(resCord);
                }
            }

            return returnValue;
        }

        public IEnumerable<RestaurantModel> GetRestaurantName(SearchRequest searchRequest)
        {
            var models = _dbContext.Restaurants.
            //.Where(q => q.Status == (int)WellKnownStatus.Active).
            OrderBy(q => q.RName).ProjectTo<RestaurantModel>(_mapper.ConfigurationProvider).ToList();

            List<RestaurantModel> filteredList = new List<RestaurantModel>();

            var userCord = new GeoCoordinate(searchRequest.latitude, searchRequest.longitude);

            foreach (var model in models)
            {
                if (model.RName.ContainsIgnoreCase(searchRequest.name))
                {
                    double.TryParse(model.RLatitude, out double lat);
                    double.TryParse(model.RLongitude, out double lon);

                    if (lat > 0 && lon > 0)
                    {
                        var resCord = new GeoCoordinate(lat, lon);

                        var distance = userCord.GetDistanceTo(resCord);

                        model.Distance = distance;
                        filteredList.Add(model);
                    }
                }
            }

            if (filteredList.Count > 0)
            {
                var resOpeningHours = _dbContext.ResOpeningHours.Where(q => q.Status == (int)WellKnownStatus.Active).
                                      OrderBy(q => q.RestaurantId).ProjectTo<ResOpeningHourModel>(_mapper.ConfigurationProvider).ToList();

                foreach (var model in filteredList)
                {
                    if (!string.IsNullOrEmpty(searchRequest.UserId))
                        FetchUserRestaurantDetails(model, searchRequest.UserId);

                    FetchExtraProperties(model, resOpeningHours);
                }
            }

            return filteredList;
        }

        public IEnumerable<RestaurantModel> GetAllRestaurants()
        {
            var allRestaurants = _dbContext.Restaurants
                                .OrderBy(q => q.RName).ProjectTo<RestaurantModel>(_mapper.ConfigurationProvider).ToList();

            return allRestaurants;
        }

        public RestaurantModelResponse GetAllRestaurants(PagingQueryParam param)
        {
            var allRestaurants = _dbContext.Restaurants
                                    .Select(r => new BackOfficeRestaurant
                                    {
                                        Id = r.Id,
                                        MapIcon = r.MapIcon,
                                        RAddress = r.RAddress,
                                        RMetro = r.RMetro,
                                        RName = r.RName,
                                        Status = SetStatus(r.Status)
                                    });

            int totalRecordCount = allRestaurants.Count();

            //Filter records
            allRestaurants = Filter(param.Filter, allRestaurants);

            //Order records
            allRestaurants = Sort(param.Sort, allRestaurants);

            if (string.IsNullOrWhiteSpace(param.Sort))
                allRestaurants = allRestaurants.OrderBy(r => r.RName);

            var pagedAdminUserList = allRestaurants.Skip(param.StartingIndex).Take(param.PageSize).ToList();

            return new RestaurantModelResponse { List = pagedAdminUserList, TotalRecords = totalRecordCount };
        }

        private IQueryable<BackOfficeRestaurant> Filter(string filter, IQueryable<BackOfficeRestaurant> q)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                q = q.Where(f => (f.RName.Contains(filter)));
            }

            return q;
        }

        private IQueryable<BackOfficeRestaurant> Sort(string sortParam, IQueryable<BackOfficeRestaurant> q)
        {
            if (!string.IsNullOrWhiteSpace(sortParam))
            {
                var arrayStr = sortParam.Split('|');

                if (arrayStr.Count() >= 2)
                {
                    int columnNumber = 0;

                    if (Int32.TryParse(arrayStr[0], out columnNumber))
                    {
                        string order = arrayStr[1];

                        if (!string.IsNullOrEmpty(order) && order == SortType.Asc)
                            return GetMappedColumn(columnNumber, true, q);
                        else if (!string.IsNullOrEmpty(order) && order == SortType.Desc)
                            return GetMappedColumn(columnNumber, false, q);
                    }
                }
            }

            return q;
        }

        private IQueryable<BackOfficeRestaurant> GetMappedColumn(int columnNumber, bool asc, IQueryable<BackOfficeRestaurant> q)
        {
            if (asc)
            {
                // Added sorting
                if (columnNumber == (int)SortingColumnTransaction.Id)
                    return q.OrderBy(f => f.Id);
                else if (columnNumber == (int)SortingColumnTransaction.RName)
                    return q.OrderBy(f => f.RName);
                else if (columnNumber == (int)SortingColumnTransaction.RAddress)
                    return q.OrderBy(f => f.RAddress);
            }
            else
            {
                // Added sorting
                if (columnNumber == (int)SortingColumnTransaction.Id)
                    return q.OrderByDescending(f => f.Id);
                else if (columnNumber == (int)SortingColumnTransaction.RName)
                    return q.OrderByDescending(f => f.RName);
                else if (columnNumber == (int)SortingColumnTransaction.RAddress)
                    return q.OrderByDescending(f => f.RAddress);
            }

            return q;
        }

        enum SortingColumnTransaction
        {
            Id = 0,
            RName = 1,
            RAddress = 2,
            RPrice = 3
        }

        private bool SetStatus(int status)
        {
            return status == (int)WellKnownStatus.Active;
        }

        public RestaurantModel GetBackOfficeRestaurantById(int resId)
        {
            var restaurant = _dbContext.Restaurants
               .Where(q => q.Id == resId)
               .ProjectTo<RestaurantModel>(_mapper.ConfigurationProvider).SingleOrDefault();

            var resOpeningHours = _dbContext.ResOpeningHours.Where(q => q.Status == (int)WellKnownStatus.Active).
                                    Where(q => q.RestaurantId == restaurant.Id).ProjectTo<ResOpeningHourModel>(_mapper.ConfigurationProvider).ToList();

            FetchExtraProperties(restaurant, resOpeningHours);

            var rate = _dbContext.ResComments.FirstOrDefault(c => c.RestaurantId == resId && c.Status == (int)WellKnownStatus.Active);

            if (rate != null)
                restaurant.Rate = rate.RComment;

            var subCat = (from sb in _dbContext.SubCategorires.Include(r => r.ResSubcategoryEntityList)
                          select new SelectListItem
                          {
                              Value = sb.Id.ToString(),
                              Text = sb.Name,
                              Selected = checkSub(sb.ResSubcategoryEntityList, resId)
                          });

            if (subCat != null)
                restaurant.SubcategoriesList = subCat.ToList();

            var resBadges = (from opt in _dbContext.Badge.Include(s => s.ResBadgeEntity)
                             select new ResBadgeModel
                             {
                                 IconId = opt.IconId,
                                 Id = opt.Id,
                                 Name = opt.Name,
                                 Selected = checkOpt(opt.ResBadgeEntity, resId)
                             });

            if (resBadges != null)
                restaurant.ResBadges = resBadges.ToList();

            var restaurantBadges = (from opt in _dbContext.Badge.Include(s => s.ResBadgeEntity)
                                    where opt.ResBadgeEntity.Any(c => c.RestaurantId == resId)
                                    select new ResBadgeModel
                                    {
                                        IconId = opt.IconId,
                                        Id = opt.Id,
                                        Name = opt.Name,
                                    });

            if (restaurantBadges != null)
                restaurant.RestaurantBadges = restaurantBadges.ToList();

            var mapIcon = _dbContext.SubCategorires.FirstOrDefault(s => s.Name == restaurant.MapIcon);
            if (mapIcon != null)
                restaurant.MapIcon = mapIcon.Id.ToString();

            return restaurant;
        }

        public List<BadgeModel> GetBadges()
        {
            return _dbContext.Badge.OrderBy(r => r.Name).ProjectTo<BadgeModel>(_mapper.ConfigurationProvider).ToList();
        }

        public RestaurantInsertResponse AddRestaurant(RestaurantRequest request)
        {
            var resCategories = _dbContext.ResCategories.FirstOrDefault(c => c.Id == request.CategoryId && c.Status == (int)WellKnownStatus.Active);
            if (resCategories == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidRestaurantCategory);

            var countries = _dbContext.Countries.FirstOrDefault(c => c.Id == request.CountryId && c.Status == (int)WellKnownStatus.Active);
            if (countries == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidCountry);

            var entity = _mapper.Map<Restaurant>(request);

            entity.CreatedOn = DateTimeOffset.Now;
            entity.Status = (int)WellKnownStatus.Active;
            entity.MapIcon = SubCatName(request.MapIcon);

            _dbContext.Restaurants.Add(entity);
            _dbContext.SaveChanges();

            var insertedModel = _mapper.Map<RestaurantModel>(entity);

            foreach (var resOpeningHour in request.ROpens)
            {
                var entityOpen = new ResOpeningHour()
                {
                    CreatedOn = DateTimeOffset.Now,
                    Closes = resOpeningHour.Closes,
                    Day = resOpeningHour.Day,
                    Opens = resOpeningHour.Opens,
                    RestaurantId = entity.Id,
                    Session = resOpeningHour.Session.ToString(),
                    Status = (int)WellKnownStatus.Active
                };

                _dbContext.ResOpeningHours.Add(entityOpen);
                _dbContext.SaveChanges();
            }

            if (request.Subcategories != null)
            {
                foreach (var item in request.Subcategories)
                {
                    _dbContext.ResSubCategories.Add(new ResSubcategoryEntity
                    {
                        CreatedOn = DateTimeOffset.Now,
                        RestaurantId = entity.Id,
                        SubcategoryId = item,
                        Status = (int)WellKnownStatus.Active
                    });

                    _dbContext.SaveChanges();
                }
            }

            if (request.ResBadgeList != null)
            {
                foreach (var item in request.ResBadgeList)
                {
                    _dbContext.ResBadge.Add(new ResBadgeEntity
                    {
                        CreatedOn = DateTimeOffset.Now,
                        RestaurantId = entity.Id,
                        BadgeId = item,
                        Status = (int)WellKnownStatus.Active
                    });

                    _dbContext.SaveChanges();
                }
            }

            return new RestaurantInsertResponse()
            {
                IsSuccessful = true,
                Restaurant = insertedModel,
            };
        }

        public RestaurantInsertResponse EditRestaurant(RestaurantUpdateRequest request)
        {
            var resCategories = _dbContext.ResCategories.FirstOrDefault(c => c.Id == request.CategoryId);

            if (resCategories == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidRestaurantCategory);

            var countries = _dbContext.Countries.FirstOrDefault(c => c.Id == request.CountryId);
            if (countries == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidCountry);

            var restaurantsEntity = _dbContext.Restaurants.FirstOrDefault(x => x.Id == request.RestaurantId);

            _mapper.Map(request, restaurantsEntity);

            restaurantsEntity.MapIcon = SubCatName(request.MapIcon);

            _dbContext.Restaurants.Update(restaurantsEntity);
            _dbContext.SaveChanges();

            var insertedModel = _mapper.Map<RestaurantModel>(restaurantsEntity);

            var resOpeningHoursEntity = _dbContext.ResOpeningHours.Where(e => e.RestaurantId == restaurantsEntity.Id).ToList();

            foreach (var item in resOpeningHoursEntity)
            {
                _dbContext.ResOpeningHours.Remove(item);
                _dbContext.SaveChanges();
            }

            foreach (var resOpeningHour in request.ROpens)
            {
                var entityOpen = new ResOpeningHour()
                {
                    CreatedOn = DateTimeOffset.Now,
                    Closes = resOpeningHour.Closes,
                    Day = resOpeningHour.Day,
                    Opens = resOpeningHour.Opens,
                    RestaurantId = restaurantsEntity.Id,
                    Session = resOpeningHour.Session.ToString(),
                    Status = (int)WellKnownStatus.Active
                };

                _dbContext.ResOpeningHours.Add(entityOpen);
                _dbContext.SaveChanges();
            }

            var subcategoriesEntiy = _dbContext.ResSubCategories.Where(e => e.RestaurantId == restaurantsEntity.Id).ToList();

            foreach (var item in subcategoriesEntiy)
            {
                _dbContext.ResSubCategories.Remove(item);
                _dbContext.SaveChanges();
            }

            if (request.Subcategories != null)
            {
                foreach (var item in request.Subcategories)
                {
                    _dbContext.ResSubCategories.Add(new ResSubcategoryEntity
                    {
                        CreatedOn = DateTimeOffset.Now,
                        RestaurantId = restaurantsEntity.Id,
                        SubcategoryId = item,
                        Status = (int)WellKnownStatus.Active
                    });
                    _dbContext.SaveChanges();
                }
            }

            var resBadgeEntity = _dbContext.ResBadge.Where(e => e.RestaurantId == restaurantsEntity.Id).ToList();

            foreach (var item in resBadgeEntity)
            {
                _dbContext.ResBadge.Remove(item);
                _dbContext.SaveChanges();
            }

            if (request.ResBadgeList != null)
            {
                foreach (var item in request.ResBadgeList)
                {
                    _dbContext.ResBadge.Add(new ResBadgeEntity
                    {
                        CreatedOn = DateTimeOffset.Now,
                        RestaurantId = restaurantsEntity.Id,
                        BadgeId = item,
                        Status = (int)WellKnownStatus.Active
                    });

                    _dbContext.SaveChanges();
                }
            }

            return new RestaurantInsertResponse()
            {
                IsSuccessful = true,
                Restaurant = insertedModel
            };
        }

        public DeleteRestaurent DeleteRestaurent(int resId)
        {
            var entity = _dbContext.Restaurants.FirstOrDefault(x => x.Id == resId);

            _dbContext.Restaurants.Remove(entity);
            _dbContext.SaveChanges();

            return new DeleteRestaurent()
            {
                IsSuccessful = true
            };
        }

        public IEnumerable<CountryModel> CountryList()
        {
            return _dbContext.Countries.Where(s => s.Status == (int)WellKnownStatus.Active).ProjectTo<CountryModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<ResCategoryModel> ResCategoryList()
        {
            return _dbContext.ResCategories.ProjectTo<ResCategoryModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<ResCategoryModel> SubCategoryList()
        {
            return _dbContext.SubCategorires.ProjectTo<ResCategoryModel>(_mapper.ConfigurationProvider);
        }

        #endregion

        #endregion

        #region Private

        private void FetchRestaurantDetails(RestaurantResponse model, string requestUserId)
        {
            if (!string.IsNullOrWhiteSpace(requestUserId))
            {
                int.TryParse(requestUserId, out int userId);

                var userFavouriteRestaurant = _dbContext.UserFavouriteRestaurant.FirstOrDefault(f => f.UserId == userId && f.RestaurantId == model.Id);

                if (userFavouriteRestaurant != null)
                    model.IsFavourite = true;
            }

            var resBadges = (from opt in _dbContext.Badge.Include(s => s.ResBadgeEntity)
                             where opt.ResBadgeEntity.Any(c => c.RestaurantId == model.Id)
                             select new ResBadgeModel
                             {
                                 IconId = opt.IconId,
                                 Id = opt.Id,
                                 Name = opt.Name
                             });

            if (resBadges != null)
                model.RestaurantBadges = resBadges.ToList();

            model.Images.ToList().ForEach(x =>
            {
                x.Url = _fileService.GetRestaurantImagePath(x);
                x.IsMainImage = x.Type == 1;
            });
        }

        private void FetchUserRestaurantDetails(RestaurantModel model, string requestUserId)
        {
            int.TryParse(requestUserId, out int userId);
            var userRestaurantChoice = _dbContext.UserRestaurantChoices.FirstOrDefault(c => c.UserId == userId && c.RestaurantId == model.Id);
            var userFavouriteRestaurant = _dbContext.UserFavouriteRestaurant.FirstOrDefault(f => f.UserId == userId && f.RestaurantId == model.Id);

            if (userRestaurantChoice != null)
                model.MyChoice = userRestaurantChoice.MyChoice;

            if (userFavouriteRestaurant != null)
            {
                UserFavouriteRestaurantModel userFavourite = new UserFavouriteRestaurantModel();
                model.UserFavouriteRestaurantList = new List<UserFavouriteRestaurantModel>();
                _mapper.Map(userFavouriteRestaurant, userFavourite);

                model.UserFavouriteRestaurantList.Add(userFavourite);
            }
        }

        private void FetchExtraProperties(RestaurantModel model, List<ResOpeningHourModel> resOpeningHours)
        {
            var resOpeningHoursList = resOpeningHours.Where(x => x.RestaurantId == model.Id).GroupBy(x => x.Day)
                                .Select(group => new EditResOpeningDayModel
                                {
                                    Weekday = group.Key,
                                    Shifts = group.Select(
                                    i => new EditResOpeningShiftModel
                                    {
                                        Closes = i.Closes,
                                        Day = i.Day,
                                        Id = i.Id,
                                        Opens = i.Opens,
                                        RestaurantId = i.RestaurantId,
                                        Session = i.Session
                                    })
                                }).ToList();

            model.OpeningHoursList = resOpeningHoursList;
            model.OpeningHours = FetchOpenningHours(model.Id, resOpeningHours);
            model.Images.ForEach(x =>
            {
                x.Url = _fileService.GetRestaurantImagePath(x);
                x.IsMainImage = x.Type == 1;
            });

            var resBadges = (from opt in _dbContext.Badge.Include(s => s.ResBadgeEntity)
                             where opt.ResBadgeEntity.Any(c => c.RestaurantId == model.Id)
                             select new ResBadgeModel
                             {
                                 IconId = opt.IconId,
                                 Id = opt.Id,
                                 Name = opt.Name
                             });

            if (resBadges != null)
                model.RestaurantBadges = resBadges.ToList();

            model.IsOpen = VerifyIsopen(model.Id, resOpeningHours);
        }

        private bool VerifyIsopen(int id, List<ResOpeningHourModel> resOpeningHours)
        {
            bool isOpen = false;

            try
            {
                var openningHours = resOpeningHours.FindAll(x => x.RestaurantId == id);

                if (openningHours.Count > 0)
                {
                    var euTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                    DateTime now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, euTimeZone);

                    //add 2 hours because GCP not give current time uisng timezone
                    var centralEuropeTime = now.AddHours(2).ToString("HH:mm", new CultureInfo("fr-FR"));

                    var dayOfWeeek = now.ToString("dddd", new CultureInfo("fr-FR")).ToLower();
                    var todayOpenningHours = openningHours.FindAll(x => x.Day.ToLower() == dayOfWeeek);

                    foreach (var openingHour in todayOpenningHours)
                    {
                        if (openingHour.Opens == "Fermé")
                        {
                            isOpen = false;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(openingHour.Closes) && !string.IsNullOrWhiteSpace(openingHour.Opens))
                            {
                                if (openingHour.Opens == "0:00")
                                    openingHour.Opens = "00:00";

                                if (openingHour.Closes == "0:00")
                                    openingHour.Closes = "11:59";

                                if (openingHour.Closes == "00:00")
                                    openingHour.Closes = "11:59";

                                TimeSpan.TryParse(openingHour.Opens, out TimeSpan openTime);
                                TimeSpan.TryParse(openingHour.Closes, out TimeSpan closeTime);

                                var fromTime = DateTime.Parse(openingHour.Opens);
                                var toTime = DateTime.Parse(openingHour.Closes);
                                var nowtime = DateTime.Parse(centralEuropeTime);

                                if (openTime > closeTime)
                                {
                                    toTime = toTime.AddDays(1);
                                }

                                if (nowtime > fromTime && nowtime < toTime)
                                {
                                    isOpen = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return isOpen;
        }

        private List<ResOpeningDayModel> FetchOpenningHours(int id, List<ResOpeningHourModel> resOpeningHours)
        {
            var openingHours = resOpeningHours.Where(x => x.RestaurantId == id).GroupBy(x => x.Day)
                               .Select(group => new ResOpeningDayModel
                               {
                                   Weekday = group.Key,
                                   Shifts = group.Select(
                                   i => new ResOpeningShiftModel { Name = i.Session, Duration = string.Format("{0}-{1}", i.Opens, i.Closes) })

                               }).ToList();

            return openingHours;
        }

        private bool checkSub(ICollection<ResSubcategoryEntity> resSubcategoryEntityList, int resId)
        {
            return resSubcategoryEntityList.Any(r => r.RestaurantId == resId);
        }

        private bool checkOpt(ICollection<ResBadgeEntity> resSubcategoryEntityList, int resId)
        {
            return resSubcategoryEntityList.Any(r => r.RestaurantId == resId);
        }

        private string SubCatName(string subCatId)
        {
            int.TryParse(subCatId, out int id);
            return _dbContext.SubCategorires.FirstOrDefault(r => r.Id == id).Name;
        }

        #endregion

    }

    public class SortType
    {
        public const string Asc = "asc";
        public const string Desc = "desc";
    }
}
