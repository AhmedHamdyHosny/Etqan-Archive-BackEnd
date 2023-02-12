using Classes.Common;
using EtqanArchive.BackEnd.Models;
using EtqanArchive.DataLayer.TableEntity;
using GenericBackEndCore.Classes.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EtqanArchive.BackEnd.Services
{
    public interface IProjectFileService
    {
        Task<IActionResult> GetPathFiles(string directoryPath);
    }
    public class ProjectFileService : IProjectFileService
    {
        private readonly IConfiguration _configuration;

        public ProjectFileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> GetPathFiles(string directoryPath)
        {

            string IP = _configuration["StorageServer:IP"];
            string username = _configuration["StorageServer:NetworkUsername"];
            string password = _configuration["StorageServer:NetworkPassword"];
            IEnumerable<DirecortyPathFilesResponseModel> directoryFiles = null;
            IEnumerable<FileExtension> fileExtensions = new FileExtensionModel<FileExtension>().GetData(IsBlock: false, IsDeleted: false, IncludeReferences: "ContentType");
            //copy file to destination
            //for test
            //using (new NetworkConnection(@"\\" + IP, new NetworkCredential(username, password, System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName)))
            //{
                if (Directory.Exists(directoryPath))
                {
                    List<string> files = GetDirectoryFiles(directoryPath);
                    directoryFiles = files.Select(path => new FileInfo(path)).Select(fileInfo =>
                    {
                        var fileExtension = getFileExtensionId(fileExtensions, fileInfo.Extension.Remove(0,1));
                        var model = new DirecortyPathFilesResponseModel()
                        {
                            FilePath = fileInfo.FullName,
                            FileName = fileInfo.Name,
                            FileSize = (double)fileInfo.Length / (double)(1024 * 1024),
                            Duration = fileExtension.ContentType.ContentTypeId == DBEnums.ContentType.Video ? getMediaDuration(fileInfo) : null,
                            FileExtensionId = fileExtension.FileExtensionId,
                            ContentTypeName = fileExtension.ContentType.ContentTypeName
                        };
                        return model;
                    });
                }
            //}

            return await Task.FromResult(new OkObjectResult(new JsonResponse<IEnumerable<DirecortyPathFilesResponseModel>>(directoryFiles)));
        }

        private int? getMediaDuration(FileInfo fileInfo)
        {
            //get media duration
            try
            {

            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private FileExtension getFileExtensionId(IEnumerable<FileExtension> fileExtensions, string extension)
        {
            var fileExtension = fileExtensions.SingleOrDefault(x => x.FileExtensionName == extension);
            if(fileExtension == null)
                fileExtension = fileExtensions.SingleOrDefault(x => x.FileExtensionId == DBEnums.FileExtension.Unknown);
            return fileExtension;
        }

        private List<string> GetDirectoryFiles(string directoryPath)
        {
            List<string> files = Directory.GetFiles(directoryPath).ToList();
            var directoriesPaths = Directory.GetDirectories(directoryPath).Where(x => !new DirectoryInfo(x).Name.Equals("#recycle"));
            foreach (string item in directoriesPaths)
            {
                files.AddRange(GetDirectoryFiles(item));
            }
            return files;
        }
    }
}
