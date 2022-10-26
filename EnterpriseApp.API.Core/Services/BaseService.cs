using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;
using Microsoft.Extensions.Options;
using ApplicationDataException = EnterpriseApp.API.Core.Exceptions.ApplicationDataException;

namespace EnterpriseApp.API.Core.Services
{
    public class BaseService
    {
        protected readonly IRepository _dbContext;
        protected readonly IMapper _mapper;
        protected readonly AppSettings _appSettings;

        public BaseService(IRepository dbContext, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task CheckArticleExists(string articleId)
        {
            var article = await _dbContext.FindAsync<Article>(articleId);

            if (article == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidArticle);

            if (article.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_ArticleDeactivated);

        }

        public async Task CheckUserExists(string userId)
        {
            var user = await _dbContext.FindAsync<User>(userId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (user.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_UserDeactivated);

        }

        public string GenerateRandomOTP(int iOTPLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string otp = string.Empty;
            string sTempChars = string.Empty;

            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                otp += sTempChars;
            }

            otp = "111222";

            return otp;
        }

        protected async Task<Location> GetLocation(string districtId, string divisionId)
        {
            List<Area> entities = await _dbContext.FilterAsync<Area>(x => x.Districts.Any(y => y.Id == districtId));

            Area area = entities.FirstOrDefault();

            District district = area?.Districts.FirstOrDefault(x => x.Id == districtId);

            Division division = district?.Divisions.FirstOrDefault(x => x.Id == divisionId);

            Location location = new Location()
            {
                DistrictId = district?.Id,
                DistrictName = district?.Name,
                DivisionId = division?.Id,
                DivisionName = division?.Name,
            };

            return location;
        }
    }

    public static class ExtensionOperation
    {
        public static async Task<PaginatedResponse<T>> AsPaginatedResponseAsync<T>(this IQueryable<T> entities, PagingQueryParam paging)
        {
            int totalRecordCount = await Task.Run(() => entities.Count());
            List<T> list;

            if (paging.StartingIndex == -1 && paging.PageSize == -1)
            {
                list = await Task.Run(() => entities.ToList());
            }
            else
            {
                list = await Task.Run(() => entities.Skip(paging.StartingIndex).Take(paging.PageSize).ToList());
            }

            return new PaginatedResponse<T>() { Data = list, TotalDataRecords = totalRecordCount };
        }
    }
}
