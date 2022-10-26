using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.ViewModels;
using EnterpriseApp.API.Models;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace EnterpriseApp.API.Helpers
{
    public class FileHelper
    {
        public FileHelper()
        {
        }

        internal static async Task<List<FileDetailModel>> SaveFile(CommonFileUploadFormModel fileUpload, IFileService fileService, AppSettings appSettings)
        {
            switch ((WellKnownFileUploadType)fileUpload.UploadType)
            {
                case WellKnownFileUploadType.None:
                    throw new Core.Exceptions.ApplicationDataException(EnterpriseApp.API.Core.Exceptions.StatusCode.ERROR_InvalidFileUpload);
                case WellKnownFileUploadType.Article:
                    break;
                case WellKnownFileUploadType.User:
                    break;
                default:
                    throw new Core.Exceptions.ApplicationDataException(EnterpriseApp.API.Core.Exceptions.StatusCode.ERROR_InvalidFileUpload);
            }

            List<FileDetailModel> fileList = new List<FileDetailModel>();

            if (fileUpload.File != null)
            {
                var formFile = fileUpload.File;

                var tempPath = Path.GetTempFileName();

                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(tempPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    string actualFileName = GetActualFileName(formFile.FileName);

                    FileOperationModel fileModel = new FileOperationModel
                    {
                        ActualFileName = actualFileName,
                        TempPath = tempPath,
                    };

                    bool isSavingSuccess = await fileService.SaveFileAsync(fileModel);

                    if (!isSavingSuccess)
                    {
                        var failedDetail = new FileDetailModel { File = new CommonFileUploadModel { Name = fileUpload.Name }, IsSuccessful = false };

                        //_logger.LogError("File Could not upload. File:{0} UserId:{1}", failedDetail.FileName, fileUpload.RegId);

                        fileList.Add(failedDetail);
                    }

                    CommonFileUploadModelRequest fileUploadInsertModel = new CommonFileUploadModelRequest
                    {
                        Description = fileUpload.Description,
                        Name = fileUpload.Name,
                        ToEnitityId = fileUpload.ToEnitityId,
                        FileName = actualFileName,
                        UploadType = fileUpload.UploadType,
                        Type = fileUpload.Type
                    };

                    var detail = await fileService.InsertFileData(fileUploadInsertModel);

                    fileList.Add(detail);
                }
            }

            return fileList;
        }

        public async static Task<T> UploadLocationData<T>(CommonFileUploadFormModel fileUpload) where T : class, new()
        {
            T items = null;

            try
            {
                if (fileUpload.File != null)
                {
                    var formFile = fileUpload.File;

                    var tempPath = Path.GetTempFileName();

                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream(tempPath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);

                            bool hasHeader = true;

                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                            using (var excelPack = new ExcelPackage(new FileInfo(tempPath)))
                            {
                                //Lets Deal with first worksheet.(You may iterate here if dealing with multiple sheets)
                                var ws = excelPack.Workbook.Worksheets["Sheet1"];

                                //Get all details as DataTable -because Datatable make life easy :)
                                DataTable excelasTable = new DataTable();

                                foreach (var firstRowCell in ws.Cells[2, 1, 1, ws.Dimension.End.Column])
                                {
                                    //Get colummn details
                                    if (!string.IsNullOrEmpty(firstRowCell.Text))
                                    {
                                        string columnName = firstRowCell.Text.Replace(" ", "_");

                                        excelasTable.Columns.Add(columnName);
                                    }
                                }

                                var startRow = hasHeader ? 3 : 2;

                                //Get row details
                                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                {
                                    var wsRow = ws.Cells[rowNum, 1, rowNum, excelasTable.Columns.Count];
                                    DataRow row = excelasTable.Rows.Add();
                                    foreach (var cell in wsRow)
                                    {
                                        string text = cell.Text;

                                        text = Regex.Replace(text, @"\d+: ", string.Empty, RegexOptions.IgnoreCase);

                                        row[cell.Start.Column - 1] = text;
                                    }
                                }

                                //Get everything as generics and let end user decides on casting to required type
                                var generatedType = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(excelasTable));

                                items = (T)Convert.ChangeType(generatedType, typeof(T));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return items;
        }

        #region Helper Methods

        public static string GetActualFileName(string fileName)
        {
            var dt = DateTime.Now;
            string actualName = dt.ToString("yyyyMMddhhmmss") + "_" + dt.Ticks.ToString() + Path.GetExtension(fileName);
            return actualName;
        }

        #endregion
    }
}
