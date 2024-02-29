using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PfcAPI.Infrastucture.DBContext;
using PfcAPI.Infrastucture.Interfaces;
using PfcAPI.Infrastucture.utility;
using PfcAPI.Model.Appointee;
using PfcAPI.Model.Company;
using PfcAPI.Model.DataAccess;
using PfcAPI.Model.DataTransfer.UAN.Response;
using PfcAPI.Model.RequestModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using static PfcAPI.Infrastucture.CommonEnum;

namespace PfcAPI.Infrastucture.Context
{

    public class FileContext : IFileContext
    {
        private readonly DbContextDB _dbContextClass;
        private static int _curRow = 1;
        private static int _curCol = 1;

        public FileContext(DbContextDB dbContextClass)
        {
            _dbContextClass = dbContextClass;
        }
        /// <summary>
        /// save excel file details in to db
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filepath"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public async Task<int> PostUploadedXSLfileAsync(string? fileName, string? filepath, int? companyid)
        {

            var _postobj = new UploadedXSLfile();
            _postobj.CompanyId = companyid;
            _postobj.FileName = fileName;
            _postobj.FilePath = filepath;
            _postobj.ActiveStatus = true;
            _postobj.CreatedBy = 1;
            _postobj.CreatedOn = DateTime.Now;

            _dbContextClass.UploadedXSLfile.Add(_postobj);
            await _dbContextClass.SaveChangesAsync();

            var fileId = _postobj.FileId;
            return fileId;
        }
        /// <summary>
        /// Genarate Datatable from uploaded excel file from Company or Admin User
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="fileData"></param>
        /// <returns>UploadedXSLfileDetails</returns>
        public Task<UploadedXSLfileDetailsResponse> GetDataTableFromxlsFile(int? CompanyId, IFormFile fileData, string? subfolder)
        {
            var _uploadedxslfiledetails = new UploadedXSLfileDetailsResponse();
            try
            {

                var slpitFileName = fileData.FileName?.Split('.');
                var _fileNameWithoutext = slpitFileName?.FirstOrDefault();
                var _ext = Path.GetExtension(fileData.FileName);
                var _fileName = $"{_fileNameWithoutext}{DateTime.Now.Ticks}{_ext}";


                string Cuurentpath = Directory.GetCurrentDirectory();
                string path = string.IsNullOrEmpty(subfolder) ? Path.Combine(Cuurentpath, "Uploads") : Path.Combine(Cuurentpath, "Uploads", subfolder);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                //Save the uploaded Excel file.
                string filePath = Path.Combine(path, _fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    fileData.CopyTo(stream);
                }
                //Create a new DataTable.
                DataTable dt = new DataTable();

                var fi = new FileInfo(filePath);
                using (var package = new ExcelPackage(fi))
                {
                    var workbook = package.Workbook;
                    var worksheet = workbook.Worksheets.First();

                    // get number of rows and columns in the sheet
                    int rows = worksheet.Dimension.Rows; // 20

                    int columns = worksheet.Dimension.Columns; // 7
                    // loop through the worksheet rows and columns
                    for (int i = 1; i <= rows; i++)
                    {
                        if (i > 1)
                        {
                            dt.Rows.Add();
                        }
                        for (int j = 1; j <= columns; j++)
                        {
                            string content = worksheet?.Cells[i, j]?.Value?.ToString() ?? string.Empty;
                            if (i == 1)
                            {

                                dt.Columns.Add(content);
                            }
                            else
                            {
                                dt.Rows[dt.Rows.Count - 1][j - 1] = content;
                            }

                            /* Do something ...*/
                        }
                    }
                    //  package.Save();
                }

                _uploadedxslfiledetails.FileName = _fileName;
                _uploadedxslfiledetails.FilePath = filePath;
                _uploadedxslfiledetails.dataTable = dt;

            }
            catch (Exception)
            {
                throw;
            }
            return Task.FromResult(_uploadedxslfiledetails);
        }
        /// <summary>
        /// validate data format of the uploaded data from excel file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<XSLfileDetailsListResponse> ValidateFromxlsFile(UploadedXSLfileDetailsResponse data)
        {
            var validateData = new XSLfileDetailsListResponse();
            DataTable ValidData = new DataTable();
            DataTable InValidData = new DataTable();
            bool isRemarksColumnAdd = false;
            if (data?.dataTable?.Rows != null)
            {
                foreach (var column in data.dataTable.Columns)
                {
                    ValidData.Columns.Add(column.ToString());
                    InValidData.Columns.Add(column.ToString());
                }

                //foreach (var (row, index, appointeeName, AppointeeEmailId, MobileNo, DateOfOffer, DateOfJoining, EPFWages)
                foreach (var (row, index, candidateId, companyName, appointeeName, AppointeeEmailId, MobileNo, DateOfJoining, IsPFverificationReq, lvl1Email, lvl2Email, lvl3Email)
                    in from DataRow row in data.dataTable.Rows
                       let rowindex = data.dataTable.Rows.IndexOf(row)
                       let index = rowindex + 1
                       let candidateId = ((string)row["Candidate ID"])?.Trim()
                       let companyName = ((string)row["Company Name"])?.Trim()
                       let appointeeName = ((string)row["Name"])?.Trim()
                       let MobileNo = ((string)row["Phone No"])?.Trim()
                       let AppointeeEmailId = ((string)row["EmailId"])?.Trim()
                       let DateOfJoining = ((string)row["Date Of Joining"])?.Trim()
                       let IsPFverificationReq = ((string)row["Fresher"])?.Trim()?.ToUpper()
                       let lvl1Email = ((string)row["level1 Email"])?.Trim()
                       let lvl2Email = ((string)row["level2 Email"])?.Trim()
                       let lvl3Email = ((string)row["level3 Email"])?.Trim()

                       select (row, index, candidateId, companyName, appointeeName, AppointeeEmailId, MobileNo, DateOfJoining, IsPFverificationReq, lvl1Email, lvl2Email, lvl3Email))
                {
                    string errormsg = string.Empty;
                    string msg = string.Empty;
                    bool isdataValid = true;
                    if (string.IsNullOrEmpty(candidateId) || string.IsNullOrEmpty(appointeeName) || !string.IsNullOrEmpty(IsPFverificationReq))
                    {
                        if (string.IsNullOrEmpty(companyName))
                        {
                            isdataValid = false;
                            var _Issue = "Company Name should not be blank.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {companyName}, Issue: {_Issue} " : $" Data: {companyName}, Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                        if (string.IsNullOrEmpty(appointeeName))
                        {
                            isdataValid = false;
                            var _Issue = "Name should not be blank.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {appointeeName}, Issue: {_Issue} " : $" Data: {appointeeName}, Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                    }
                    // bool isEmail = Regex.IsMatch(AppointeeEmailId, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                    bool isEmail = CommonUtility.IsEmailValidate(AppointeeEmailId);
                    if (!isEmail)
                    {
                        isdataValid = false;
                        var _Issue = "Email format is wrong.";
                        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {AppointeeEmailId}, Issue: {_Issue} " : $" Data: {AppointeeEmailId}, Issue: {_Issue} ";
                        validateData.InternalMessages.Add(msg);
                        errormsg += $"{msg}, ";
                    }

                    if (!string.IsNullOrEmpty(lvl1Email))
                    {
                        isEmail = CommonUtility.IsEmailValidate(lvl1Email);
                        if (!isEmail)
                        {
                            isdataValid = false;
                            var _Issue = "Leble1 Email format is wrong.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {lvl1Email}, Issue: {_Issue} " : $" Data: {lvl1Email}, Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                    }
                    if (!string.IsNullOrEmpty(lvl2Email))
                    {
                        isEmail = CommonUtility.IsEmailValidate(lvl2Email);
                        if (!isEmail)
                        {
                            isdataValid = false;
                            var _Issue = "Leble1 Email format is wrong.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {lvl2Email}, Issue: {_Issue} " : $" Data: {lvl2Email}, Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                    }
                    if (!string.IsNullOrEmpty(lvl3Email))
                    {
                        isEmail = CommonUtility.IsEmailValidate(lvl3Email);
                        if (!isEmail)
                        {
                            isdataValid = false;
                            var _Issue = "Leble1 Email format is wrong.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {lvl3Email}, Issue: {_Issue} " : $" Data: {lvl3Email}, Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                    }

                    bool isMobileNo = Regex.IsMatch(MobileNo, @"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)", RegexOptions.IgnoreCase);
                    if (!isMobileNo)
                    {
                        isdataValid = false;
                        var _Issue = "Mobile No format is wrong.";
                        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {MobileNo},  Issue: {_Issue}" : $" Data: {MobileNo},  Issue: {_Issue}";
                        validateData.InternalMessages.Add(msg);
                        errormsg += $"{msg}, ";
                    }


                    var Jngrdate = new DateTime();
                    bool joindateValidity = DateTime.TryParseExact(DateOfJoining, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Jngrdate);

                    if (!joindateValidity)
                    {
                        isdataValid = false;
                        var _Issue = "Joining date format(dd-MM-yyyy) is wrong.";
                        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {DateOfJoining},  Issue: {_Issue} " : $" Data: {DateOfJoining},  Issue: {_Issue} ";
                        validateData.InternalMessages.Add(msg);
                        errormsg += $"{msg}, ";
                    }
                    else
                    {
                        if (Jngrdate < DateTime.Now)
                        {
                            isdataValid = false;
                            var _Issue = "Joining date must be a future date.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {DateOfJoining},  Issue: {_Issue} " : $" Data: {DateOfJoining},  Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                    }
                    if (string.IsNullOrEmpty(IsPFverificationReq) || !(IsPFverificationReq == "YES" || IsPFverificationReq == "NO"))
                    {
                        isdataValid = false;
                        var _Issue = "Fresher should not be blank and value should be 'YES' or 'NO'";
                        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {IsPFverificationReq}, Issue: {_Issue} " : $" Data: {IsPFverificationReq}, Issue: {_Issue} ";
                        validateData.InternalMessages.Add(msg);
                        errormsg += $"{msg}, ";
                    }



                    if (isdataValid)
                    {
                        ValidData.Rows.Add(row.ItemArray);
                    }
                    else
                    {
                        if (!isRemarksColumnAdd)
                        {
                            isRemarksColumnAdd = true;
                            InValidData.Columns.Add("Remarks");
                        }
                        InValidData.Rows.Add(row.ItemArray);
                        int rowindex = InValidData.Rows.Count - 1;
                        InValidData.Rows[rowindex]["Remarks"] = errormsg;
                    }
                }
            }
            validateData.ValidXlsData = ValidData;
            validateData.InValidXlsData = InValidData;
            return validateData;
        }

        public async Task<XSLfileDetailsListResponse> ValidateUpdatexlsFile(UploadedXSLfileDetailsResponse data)
        {
            var validateData = new XSLfileDetailsListResponse();
            DataTable ValidData = new DataTable();
            DataTable InValidData = new DataTable();
            bool isRemarksColumnAdd = false;
            if (data?.dataTable?.Rows != null)
            {
                foreach (var column in data.dataTable.Columns)
                {
                    ValidData.Columns.Add(column.ToString());
                    InValidData.Columns.Add(column.ToString());
                }
                foreach (var (row, index, candidateId, appointeeName, DateOfJoining)//, AppointeeEmailId, MobileNo,companyName, IsPFverificationReq, lvl1Email, lvl2Email, lvl3Email)
                    in from DataRow row in data.dataTable.Rows
                       let rowindex = data.dataTable.Rows.IndexOf(row)
                       let index = rowindex + 1
                       let candidateId = ((string)row["Candidate ID"])?.Trim()
                       let DateOfJoining = ((string)row["Updated Date Of Joining"])?.Trim()
                       let appointeeName = ((string)row["Updated Name"])?.Trim()
                       //let AppointeeEmailId = ((string)row["Updated EmailId"])?.Trim()
                       //let companyName = ((string)row["Updated Company Name"])?.Trim()
                       //let MobileNo = ((string)row["Updated Phone No"])?.Trim()
                       //let IsPFverificationReq = ((string)row["Updated Fresher"])?.Trim()?.ToUpper()
                       //let lvl1Email = ((string)row["Updated level1 Email"])?.Trim()
                       //let lvl2Email = ((string)row["Updated level2 Email"])?.Trim()
                       //let lvl3Email = ((string)row["Updated level3 Email"])?.Trim()
                       where !(string.IsNullOrEmpty(candidateId))
                       select (row, index, candidateId, appointeeName, DateOfJoining))//, AppointeeEmailId, MobileNo, companyName, IsPFverificationReq, lvl1Email, lvl2Email, lvl3Email))
                {
                    string errormsg = string.Empty;
                    string msg = string.Empty;
                    bool isdataValid = true;
                    //bool isEmail = false;

                    //if (!string.IsNullOrEmpty(AppointeeEmailId))
                    //{
                    //    isEmail = CommonUtility.IsEmailValidate(AppointeeEmailId);
                    //    if (!isEmail)
                    //    {
                    //        isdataValid = false;
                    //        var _Issue = "Email format is wrong.";
                    //        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {AppointeeEmailId}, Issue: {_Issue} " : $" Data: {AppointeeEmailId}, Issue: {_Issue} ";
                    //        validateData.InternalMessages.Add(msg);
                    //        errormsg += $"{msg}, ";
                    //    }
                    //}
                    //if (!string.IsNullOrEmpty(lvl1Email))
                    //{
                    //    isEmail = CommonUtility.IsEmailValidate(lvl1Email);
                    //    if (!isEmail)
                    //    {
                    //        isdataValid = false;
                    //        var _Issue = "Leble1 Email format is wrong.";
                    //        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {lvl1Email}, Issue: {_Issue} " : $" Data: {lvl1Email}, Issue: {_Issue} ";
                    //        validateData.InternalMessages.Add(msg);
                    //        errormsg += $"{msg}, ";
                    //    }
                    //}
                    //if (!string.IsNullOrEmpty(lvl2Email))
                    //{
                    //    isEmail = CommonUtility.IsEmailValidate(lvl2Email);
                    //    if (!isEmail)
                    //    {
                    //        isdataValid = false;
                    //        var _Issue = "Leble1 Email format is wrong.";
                    //        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {lvl2Email}, Issue: {_Issue} " : $" Data: {lvl2Email}, Issue: {_Issue} ";
                    //        validateData.InternalMessages.Add(msg);
                    //        errormsg += $"{msg}, ";
                    //    }
                    //}
                    //if (!string.IsNullOrEmpty(lvl3Email))
                    //{
                    //    isEmail = CommonUtility.IsEmailValidate(lvl3Email);
                    //    if (!isEmail)
                    //    {
                    //        isdataValid = false;
                    //        var _Issue = "Leble1 Email format is wrong.";
                    //        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {lvl3Email}, Issue: {_Issue} " : $" Data: {lvl3Email}, Issue: {_Issue} ";
                    //        validateData.InternalMessages.Add(msg);
                    //        errormsg += $"{msg}, ";
                    //    }
                    //}
                    //if (!string.IsNullOrEmpty(MobileNo))
                    //{
                    //    bool isMobileNo = Regex.IsMatch(MobileNo, @"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)", RegexOptions.IgnoreCase);
                    //    if (!isMobileNo)
                    //    {
                    //        isdataValid = false;
                    //        var _Issue = "Mobile No format is wrong.";
                    //        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {MobileNo},  Issue: {_Issue}" : $" Data: {MobileNo},  Issue: {_Issue}";
                    //        validateData.InternalMessages.Add(msg);
                    //        errormsg += $"{msg}, ";
                    //    }
                    //}

                    if (!string.IsNullOrEmpty(DateOfJoining))
                    {
                        var Jngrdate = new DateTime();
                        bool joindateValidity = DateTime.TryParseExact(DateOfJoining, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Jngrdate);

                        if (!joindateValidity)
                        {
                            isdataValid = false;
                            var _Issue = "Joining date format(dd-MM-yyyy) is wrong.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {DateOfJoining},  Issue: {_Issue} " : $" Data: {DateOfJoining},  Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                        else
                        {
                            if (Jngrdate < DateTime.Now)
                            {
                                isdataValid = false;
                                var _Issue = "Joining date must be a future date.";
                                msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {DateOfJoining},  Issue: {_Issue} " : $" Data: {DateOfJoining},  Issue: {_Issue} ";
                                validateData.InternalMessages.Add(msg);
                                errormsg += $"{msg}, ";
                            }
                        }
                    }
                    //if (!string.IsNullOrEmpty(IsPFverificationReq) && (!(IsPFverificationReq == "YES" || IsPFverificationReq == "NO")))
                    //{
                    //    isdataValid = false;
                    //    var _Issue = "Fresher value should be 'YES' or 'NO'";
                    //    msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {IsPFverificationReq}, Issue: {_Issue} " : $" Data: {IsPFverificationReq}, Issue: {_Issue} ";
                    //    validateData.InternalMessages.Add(msg);
                    //    errormsg += $"{msg}, ";
                    //}


                    if (isdataValid)
                    {
                        ValidData.Rows.Add(row.ItemArray);
                    }
                    else
                    {
                        if (!isRemarksColumnAdd)
                        {
                            isRemarksColumnAdd = true;
                            InValidData.Columns.Add("Remarks");
                        }
                        InValidData.Rows.Add(row.ItemArray);
                        int rowindex = InValidData.Rows.Count - 1;
                        InValidData.Rows[rowindex]["Remarks"] = errormsg;
                    }
                }
            }
            validateData.ValidXlsData = ValidData;
            validateData.InValidXlsData = InValidData;
            return validateData;
        }

        [Obsolete]
        public async Task<string> SaveDataTabletofile(DataTable data, string category, ValidationType type)
        {
            //string fileName = $"{category}_{Guid.NewGuid():N}.xlsx";
            string cuurentpath = Directory.GetCurrentDirectory();
            string currDate = DateTime.Now.ToShortDateString();
            string subfolder = string.Empty;
            switch (type)
            {
                case ValidationType.Invalid:
                    subfolder = "ErrorDownLoad";
                    break;
                case ValidationType.Duplicate:
                    subfolder = "DuplicateDownload";
                    break;
                case ValidationType.ApiCount:
                    subfolder = "ApiCount";
                    break;
                case ValidationType.AppointeeCount:
                    subfolder = "AppointeeCount";
                    break;
                case ValidationType.Critical1week:
                    subfolder = "Critical";
                    break;
                case ValidationType.Critical2week:
                    subfolder = "Critical";
                    break;
                case ValidationType.NoLinkSent:
                    subfolder = "NoLinkSent";
                    break;
                case ValidationType.NoResponse:
                    subfolder = "NoResponse";
                    break;
                case ValidationType.Processing:
                    subfolder = "Processing";
                    break;
                default:
                    subfolder = "Default";
                    break;

            }
            string fileName = $"{category}_{subfolder}_{Guid.NewGuid():N}.xlsx";
            string path = Path.Combine(cuurentpath, subfolder, currDate);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            //Save the uploaded Excel file.
            string filePath = Path.Combine(path, fileName);

            using (ExcelPackage pck = new ExcelPackage(filePath))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("AppointeeList");

                ws.Cells["A1"].LoadFromDataTable(data, true);
                ws.Cells["A1"].Style.WrapText = true;
                pck.Save();
            }
            return filePath;
        }

        public async Task<Filedata> GenerateDataTableTofile(DataTable data, string category, ValidationType type)
        {
            DateTime _currDate = DateTime.Now;
            string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";

            string subfolder = string.Empty;
            switch (type)
            {
                case ValidationType.Invalid:
                    subfolder = "Invalid";
                    break;
                case ValidationType.Duplicate:
                    subfolder = "Duplicate";
                    break;
                case ValidationType.ApiCount:
                    subfolder = "ApiCount";
                    break;
                case ValidationType.AppointeeCount:
                    subfolder = "AppointeeCount";
                    break;
                case ValidationType.Critical1week:
                    subfolder = "Critical";
                    break;
                case ValidationType.Critical2week:
                    subfolder = "Critical";
                    break;
                case ValidationType.NoLinkSent:
                    subfolder = "NoLinkSent";
                    break;
                case ValidationType.NoResponse:
                    subfolder = "NoResponse";
                    break;
                case ValidationType.Processing:
                    subfolder = "Processing";
                    break;
                default:
                    subfolder = "Default";
                    break;

            }
            string fileName = $"{subfolder}_{category}_{_currDateString}.xlsx";
            var fileData = CommonUtility.ExportFromDataTableToExcel(data, "AppointeeList", string.Empty);
            //string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string contentType = "xlsx";
            var _Filedata = new Filedata() { FileData = fileData, FileName = fileName, FileType = contentType };
            return _Filedata;
        }

        /// <summary>
        /// Get Appointee Uploaded File detail from specefic path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>byte[]</returns>
        //public async Task<byte[]?> GetFileDataAsync(string filePath)
        //{

        //    //... code for validation and get the file
        //    if (Path.Exists(filePath))
        //    {
        //        var provider = new FileExtensionContentTypeProvider();
        //        if (!provider.TryGetContentType(filePath, out var contentType))
        //        {
        //            contentType = "application/octet-stream";
        //        }

        //        var bytes = await File.ReadAllBytesAsync(filePath);
        //        return bytes;
        //    }

        // throw new NotImplementedException();
        //return null;
        //}
        /// <summary>
        /// Appointee Uploaded file remove and database file status change
        /// </summary>
        /// <param name="appointeeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task updateDocFile(int appointeeId, int userId)
        {
            //... code for validation and get the file

            var _prevdocDetails = await _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true).ToListAsync();
            _prevdocDetails.ForEach(x =>
            {
                x.ActiveStatus = false;
                x.UpdatedOn = DateTime.Now;
                x.UpdatedBy = userId;

            });
            await _dbContextClass.SaveChangesAsync();

            foreach (var obj in _prevdocDetails)
            {
                var file_path = RemoveDocFile(obj.UploadPath);
            }
        }
        /// <summary>
        /// Remove file from Path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool RemoveDocFile(string fileName)
        {

            if (fileName != null || fileName != string.Empty)
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);

            }
            return true;
        }

        /// <summary>
        /// Appointee uploaded file save db
        /// </summary>
        /// <param name="AppointeeDetails"></param>
        /// <returns></returns>
        public async Task postappointeeUploadedFiles(AppointeeFileDetailsRequest AppointeeDetails)
        {
            //try
            //{
            if (AppointeeDetails != null)
            {
                var fileUploaded = AppointeeDetails.FileDetails;
                var _UploadDetails = new List<AppointeeUploadDetails>();
                var _appointeeId = AppointeeDetails?.AppointeeId;
                if (fileUploaded?.Count > 0)
                {
                    var _prevDocList = _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(_appointeeId) & x.ActiveStatus == true).ToList();
                    if (!string.IsNullOrEmpty(AppointeeDetails.FileUploaded ?? string.Empty))
                    {

                        var _Uploadedfiledetails = JsonConvert.DeserializeObject<List<FileUploadDataModel>>(AppointeeDetails.FileUploaded).ToList() ?? new List<FileUploadDataModel>();
                        //foreach ((IFormFile obj, string fileName, string mimetype, FileUploadDataModel filedetails, string path) in from obj in fileUploaded
                        //                                                             let fileName = $"{DateTime.Now.Ticks}_{obj.FileName}"
                        //                                                             let fileLength = obj.Length
                        //                                                             let mimetype = obj.ContentType?.ToLower()
                        //                                                             let Cuurentpath = Directory.GetCurrentDirectory()
                        //                                                             let filedetails = _Uploadedfiledetails?.FirstOrDefault(x => x.fileName == obj.FileName & x.fileLength == fileLength & x.isFileUploaded == true)
                        //                                                             where filedetails != null
                        //                                                             let path = Path.Combine(Cuurentpath, "AppointeeUploads", AppointeeDetails.AppointeeCode, filedetails?.uploadTypeAlias ?? "")
                        //                                                             select (obj, fileName, mimetype, filedetails, path))
                        //{

                        foreach (IFormFile obj in fileUploaded)
                        {
                            //IFormFile obj = fileobj;
                            string fileName = $"{DateTime.Now.Ticks}_{obj?.FileName}";
                            var fileLength = obj?.Length;
                            string? mimetype = obj?.ContentType?.ToLower();
                            string Cuurentpath = Directory.GetCurrentDirectory();
                            FileUploadDataModel filedetails = _Uploadedfiledetails?.FirstOrDefault(x => x.fileName == obj.FileName & x.fileLength == fileLength & x.isFileUploaded == true);
                            if (filedetails != null)
                            {
                                string path = Path.Combine(Cuurentpath, "AppointeeUploads", AppointeeDetails.AppointeeCode, filedetails?.uploadTypeAlias ?? "");

                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                //Save the uploaded file.
                                string _filePath = Path.Combine(path, fileName);
                                using (FileStream stream = new FileStream(_filePath, FileMode.Create))
                                {
                                    obj.CopyTo(stream);
                                }
                                if (Path.Exists(_filePath))
                                {
                                    var uploaddata = new AppointeeUploadDetails();
                                    uploaddata.AppointeeId = AppointeeDetails.AppointeeId;
                                    uploaddata.UploadTypeId = filedetails.uploadTypeId;
                                    uploaddata.UploadTypeCode = filedetails.uploadTypeAlias;
                                    uploaddata.FileName = fileName;
                                    uploaddata.UploadPath = _filePath;
                                    uploaddata.IsPathRefered = CheckType.yes;
                                    uploaddata.MimeType = mimetype;
                                    uploaddata.ActiveStatus = true;
                                    uploaddata.CreatedBy = AppointeeDetails.UserId;
                                    uploaddata.CreatedOn = DateTime.Now;
                                    _UploadDetails.Add(uploaddata);

                                    // start remove prev file
                                    var _prevdocDetails = _prevDocList.Where(x => x.UploadTypeId == filedetails.uploadTypeId).FirstOrDefault();
                                    if (_prevdocDetails != null)
                                    {
                                        _prevdocDetails.ActiveStatus = false;
                                        _prevdocDetails.UpdatedOn = DateTime.Now;
                                        _prevdocDetails.UpdatedBy = AppointeeDetails.UserId;
                                        // await _dbContextClass.SaveChangesAsync();
                                        var file_removed = RemoveDocFile(_prevdocDetails.UploadPath);
                                    }
                                    // end remove prev file
                                }
                            }
                        }
                    }
                    if (_UploadDetails.Count > 0)
                    {
                        _dbContextClass.AppointeeUploadDetails.AddRange(_UploadDetails);
                        //await _dbContextClass.SaveChangesAsync();
                    }
                    await _dbContextClass.SaveChangesAsync();
                }
            }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task postappointeeDataInFiles(AppointeeDataSaveInFilesRequset UploadDetails)
        {
            //try
            //{
            if (UploadDetails != null)
            {

                var _appointeeId = UploadDetails?.AppointeeId;

                var _uploadFileType = await _dbContextClass.UploadTypeMaster.Where(x => x.UploadTypeCode.Equals(UploadDetails.FileTypeAlias) & x.ActiveStatus == true).FirstOrDefaultAsync();
                var _prevDocList = _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(_appointeeId) & x.ActiveStatus == true).ToList();
                if (!string.IsNullOrEmpty(UploadDetails.FileUploaded ?? string.Empty))
                {

                    if (!string.IsNullOrEmpty(UploadDetails?.FileUploaded))
                    {
                        var Cuurentpath = Directory.GetCurrentDirectory();
                        var path = Path.Combine(Cuurentpath, "AppointeeUploads", UploadDetails.AppointeeCode, UploadDetails?.FileTypeAlias ?? "");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //Save the uploaded file.
                        var _fileName = string.Concat(_uploadFileType.UploadTypeName, UploadDetails.mimetype);
                        string _filePath = Path.Combine(path, _fileName);

                        if (UploadDetails.FileTypeAlias == FileTypealias.PFPassbook)
                        {
                            File.WriteAllText(_filePath, UploadDetails.FileUploaded);
                            //var PassBookResponse = JsonConvert.DeserializeObject<GetUanPassbookResponse>(UploadDetails.FileUploaded);
                            //using (StreamWriter file = File.CreateText(@"D:\path.txt"))
                            //{
                            //    JsonSerializer serializer = new JsonSerializer();
                            //    //serialize object directly into file stream
                            //    serializer.Serialize(file, PassBookResponse);
                            //}
                        }
                        else
                        {
                            using (StreamWriter file = File.CreateText(_filePath))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                //serialize object directly into file stream
                                serializer.Serialize(file, UploadDetails.FileUploaded);
                            }
                        }

                        //using (FileStream stream = new FileStream(_filePath, FileMode.Create))
                        //{
                        //    obj.CopyTo(stream);
                        //}
                        if (Path.Exists(_filePath))
                        {
                            var uploaddata = new AppointeeUploadDetails();
                            uploaddata.AppointeeId = UploadDetails.AppointeeId;
                            uploaddata.UploadTypeId = _uploadFileType.UploadTypeId;
                            uploaddata.UploadTypeCode = _uploadFileType.UploadTypeCode;
                            uploaddata.FileName = _uploadFileType.UploadTypeName;
                            uploaddata.UploadPath = _filePath;
                            uploaddata.IsPathRefered = CheckType.yes;
                            uploaddata.MimeType = UploadDetails.mimetype;
                            uploaddata.ActiveStatus = true;
                            uploaddata.CreatedBy = UploadDetails.UserId;
                            uploaddata.CreatedOn = DateTime.Now;
                            //_UploadDetails.Add(uploaddata);

                            _dbContextClass.AppointeeUploadDetails.Add(uploaddata);
                            // start remove prev file
                            var _prevdocDetails = _prevDocList.Where(x => x.UploadTypeId == _uploadFileType.UploadTypeId).FirstOrDefault();
                            if (_prevdocDetails != null)
                            {
                                _prevdocDetails.ActiveStatus = false;
                                _prevdocDetails.UpdatedOn = DateTime.Now;
                                _prevdocDetails.UpdatedBy = UploadDetails.UserId;
                                await _dbContextClass.SaveChangesAsync();
                                var file_removed = RemoveDocFile(_prevdocDetails.UploadPath);
                            }
                            // end remove prev file
                        }

                    }
                }
                //if (_UploadDetails.Count > 0)
                //{

                //}
            }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private static void SetCurrentCell(int row, int col)
        {
            _curRow = row;
            _curCol = col;
        }
        private static void SetNextValue(ExcelWorksheet worksheet, object val)
        {
            worksheet.Cells[_curRow, _curCol].Value = val;
            _curCol++;
        }
        public async Task<FilekDownloadDetails> DownloadPassbook(int appointeeId, int userId)
        {
            //byte[]? bytes;
            var fileDetails = new FilekDownloadDetails();
            var _appointeedetails = await _dbContextClass.AppointeeDetails.Where(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true).FirstOrDefaultAsync();
            string fileName = $"{_appointeedetails?.AppointeeName}_{Guid.NewGuid()}_Pfpassbook.xlsx";
            string cuurentpath = Directory.GetCurrentDirectory();
            DateTime _currDate = DateTime.Now;
            string currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
            //string currDate = DateTime.Now.ToShortDateString();
            string filePath = string.Empty;
            Surepass_GetUanPassbookResponse PassBookResponse = new Surepass_GetUanPassbookResponse();
            var _prevexcelDocList = await _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true & x.UploadTypeCode == FileTypealias.PFPassbookExcel).ToListAsync();
            if (_prevexcelDocList?.Count == 0)
            {
                var _DocList = await _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true & x.UploadTypeCode == FileTypealias.PFPassbook).FirstOrDefaultAsync();
                //string subfolder = (Int32)type == (Int32)ValidationType.Duplicate ? "DuplicateDownload" : (Int32)type == (Int32)ValidationType.Invalid ? "ErrorDownLoad" : "";
                if (_DocList != null)
                {
                    string path = _DocList.UploadPath;
                    var _folderpath = Path.Combine(cuurentpath, "Downloads", FileTypealias.PFPassbook);
                    filePath = Path.Combine(_folderpath, fileName);

                    if (File.Exists(path))
                    {
                        // Read entire text file content in one string
                        string _passbookdata = File.ReadAllText(path);
                        PassBookResponse = JsonConvert.DeserializeObject<Surepass_GetUanPassbookResponse>(_passbookdata);
                        if (PassBookResponse?.data != null)
                        {
                            savePassbookexcelFile(PassBookResponse, _folderpath, filePath);

                        }
                    }
                    if (Path.Exists(filePath))
                    {


                        var _uploadFileType = await _dbContextClass.UploadTypeMaster.Where(x => x.UploadTypeCode.Equals(FileTypealias.PFPassbookExcel) & x.ActiveStatus == true).FirstOrDefaultAsync();
                        var uploaddata = new AppointeeUploadDetails();
                        uploaddata.AppointeeId = appointeeId;
                        uploaddata.UploadTypeId = _uploadFileType.UploadTypeId;
                        uploaddata.UploadTypeCode = _uploadFileType.UploadTypeCode;
                        uploaddata.FileName = _uploadFileType.UploadTypeName;
                        uploaddata.UploadPath = filePath;
                        uploaddata.IsPathRefered = CheckType.yes;
                        uploaddata.MimeType = "xlsx";
                        uploaddata.ActiveStatus = true;
                        uploaddata.CreatedBy = userId;
                        uploaddata.CreatedOn = _currDate;
                        //_UploadDetails.Add(uploaddata);

                        _dbContextClass.AppointeeUploadDetails.Add(uploaddata);
                        await _dbContextClass.SaveChangesAsync();
                    }
                }
            }
            else
            {
                filePath = _prevexcelDocList?.LastOrDefault()?.UploadPath ?? string.Empty;
            }

            var bytes = !string.IsNullOrEmpty(filePath) ? await File.ReadAllBytesAsync(filePath) : null;
            string DownloadfileName = $"{_appointeedetails?.AppointeeName}_Pfpassbook_{currDateString}.xlsx";
            fileDetails.FileData = bytes;
            fileDetails.FileName = DownloadfileName;
            fileDetails.FileExtention = "xlsx";
            return fileDetails;
            //  return filePath;
        }
        public async Task<FilekDownloadDetails> DownloadTrustPassbook(int appointeeId, int userId)
        {
            //byte[]? bytes;
            var fileDetails = new FilekDownloadDetails();
            var _appointeedetails = await _dbContextClass.AppointeeDetails.Where(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true).FirstOrDefaultAsync();

            string cuurentpath = Directory.GetCurrentDirectory();
            DateTime _currDate = DateTime.Now;
            string currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
            string fileName = $"{_appointeedetails?.AppointeeName}_TrustPassbook_{currDateString}.xlsxsx";
            string filePath = string.Empty;
            string fileExt = string.Empty;
            Surepass_GetUanPassbookResponse PassBookResponse = new Surepass_GetUanPassbookResponse();
            var _docList = await _dbContextClass.AppointeeUploadDetails.Where(x => x.AppointeeId.Equals(appointeeId) & x.ActiveStatus == true & x.UploadTypeCode == FileTypealias.PFPassbookTrust).ToListAsync();
            if (_docList?.Count > 0)
            {
                filePath = _docList?.LastOrDefault()?.UploadPath ?? string.Empty;
                fileExt = _docList?.LastOrDefault()?.MimeType ?? string.Empty;

            }
            var bytes = !string.IsNullOrEmpty(filePath) ? await File.ReadAllBytesAsync(filePath) : null;

            fileDetails.FileData = bytes;
            fileDetails.FileName = fileName;
            fileDetails.FileExtention = fileExt;

            return fileDetails;
            //  return filePath;
        }
        private static void savePassbookexcelFile(Surepass_GetUanPassbookResponse PassBookResponse, string folderPath, string filePath)
        {

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var companydetails = PassBookResponse?.data?.companies.Values.ToList();
            var _memberName = PassBookResponse?.data?.full_name;
            //var _memberId = PassBookResponse?.data?.;
            var _dob = PassBookResponse?.data?.dob;
            var _uan = PassBookResponse?.data?.pf_uan;
            using (ExcelPackage excelPackage = new ExcelPackage(filePath))
            {
                int index = 0;
                foreach (var obj in companydetails)
                {
                    //var passbookdt = CommonUtility.ToDataTable(obj.passbook);
                    var sheetname = $"sheet_{index}_{obj?.company_name?.ToString() ?? "default"}";
                    //create an instance of the the first sheet in the loaded file
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(sheetname);
                    //add some data
                    var _memberId = obj?.passbook?.FirstOrDefault()?.member_id;
                    worksheet.Cells["A1"].Value = "Employees Provident Fund Organization";
                    worksheet.Cells[1, 1, 1, 5].Merge = true;
                    worksheet.Cells["A3"].Value = "Member Passbook";
                    worksheet.Cells["A3"].Style.Font.Size = 16;
                    worksheet.Cells["A3"].Style.Font.Bold = true;
                    using (var range = worksheet.Cells[1, 1, 1, 5])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                        range.Style.Font.Size = 18;
                    }

                    worksheet.Cells["A5"].Value = "Establishment ID / Name";
                    worksheet.Cells["A6"].Value = "Member ID / Name";
                    worksheet.Cells["A7"].Value = "DOB";
                    worksheet.Cells["A8"].Value = "UAN";

                    worksheet.Cells["B5"].Value = string.Concat(obj?.establishment_id, "/", obj?.company_name);
                    worksheet.Cells["B6"].Value = string.Concat(_memberId, "/", _memberName);
                    worksheet.Cells["B7"].Value = _dob;
                    worksheet.Cells["B8"].Value = _uan;
                    worksheet.Cells["A10"].Value = "EPF Passbook  ";
                    using (var range = worksheet.Cells[5, 1, 10, 5])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Size = 14;
                    }

                    SetCurrentCell(13, 1);
                    //Add the headers
                    SetNextValue(worksheet, "Wage Month");
                    //SetNextValue(worksheet, "Particulars");
                    SetNextValue(worksheet, "Date of Credit / Withdrawal");
                    ////SetNextValue(worksheet, "Transaction Type");
                    //SetNextValue(worksheet, "EPF Wages");
                    //SetNextValue(worksheet, "EPS Wages");
                    ////SetNextValue(worksheet, "Employee Share");
                    ////SetNextValue(worksheet, "Employer Share");
                    ////SetNextValue(worksheet, "Pension Contribution");


                    //Format the headers
                    using (var range = worksheet.Cells[_curRow, 1, _curRow, _curCol - 1])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                        range.Style.Font.Color.SetColor(Color.Black);
                        range.Style.Font.Size = 18;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    }

                    var datatablerow = _curRow + 1;
                    for (var i = 0; i < obj?.passbook?.Count(); i++)
                    {
                        SetCurrentCell(i + datatablerow, 1);
                        var _wagemonth = string.Concat(obj?.passbook[i]?.month, "-", obj?.passbook[i]?.year);
                        SetNextValue(worksheet, _wagemonth);
                        //SetNextValue(worksheet, obj?.passbook[i]?.description);
                        SetNextValue(worksheet, obj?.passbook[i]?.approved_on);
                        ////SetNextValue(worksheet, obj?.passbook[i]?.transaction_category);
                        ////SetNextValue(worksheet, obj?.passbook[i]?.employee_share);
                        ////SetNextValue(worksheet, obj?.passbook[i]?.employer_share);
                        ////SetNextValue(worksheet, obj?.passbook[i]?.pension_share);
                    }

                    worksheet.Cells.AutoFitColumns(10, 100);
                    SetCurrentCell(_curRow + 1, 5);

                    ////SetNextValue(worksheet, obj?.employee_total);
                    ////SetNextValue(worksheet, obj?.employer_total);
                    ////SetNextValue(worksheet, obj?.pension_total);
                    worksheet.Cells[_curRow, 1, _curRow, _curCol - 1].Style.Font.Bold = true;
                    index++;
                }

                excelPackage.Workbook.Properties.Title = "Passbook Details";
                excelPackage.Workbook.Properties.Author = "Veridata";
                excelPackage.Workbook.Properties.Company = "Veridata";
                //save the changes
                excelPackage.Save();
            }

        }

    }
}
