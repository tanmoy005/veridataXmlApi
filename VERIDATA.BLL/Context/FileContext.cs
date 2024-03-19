﻿using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Notification.Provider;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Request;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using VERIDATA.Model.Table.Master;
using VERIDATA.Model.Table.Public;
using static VERIDATA.BLL.utility.CommonEnum;

namespace VERIDATA.BLL.Context
{

    public class FileContext : IFileContext
    {
        private readonly IAppointeeDalContext _appointeeContext;
        private readonly IUserContext _userContext;
        private readonly IEmailSender _emailSender;
        private readonly IWorkFlowDalContext _workFlowDalContext;

        public FileContext(IAppointeeDalContext appointeeContext, IUserContext userContext, IEmailSender emailSender, IWorkFlowDalContext workFlowDalContext)
        {
            //_dbContextClass = dbContextClass;
            _appointeeContext = appointeeContext;
            _userContext = userContext;
            _emailSender = emailSender;
            _workFlowDalContext = workFlowDalContext;
        }

        public async Task<UploadedxlsRawFileDataResponse> UploadAppointeexlsFile(CompanyFileUploadRequest fileDetails)
        {
            int _duplicateUserCount = 0;
            int _InvalidUserCount = 0;
            List<Filedata> _FileDataList = new();
            UserDetailsResponse userData = await _userContext.getUserDetailsAsyncbyId(fileDetails.UserId);
            List<RawFileDataDetailsResponse> RawFileData = new();
            UploadedXSLfileDetailsResponse resdata = await GetDataTableFromxlsFile(fileDetails.CompanyId, fileDetails.FileDetails, "NewUser");
            XSLfileDetailsListResponse validateresdata = ValidateFromxlsFile(resdata);

            if (validateresdata.InValidXlsData?.Rows?.Count > 0)
            {
                _InvalidUserCount = validateresdata.InValidXlsData?.Rows?.Count ?? 0;
                Filedata _fileDetails = GenerateDataTableTofile(validateresdata.InValidXlsData, "Error", CommonEnum.ValidationType.Invalid);
                Filedata _InvalidFiledata = new() { FileData = _fileDetails.FileData, FileName = "Invalid_Data", FileType = _fileDetails.FileType };
                _FileDataList.Add(_InvalidFiledata);
                List<Filedata> attachtData = new() { _InvalidFiledata };
                await _emailSender.SendMailWithAttachtment(userData.UserName, userData.EmailId, attachtData, CommonEnum.ValidationType.Invalid);
                //send mail to companyUser with error data file attached
            }

            if (validateresdata.ValidXlsData?.Rows?.Count > 0)
            {
                //GetFileId
                int _Fileid = await _workFlowDalContext.PostUploadedXSLfileAsync(resdata.FileName, resdata.FilePath, fileDetails.CompanyId);
                List<AppointeeBasicInfo> rawListData = new();

                foreach ((DataRow row, AppointeeBasicInfo _rawData) in
                from DataRow row in validateresdata.ValidXlsData?.Rows
                let _rawData = new AppointeeBasicInfo()
                select (row, _rawData))
                {
                    string? candidateID = ((string)row["Candidate ID"])?.Trim();
                    string? appointeeName = ((string)row["Name"])?.Trim();
                    string? AppointeeEmailId = ((string)row["EmailId"])?.Trim();
                    string? CompanyName = ((string)row["Company Name"])?.Trim();
                    if (!(string.IsNullOrEmpty(AppointeeEmailId) && string.IsNullOrEmpty(appointeeName)))
                    {
                        _rawData.CandidateID = candidateID;
                        _rawData.AppointeeName = appointeeName;
                        _rawData.AppointeeEmailId = AppointeeEmailId?.Trim();
                        _rawData.MobileNo = ((string)row["Phone No"])?.Trim();
                        _rawData.IsFresher = ((string)row["Fresher"])?.Trim()?.ToUpper();
                        _rawData.DateOfJoining = ((string)row["Date Of Joining"])?.Trim();
                        _rawData.lvl1Email = ((string)row["level1 Email"])?.Trim();
                        _rawData.lvl2Email = ((string)row["level2 Email"])?.Trim();
                        _rawData.lvl3Email = ((string)row["level3 Email"])?.Trim();
                        _rawData.CompanyName = CompanyName?.Trim();
                        rawListData.Add(_rawData);
                    }
                }
                List<AppointeeBasicInfo> _rawlistdata = rawListData;
                List<AppointeeBasicInfo>? ExistingUserList = await _userContext.validateExistingUser(rawListData);
                if (ExistingUserList?.Count > 0)
                {
                    _duplicateUserCount = ExistingUserList?.Count ?? 0;
                    _rawlistdata = rawListData.Except(ExistingUserList).ToList();
                    DataTable _rawfinallistdDT = CommonUtility.ToDataTable(ExistingUserList);
                    Filedata _fileDetails = GenerateDataTableTofile(_rawfinallistdDT, "Error", CommonEnum.ValidationType.Duplicate);
                    Filedata _duplicateFiledata = new() { FileData = _fileDetails?.FileData, FileName = "Duplicate_Data", FileType = "xlsx" };
                    _FileDataList.Add(_duplicateFiledata);
                    List<Filedata> attachtData = new() { _duplicateFiledata };
                    await _emailSender.SendMailWithAttachtment(userData.UserName, userData.EmailId, attachtData, CommonEnum.ValidationType.Duplicate);
                }

                List<AppointeeBasicInfo> _rawfinallistdata = _rawlistdata.GroupBy(m => new { m.CandidateID })
                            .Select(group => group.First())  // instead of First you can also apply your logic here what you want to take, for example an OrderBy
                            .ToList();

                RawdataSubmitRequest reqObj = new()
                {
                    ApnteFileData = _rawfinallistdata,
                    CompanyId = fileDetails?.CompanyId ?? 0,
                    UserId = fileDetails?.UserId ?? 0,
                    FileId = _Fileid
                };

                await _workFlowDalContext.PostRawfiledataAsync(reqObj);
                List<RawFileData> _rawdataList = await _workFlowDalContext.GetRawfiledataByIdAsync(_Fileid, fileDetails?.CompanyId ?? 0);

                foreach ((RawFileData row, RawFileDataDetailsResponse _data) in
                from RawFileData row in _rawdataList
                let _data = new RawFileDataDetailsResponse()
                select (row, _data))
                {
                    _data.id = row.RawFileId;
                    _data.fileId = row.FileId;
                    _data.companyId = row.CompanyId;
                    _data.companyName = row.CompanyName;
                    _data.CandidateId = row.CandidateId;
                    _data.appointeeName = row.AppointeeName;
                    _data.appointeeEmailId = row.AppointeeEmailId;
                    _data.mobileNo = row.MobileNo;
                    _data.dateOfOffer = row.DateOfOffer;
                    _data.dateOfJoining = row.DateOfJoining;
                    _data.isChecked = null;
                    RawFileData.Add(_data);
                }
            }

            //transaction.Commit();
            UploadedxlsRawFileDataResponse rawDataResponse = new() { RawFileData = RawFileData, DownloadFileData = _FileDataList, DuplicateCount = _duplicateUserCount, InvalidUserCount = _InvalidUserCount };
            return rawDataResponse;

        }
        public async Task<UploadedxlsRawFileDataResponse> UploadUpdateAppointeexlsFile(CompanyFileUploadRequest fileDetails)
        {
            int _nonExitingUserCount = 0;
            int _InvalidUserCount = 0;
            List<Filedata> _FileDataList = new();
            UserDetailsResponse userData = await _userContext.getUserDetailsAsyncbyId(fileDetails.UserId);
            List<RawFileDataDetailsResponse> RawFileData = new();
            UploadedXSLfileDetailsResponse resdata = await GetDataTableFromxlsFile(fileDetails.CompanyId, fileDetails.FileDetails, "UpdateUser");
            XSLfileDetailsListResponse validateResdata = ValidateUpdatexlsFile(resdata);

            if (validateResdata.InValidXlsData?.Rows?.Count > 0)
            {
                _InvalidUserCount = validateResdata.InValidXlsData?.Rows?.Count ?? 0;
                Filedata _fileDetails = GenerateDataTableTofile(validateResdata.InValidXlsData, "Error", CommonEnum.ValidationType.Invalid);
                Filedata _InvalidFiledata = new() { FileData = _fileDetails.FileData, FileName = "Invalid_Data", FileType = _fileDetails.FileType };
                _FileDataList.Add(_InvalidFiledata);
                List<Filedata> attachtData = new() { _InvalidFiledata };
                await _emailSender.SendMailWithAttachtment(userData.UserName, userData.EmailId, attachtData, CommonEnum.ValidationType.Invalid);
                //send mail to companyUser with error data file attached
            }

            if (validateResdata.ValidXlsData?.Rows?.Count > 0)
            {
                //GetFileId
                int _fileid = await _workFlowDalContext.PostUploadedXSLfileAsync(resdata.FileName, resdata.FilePath, fileDetails.CompanyId);
                List<UpdatedAppointeeBasicInfo> updateListData = new();

                foreach ((DataRow row, UpdatedAppointeeBasicInfo _rawData) in
                from DataRow row in validateResdata.ValidXlsData?.Rows
                let _rawData = new UpdatedAppointeeBasicInfo()
                select (row, _rawData))
                {
                    string? candidateID = ((string)row["Candidate ID"])?.Trim();
                    string? appointeeName = ((string)row["Updated Name"])?.Trim();
                    if (!string.IsNullOrEmpty(candidateID))
                    {
                        _rawData.CandidateID = candidateID;
                        _rawData.AppointeeName = string.IsNullOrEmpty(appointeeName) ? string.Empty : appointeeName;
                        _rawData.DateOfJoining = ((string)row["Updated Date Of Joining"])?.Trim();

                        updateListData.Add(_rawData);
                    }
                }

                List<UpdatedAppointeeBasicInfo> _finalupdatelistdata = updateListData.GroupBy(m => new { m.CandidateID })
                           .Select(group => group.First())  // instead of First you can also apply your logic here what you want to take, for example an OrderBy
                           .ToList();
                List<UpdatedAppointeeBasicInfo> _updatelistdata = _finalupdatelistdata;

                List<UpdatedAppointeeBasicInfo>? _nonExitingUserList = await _userContext.updateExistingUser(_finalupdatelistdata, fileDetails.UserId);
                if (_nonExitingUserList?.Count > 0)
                {
                    _nonExitingUserCount = _nonExitingUserList?.Count ?? 0;
                    _updatelistdata = updateListData.Except(_nonExitingUserList).ToList();
                    DataTable _finallistdDT = CommonUtility.ToDataTable(_nonExitingUserList);
                    Filedata _fileDetails = GenerateDataTableTofile(_finallistdDT, "Error", CommonEnum.ValidationType.Invalid);
                    Filedata _nonExistingFiledata = new() { FileData = _fileDetails?.FileData, FileName = "Non_Existing_Data", FileType = "xlsx" };
                    _FileDataList.Add(_nonExistingFiledata);
                    List<Filedata> attachtData = new() { _nonExistingFiledata };
                    await _emailSender.SendMailWithAttachtment(userData.UserName, userData.EmailId, attachtData, CommonEnum.ValidationType.NonExsist);
                }
            }
            UploadedxlsRawFileDataResponse rawDataResponse = new() { RawFileData = RawFileData, DownloadFileData = _FileDataList, NonExsitingCount = _nonExitingUserCount, InvalidUserCount = _InvalidUserCount };
            return rawDataResponse;
        }

        /// <summary>
        /// Genarate Datatable from uploaded excel file from Company or Admin User
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="fileData"></param>
        /// <returns>UploadedXSLfileDetails</returns>
        public Task<UploadedXSLfileDetailsResponse> GetDataTableFromxlsFile(int? CompanyId, IFormFile fileData, string? subfolder)
        {
            UploadedXSLfileDetailsResponse _uploadedxslfiledetails = new();

            string[]? slpitFileName = fileData.FileName?.Split('.');
            string? _fileNameWithoutext = slpitFileName?.FirstOrDefault();
            string? _ext = Path.GetExtension(fileData.FileName);
            string _fileName = $"{_fileNameWithoutext}{DateTime.Now.Ticks}{_ext}";


            string Cuurentpath = Directory.GetCurrentDirectory();
            string path = string.IsNullOrEmpty(subfolder) ? Path.Combine(Cuurentpath, "Uploads") : Path.Combine(Cuurentpath, "Uploads", subfolder);
            if (!Directory.Exists(path))
            {
                _ = Directory.CreateDirectory(path);
            }


            //Save the uploaded Excel file.
            string filePath = Path.Combine(path, _fileName);
            using (FileStream stream = new(filePath, FileMode.Create))
            {
                fileData.CopyTo(stream);
            }
            //Create a new DataTable.
            DataTable dt = new();

            FileInfo fi = new(filePath);
            using (ExcelPackage package = new(fi))
            {
                ExcelWorkbook workbook = package.Workbook;
                ExcelWorksheet? worksheet = workbook.Worksheets.First();

                // get number of rows and columns in the sheet
                int rows = worksheet.Dimension.Rows; // 20

                int columns = worksheet.Dimension.Columns; // 7
                                                           // loop through the worksheet rows and columns
                for (int i = 1; i <= rows; i++)
                {
                    if (i > 1)
                    {
                        _ = dt.Rows.Add();
                    }
                    for (int j = 1; j <= columns; j++)
                    {
                        string content = worksheet?.Cells[i, j]?.Value?.ToString() ?? string.Empty;
                        if (i == 1)
                        {

                            _ = dt.Columns.Add(content);
                        }
                        else
                        {
                            dt.Rows[^1][j - 1] = content;
                        }
                    }
                }
                //  package.Save();
            }

            _uploadedxslfiledetails.FileName = _fileName;
            _uploadedxslfiledetails.FilePath = filePath;
            _uploadedxslfiledetails.dataTable = dt;
            return Task.FromResult(_uploadedxslfiledetails);
        }
        /// <summary>
        /// validate data format of the uploaded data from excel file
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public XSLfileDetailsListResponse ValidateFromxlsFile(UploadedXSLfileDetailsResponse data)
        {
            XSLfileDetailsListResponse validateData = new();
            DataTable ValidData = new();
            DataTable InValidData = new();
            bool isRemarksColumnAdd = false;
            if (data?.dataTable?.Rows != null)
            {
                foreach (object? column in data.dataTable.Columns)
                {
                    _ = ValidData.Columns.Add(column.ToString());
                    _ = InValidData.Columns.Add(column.ToString());
                }

                //foreach (var (row, index, appointeeName, AppointeeEmailId, MobileNo, DateOfOffer, DateOfJoining, EPFWages)
                foreach ((DataRow row, int index, string candidateId, string companyName, string appointeeName, string AppointeeEmailId, string MobileNo, string DateOfJoining, string IsPFverificationReq, string lvl1Email, string lvl2Email, string lvl3Email)
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
                            string _Issue = "Company Name should not be blank.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {companyName}, Issue: {_Issue} " : $" Data: {companyName}, Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                        if (string.IsNullOrEmpty(appointeeName))
                        {
                            isdataValid = false;
                            string _Issue = "Name should not be blank.";
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
                        string _Issue = "Email format is wrong.";
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
                            string _Issue = "Leble1 Email format is wrong.";
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
                            string _Issue = "Leble1 Email format is wrong.";
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
                            string _Issue = "Leble1 Email format is wrong.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {lvl3Email}, Issue: {_Issue} " : $" Data: {lvl3Email}, Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                    }

                    bool isMobileNo = Regex.IsMatch(MobileNo, @"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)", RegexOptions.IgnoreCase);
                    if (!isMobileNo)
                    {
                        isdataValid = false;
                        string _Issue = "Mobile No format is wrong.";
                        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {MobileNo},  Issue: {_Issue}" : $" Data: {MobileNo},  Issue: {_Issue}";
                        validateData.InternalMessages.Add(msg);
                        errormsg += $"{msg}, ";
                    }


                    DateTime Jngrdate = new();
                    bool joindateValidity = DateTime.TryParseExact(DateOfJoining, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Jngrdate);

                    if (!joindateValidity)
                    {
                        isdataValid = false;
                        string _Issue = "Joining date format(dd-MM-yyyy) is wrong.";
                        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {DateOfJoining},  Issue: {_Issue} " : $" Data: {DateOfJoining},  Issue: {_Issue} ";
                        validateData.InternalMessages.Add(msg);
                        errormsg += $"{msg}, ";
                    }
                    else
                    {
                        if (Jngrdate < DateTime.Now)
                        {
                            isdataValid = false;
                            string _Issue = "Joining date must be a future date.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {DateOfJoining},  Issue: {_Issue} " : $" Data: {DateOfJoining},  Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                    }
                    if (string.IsNullOrEmpty(IsPFverificationReq) || !(IsPFverificationReq == "YES" || IsPFverificationReq == "NO"))
                    {
                        isdataValid = false;
                        string _Issue = "Fresher should not be blank and value should be 'YES' or 'NO'";
                        msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {IsPFverificationReq}, Issue: {_Issue} " : $" Data: {IsPFverificationReq}, Issue: {_Issue} ";
                        validateData.InternalMessages.Add(msg);
                        errormsg += $"{msg}, ";
                    }



                    if (isdataValid)
                    {
                        _ = ValidData.Rows.Add(row.ItemArray);
                    }
                    else
                    {
                        if (!isRemarksColumnAdd)
                        {
                            isRemarksColumnAdd = true;
                            _ = InValidData.Columns.Add("Remarks");
                        }
                        _ = InValidData.Rows.Add(row.ItemArray);
                        int rowindex = InValidData.Rows.Count - 1;
                        InValidData.Rows[rowindex]["Remarks"] = errormsg;
                    }
                }
            }
            validateData.ValidXlsData = ValidData;
            validateData.InValidXlsData = InValidData;
            return validateData;
        }

        public XSLfileDetailsListResponse ValidateUpdatexlsFile(UploadedXSLfileDetailsResponse data)
        {
            XSLfileDetailsListResponse validateData = new();
            DataTable ValidData = new();
            DataTable InValidData = new();
            bool isRemarksColumnAdd = false;
            if (data?.dataTable?.Rows != null)
            {
                foreach (object? column in data.dataTable.Columns)
                {
                    _ = ValidData.Columns.Add(column.ToString());
                    _ = InValidData.Columns.Add(column.ToString());
                }
                foreach ((DataRow row, int index, string candidateId, string appointeeName, string DateOfJoining)//, AppointeeEmailId, MobileNo,companyName, IsPFverificationReq, lvl1Email, lvl2Email, lvl3Email)
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
                       where !string.IsNullOrEmpty(candidateId)
                       select (row, index, candidateId, appointeeName, DateOfJoining))//, AppointeeEmailId, MobileNo, companyName, IsPFverificationReq, lvl1Email, lvl2Email, lvl3Email))
                {
                    string errormsg = string.Empty;
                    string msg = string.Empty;
                    bool isdataValid = true;
                    //bool isEmail = false;


                    if (!string.IsNullOrEmpty(DateOfJoining))
                    {
                        DateTime Jngrdate = new();
                        bool joindateValidity = DateTime.TryParseExact(DateOfJoining, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Jngrdate);

                        if (!joindateValidity)
                        {
                            isdataValid = false;
                            string _Issue = "Joining date format(dd-MM-yyyy) is wrong.";
                            msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {DateOfJoining},  Issue: {_Issue} " : $" Data: {DateOfJoining},  Issue: {_Issue} ";
                            validateData.InternalMessages.Add(msg);
                            errormsg += $"{msg}, ";
                        }
                        else
                        {
                            if (Jngrdate < DateTime.Now)
                            {
                                isdataValid = false;
                                string _Issue = "Joining date must be a future date.";
                                msg = string.IsNullOrEmpty(msg) ? $"Row No. : {index},  Data : {DateOfJoining},  Issue: {_Issue} " : $" Data: {DateOfJoining},  Issue: {_Issue} ";
                                validateData.InternalMessages.Add(msg);
                                errormsg += $"{msg}, ";
                            }
                        }
                    }


                    if (isdataValid)
                    {
                        _ = ValidData.Rows.Add(row.ItemArray);
                    }
                    else
                    {
                        if (!isRemarksColumnAdd)
                        {
                            isRemarksColumnAdd = true;
                            _ = InValidData.Columns.Add("Remarks");
                        }
                        _ = InValidData.Rows.Add(row.ItemArray);
                        int rowindex = InValidData.Rows.Count - 1;
                        InValidData.Rows[rowindex]["Remarks"] = errormsg;
                    }
                }
            }
            validateData.ValidXlsData = ValidData;
            validateData.InValidXlsData = InValidData;
            return validateData;
        }

        public Filedata GenerateDataTableTofile(DataTable data, string category, CommonEnum.ValidationType type)
        {
            DateTime _currDate = DateTime.Now;
            string _currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
            string subfolder = type switch
            {
                CommonEnum.ValidationType.Invalid => "Invalid",
                CommonEnum.ValidationType.Duplicate => "Duplicate",
                CommonEnum.ValidationType.ApiCount => "ApiCount",
                CommonEnum.ValidationType.AppointeeCount => "AppointeeCount",
                CommonEnum.ValidationType.Critical1week => "Critical",
                CommonEnum.ValidationType.Critical2week => "Critical",
                CommonEnum.ValidationType.NoLinkSent => "NoLinkSent",
                CommonEnum.ValidationType.NoResponse => "NoResponse",
                CommonEnum.ValidationType.Processing => "Processing",
                _ => "Default",
            };
            string fileName = $"{subfolder}_{category}_{_currDateString}.xlsx";
            byte[] fileData = CommonUtility.ExportFromDataTableToExcel(data, "AppointeeList", string.Empty);
            //string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string contentType = "xlsx";
            Filedata _Filedata = new() { FileData = fileData, FileName = fileName, FileType = contentType };
            return _Filedata;
        }

        /// <summary>
        /// Get Appointee Uploaded File detail from specefic path
        /// </summary>
        /// <param name = "filePath" ></ param >
        /// < returns > byte[] </ returns >
        public async Task<byte[]?> GetFileDataAsync(string filePath)
        {

            //code for validation and get the file
            if (Path.Exists(filePath))
            {
                FileExtensionContentTypeProvider provider = new();
                if (!provider.TryGetContentType(filePath, out _))
                {
                }

                byte[] bytes = await File.ReadAllBytesAsync(filePath);
                return bytes;
            }

            return null;
        }

        /// <summary>
        /// Remove file from Path
        /// </summary>
        /// <param name = "fileName" ></ param >
        /// < returns ></ returns >
        public bool RemoveDocFile(string fileName)
        {

            if ((fileName != null || fileName != string.Empty) && File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            return true;
        }

        /// <summary>
        /// Appointee uploaded file save db
        /// </summary>
        /// <param name="AppointeeDetails"></param>
        /// <returns></returns>
        /// 
        public async Task postappointeeUploadedFiles(AppointeeFileDetailsRequest AppointeeDetails)
        {
            //try
            //{
            if (AppointeeDetails != null)
            {
                List<IFormFile>? fileUploaded = AppointeeDetails.FileDetails;
                int _appointeeId = AppointeeDetails?.AppointeeId ?? 0;
                if (fileUploaded?.Count > 0 && _appointeeId != 0)
                {
                    List<AppointeeUploadDetails>? _prevDocList = await _appointeeContext.GetAppinteeUploadDetails(_appointeeId);
                    if (!string.IsNullOrEmpty(AppointeeDetails?.FileUploaded ?? string.Empty))
                    {

                        List<FileUploadDataModel>? _Uploadedfiledetails = JsonConvert.DeserializeObject<List<FileUploadDataModel>>(AppointeeDetails?.FileUploaded).ToList() ?? new List<FileUploadDataModel>();

                        foreach (IFormFile obj in fileUploaded)
                        {
                            //IFormFile obj = fileobj;
                            string fileName = $"{DateTime.Now.Ticks}_{obj?.FileName}";
                            long? fileLength = obj?.Length;
                            string? mimetype = obj?.ContentType?.ToLower();
                            string Cuurentpath = Directory.GetCurrentDirectory();
                            FileUploadDataModel? fileDetails = _Uploadedfiledetails?.Find(x => x.fileName == obj?.FileName && x.fileLength == fileLength && x.isFileUploaded);
                            if (fileDetails != null)
                            {
                                string path = Path.Combine(Cuurentpath, "AppointeeUploads", AppointeeDetails.AppointeeCode, fileDetails?.uploadTypeAlias ?? "");

                                if (!Directory.Exists(path))
                                {
                                    _ = Directory.CreateDirectory(path);
                                }
                                //Save the uploaded file.
                                string _filePath = Path.Combine(path, fileName);
                                using (FileStream stream = new(_filePath, FileMode.Create))
                                {
                                    obj?.CopyTo(stream);
                                }
                                if (Path.Exists(_filePath))
                                {
                                    AppointeeUploadDetails uploaddata = new()
                                    {
                                        AppointeeId = AppointeeDetails.AppointeeId,
                                        UploadTypeId = fileDetails.uploadTypeId,
                                        UploadTypeCode = fileDetails.uploadTypeAlias,
                                        FileName = fileName,
                                        UploadPath = _filePath,
                                        IsPathRefered = CheckType.yes,
                                        MimeType = mimetype,
                                        ActiveStatus = true,
                                        CreatedBy = AppointeeDetails.UserId,
                                        CreatedOn = DateTime.Now
                                    };

                                    // start remove prev file
                                    AppointeeUploadDetails? _prevdocDetails = _prevDocList?.Where(x => x.UploadTypeId == fileDetails.uploadTypeId)?.FirstOrDefault();

                                    await _appointeeContext.uploadFilesNUpdatePrevfiles(uploaddata, _prevdocDetails, AppointeeDetails.UserId);
                                    if (_prevdocDetails != null)
                                    {
                                        bool file_removed = RemoveDocFile(_prevdocDetails.UploadPath);
                                    }

                                    // end remove prev file
                                }
                            }
                        }
                    }
                }
            }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        public async Task postAppointeePassbookUpload(AppointeeDataSaveInFilesRequset UploadDetails)
        {
            if (UploadDetails != null)
            {
                List<AppointeeUploadDetails> uploadDetails = new();
                int? _appointeeId = UploadDetails?.AppointeeId;

                UploadTypeMaster _uploadFileType = await _appointeeContext.getFileTypeDataByAliasAsync(UploadDetails.FileTypeAlias);
                List<AppointeeUploadDetails> _prevDocList = await _appointeeContext.GetAppinteeUploadDetails(_appointeeId ?? 0);
                if (!string.IsNullOrEmpty(UploadDetails.FileUploaded ?? string.Empty))
                {
                    string Cuurentpath = Directory.GetCurrentDirectory();
                    string path = Path.Combine(Cuurentpath, "AppointeeUploads", UploadDetails.AppointeeCode, UploadDetails?.FileTypeAlias ?? "");
                    if (!Directory.Exists(path))
                    {
                        _ = Directory.CreateDirectory(path);
                    }
                    //Save the uploaded file.
                    string _fileName = string.Concat(_uploadFileType.UploadTypeName, UploadDetails.mimetype);
                    string _filePath = Path.Combine(path, _fileName);

                    if (UploadDetails.FileTypeAlias == FileTypealias.PFPassbook)
                    {
                        File.WriteAllText(_filePath, UploadDetails.FileUploaded);
                    }
                    else
                    {
                        using StreamWriter file = File.CreateText(_filePath);
                        JsonSerializer serializer = new();
                        //serialize object directly into file stream
                        serializer.Serialize(file, UploadDetails.FileUploaded);
                    }

                    if (Path.Exists(_filePath))
                    {
                        AppointeeUploadDetails uploaddata = new()
                        {
                            AppointeeId = UploadDetails.AppointeeId,
                            UploadTypeId = _uploadFileType.UploadTypeId,
                            UploadTypeCode = _uploadFileType.UploadTypeCode,
                            UploadSubTypeCode = UploadDetails.FileSubType,
                            FileName = _uploadFileType.UploadTypeName,
                            UploadPath = _filePath,
                            IsPathRefered = CheckType.yes,
                            MimeType = UploadDetails.mimetype,
                            ActiveStatus = true,
                            CreatedBy = UploadDetails.UserId,
                            CreatedOn = DateTime.Now
                        };
                        uploadDetails.Add(uploaddata);

                        //start remove prev file
                        AppointeeUploadDetails? _prevdocDetails = _prevDocList.Where(x => x.UploadTypeId == _uploadFileType.UploadTypeId).FirstOrDefault();

                        await _appointeeContext.uploadFilesNUpdatePrevfiles(uploaddata, _prevdocDetails, UploadDetails.UserId);
                        if (_prevdocDetails != null)
                        {
                            bool file_removed = RemoveDocFile(_prevdocDetails.UploadPath);
                        }
                        //end remove prev file
                    }
                }
            }
        }
        public async Task<FileDetailsResponse> DownloadTrustPassbook(int appointeeId, int userId)
        {
            FileDetailsResponse fileDetails = new();
            AppointeeDetails _appointeedetails = await _appointeeContext.GetAppinteeDetailsById(appointeeId);

            string cuurentpath = Directory.GetCurrentDirectory();
            DateTime _currDate = DateTime.Now;
            string currDateString = $"{_currDate.Day}_{_currDate.Month}_{_currDate.Year}";
            string fileName = $"{_appointeedetails?.AppointeeName}_TrustPassbook_{currDateString}";
            string filePath = string.Empty;
            string fileExt = string.Empty;
            List<AppointeeUploadDetails> _prevDocList = await _appointeeContext.GetAppinteeUploadDetails(appointeeId);
            List<AppointeeUploadDetails>? _docList = _prevDocList?.Where(x => x.ActiveStatus == true && x.UploadTypeCode == FileTypealias.PFPassbookTrust).ToList();
            if (_docList?.Count > 0)
            {
                filePath = _docList?.LastOrDefault()?.UploadPath ?? string.Empty;
                fileExt = _docList?.LastOrDefault()?.MimeType ?? string.Empty;

            }
            byte[]? bytes = !string.IsNullOrEmpty(filePath) ? await File.ReadAllBytesAsync(filePath) : null;

            fileDetails.FileData = bytes;
            fileDetails.FileName = fileName;
            fileDetails.FileExtention = fileExt;

            return fileDetails;
        }
        public async Task OfflineKycStatusUpdate(OfflineAadharVarifyStatusUpdateRequest reqObj)
        {

            await _appointeeContext.PostOfflineKycStatus(reqObj);

        }
        public async Task getFiledetailsByAppointeeId(int appointeeId, string candidateFileName, List<FileDetailsResponse> _FileDataList)
        {
            List<AppointeeUploadDetails> _UploadDetails = await _appointeeContext.GetAppinteeUploadDetails(appointeeId);
            if (_UploadDetails?.Count > 0)
            {
                foreach ((AppointeeUploadDetails obj, FileDetailsResponse doc) in from obj in _UploadDetails
                                                                                  let doc = new FileDetailsResponse()
                                                                                  select (obj, doc))
                {
                    byte[]? _FileData = await GetFileDataAsync(obj.UploadPath);
                    doc.FileData = _FileData;
                    string _fileName = $"{candidateFileName}_{obj?.UploadTypeCode}";
                    doc.FileName = _fileName;
                    doc.UploadTypeId = obj?.UploadTypeId ?? 0;
                    doc.mimeType = obj?.MimeType ?? string.Empty;
                    doc.UploadTypeAlias = obj?.UploadTypeCode ?? string.Empty;
                    _FileDataList.Add(doc);

                }
            }
        }
        public async Task<AppointeeUploadDetails> getFiledetailsByFileType(int appointeeId, string fileTypeCode)
        {
            AppointeeUploadDetails FileDetailsResponse = new();
            List<AppointeeUploadDetails> _UploadDetails = await _appointeeContext.GetAppinteeUploadDetails(appointeeId);
            if (_UploadDetails?.Count > 0)
            {
                FileDetailsResponse = _UploadDetails?.LastOrDefault(x => x.UploadTypeCode == fileTypeCode);
            }
            return FileDetailsResponse;
        }
        public async Task<UnzipAadharDataResponse> unzipAdharzipFiles(AppointeeAadhaarAadharXmlVarifyRequest AppointeeAdharUploadFileDetails)
        {
            UnzipAadharDataResponse response = new();
            string unzipFileContent = string.Empty;
            string messeege = string.Empty;
            bool isValid = false;

            if ((AppointeeAdharUploadFileDetails?.appointeeId ?? 0) != 0 && AppointeeAdharUploadFileDetails?.aadharFileDetails != null && !string.IsNullOrEmpty(AppointeeAdharUploadFileDetails.shareCode))
            {
                IFormFile? AdharfileUploaded = AppointeeAdharUploadFileDetails.aadharFileDetails;
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        AdharfileUploaded.CopyTo(memoryStream);

                        memoryStream.Position = 0;
                        using (var zipStream = new ZipInputStream(memoryStream))
                        {
                            if (!string.IsNullOrEmpty(AppointeeAdharUploadFileDetails.shareCode))
                            {
                                zipStream.Password = AppointeeAdharUploadFileDetails.shareCode;
                            }

                            ZipEntry entry;
                            while ((entry = zipStream.GetNextEntry()) != null)
                            {
                                using (var reader = new StreamReader(zipStream, Encoding.UTF8))
                                {
                                    unzipFileContent = reader.ReadToEnd();
                                    isValid = true;
                                }
                            }

                        }
                    }
                }
                catch (ZipException ex)
                {
                    messeege = $"Error reading zip file: {ex.Message}";
                }
                catch (Exception ex)
                {
                    messeege = $"An unexpected error occurred: {ex.Message}";
                }
            }
            else
            {
                if (AppointeeAdharUploadFileDetails?.aadharFileDetails == null)
                {
                    messeege = "eKYC file can not be null,";
                }

                if (string.IsNullOrEmpty(AppointeeAdharUploadFileDetails?.shareCode))
                {
                    messeege = messeege + " share code can not be null";
                }
            }
            if (isValid)
            {
                var isSigValid = await OfflineKycDigitalSignatureCheck(unzipFileContent);
                messeege = isSigValid ? messeege : "Invalid Kyc File signature";
                isValid = isSigValid;
            }
            response.FileContent = unzipFileContent;
            response.IsValid = isValid;
            response.Message = messeege;
            return response;
        }
        private async Task<bool> OfflineKycDigitalSignatureCheck(string? xmlFileData)
        {
            bool isValid = false;
            try
            {
                if (xmlFileData != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlFileData);

                    // Navigate and extract data
                    XmlNode root = xmlDoc.DocumentElement;

                    // Find the signature node
                    XmlNode signatureNode = root.SelectSingleNode("//ds:Signature", GetNamespaceManager(xmlDoc));
                    if (signatureNode == null)
                    {
                        return false;
                    }

                    // Create a new SignedXml object and load the signature node
                    SignedXml signedXml = new SignedXml(xmlDoc);
                    signedXml.LoadXml((XmlElement)signatureNode);

                    // Find the signing certificate
                    X509Certificate2 certificate = null;
                    foreach (KeyInfoClause keyInfoClause in signedXml.KeyInfo)
                    {
                        if (keyInfoClause is KeyInfoX509Data)
                        {
                            foreach (var cert in ((KeyInfoX509Data)keyInfoClause).Certificates)
                            {
                                certificate = (X509Certificate2)cert;
                                break;
                            }
                        }
                    }

                    if (certificate == null)
                    {
                        return false;
                    }

                    // Verify the signature
                    isValid = signedXml.CheckSignature(certificate, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return isValid;
        }

        private static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDoc)
        {
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMgr.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            return nsMgr;
        }
    }
}
