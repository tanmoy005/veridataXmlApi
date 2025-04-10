﻿using System.Data;
using Microsoft.AspNetCore.Http;
using VERIDATA.Model.DataAccess;
using VERIDATA.Model.DataAccess.Response;
using VERIDATA.Model.Request;
using VERIDATA.Model.Response;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.Interfaces
{
    public interface IFileContext
    {
        /// <summary>
        /// save excel file details in to db
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filepath"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public Task<UploadedxlsRawFileDataResponse> UploadAppointeexlsFile(CompanyFileUploadRequest fileDetails);//

        public Task<UploadedxlsRawFileDataResponse> UploadUpdateAppointeexlsFile(CompanyFileUploadRequest fileDetails);//

        public Task<UploadedXSLfileDetailsResponse> GetDataTableFromxlsFile(int? CompanyId, IFormFile fileData, string? subfolder);

        public XSLfileDetailsListResponse ValidateUpdatexlsFile(UploadedXSLfileDetailsResponse data);

        public Filedata GenerateDataTableTofile(DataTable data, string category, ValidationType type);

        public Task<byte[]?> GetFileDataAsync(string filePath);

        public Task postappointeeUploadedFiles(AppointeeFileDetailsRequest AppointeeDetails);

        public Task OfflineKycStatusUpdate(OfflineAadharVarifyStatusUpdateRequest reqObj);

        public Task<FileDetailsResponse> DownloadTrustPassbook(int appointeeId, int userId);

        public Task getFiledetailsByAppointeeId(int appointeeId, List<FileDetailsResponse> _FileDataList);

        public Task<UnzipAadharDataResponse> unzipAdharzipFiles(AppointeeAadhaarAadharXmlVarifyRequest AppointeeAdharUploadFileDetails);

        public Task<FileDetailsResponse> getFiledetailsByFileUploadId(int appointeeId, int? uploadId);

        public Task postappointeeReUploadedFiles(AppointeeReUploadFilesAfterSubmitRequest AppointeeDetails);
    }
}