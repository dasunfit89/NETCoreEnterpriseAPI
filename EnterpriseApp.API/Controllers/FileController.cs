using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.ViewModels;
using EnterpriseApp.API.Models;
using EnterpriseApp.API.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EnterpriseApp.API.Controllers
{
    [Route("api/[controller]")]
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;

        public FileController(
             IApplicationDataService articleService,
             IUserService userService,
             IFileService fileService,
             IOptions<AppSettings> appSettings) : base(articleService, userService, appSettings)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Download restaurant file asynchronously
        /// </summary>
        /// <param name="resId"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Images/{fileId}")]
        public async Task<IActionResult> DownloadFileAsync(string fileId)
        {
            IActionResult fileStreamResult = null;

            try
            {
                fileStreamResult = await GetFileAsync(fileId);
            }
            catch (Exception ex)
            {
                throw new ApplicationDataException(Core.Exceptions.StatusCode.ERROR_FileNotFound, ex.Message);
            }

            return fileStreamResult;
        }

        #region Helper Methods

        private async Task<IActionResult> GetFileAsync(string fileId)
        {
            FileDownloadModel model = await _fileService.DownloadFileAsync(fileId);

            var memoryStream = new MemoryStream();

            using (var stream = model.Stream ?? model.FileStream)
            {
                await stream.CopyToAsync(memoryStream);
            }

            memoryStream.Position = 0;

            return File(memoryStream, model.ContentType, model.FileName);
        }

        #endregion
    }
}
