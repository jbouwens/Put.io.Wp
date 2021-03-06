﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Put.io.Api.ResponseObjects;
using Put.io.Api.ResponseObjects.Files;
using RestSharp;
using Windows.Storage;
using Put.io.Api.ResponseObjects.Transfers;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage.Streams;

namespace Put.io.Api.Rest
{
    public class Files : RestBase
    {
        public Files(string authKey)
        {
            if(string.IsNullOrEmpty(authKey))
                throw new KeyNotFoundException("Auth key not defined");

            AuthKey = authKey;
        }

        /// <summary>
        /// Retrieves a list of files given a parent and authkey. This method is async
        /// </summary>
        /// <param name="parentID">(optional) the parent folder to browse</param>
        /// <param name="callback">The callback method to use once completed</param>
        public void ListFiles(int? parentID, Action<IRestResponse<FileList>> callback)
        {
            var request = NewRequest(UrlHelper.ListFiles(), Method.GET);
            
            if (parentID.HasValue && parentID.Value > 0)
                request.AddParameter("parent_id", parentID.Value);

            RestClient.ExecuteAsync(request, callback);
        }

        /// <summary>
        /// Gets a specific file by ID. This method is Async
        /// </summary>
        /// <param name="fileID">Id of the required file</param>
        /// <param name="callback">The callback method to use once completed</param>
        public void GetFile(int fileID, Action<IRestResponse<GetFileResponse>> callback)
        {
            var request = NewRequest(UrlHelper.GetFile(), Method.GET);
            
            request.AddUrlSegment("id", fileID.ToString(CultureInfo.InvariantCulture));

            RestClient.ExecuteAsync(request, callback);
        }

        /// <summary>
        /// Will initiate a file to be converted
        /// </summary>
        /// <param name="fileID">Id of the file to convert</param>
        /// <param name="callback">The callback method to use once completed</param>
        public void FileToMp4(int fileID, Action<IRestResponse<BaseObject>> callback)
        {
            var request = GetMp4Request(fileID, Method.POST);

            RestClient.ExecuteAsync(request, callback);
        }
        
        /// <summary>
        /// Gets the conversion status of an mp4 file
        /// </summary>
        /// <param name="fileID">Id of the file to check</param>
        /// <param name="callback">The callback method to use once completed</param>
        public void Mp4Status(int fileID, Action<IRestResponse<Mp4>> callback)
        {
            var request = GetMp4Request(fileID, Method.GET);

            RestClient.ExecuteAsync(request, callback);
        }

        private IRestRequest GetMp4Request(int fileID, Method method)
        {
            var request = NewRequest(UrlHelper.FileMp4(), method);
            
            request.AddUrlSegment("id", fileID.ToString(CultureInfo.InvariantCulture));

            return request;
        }

        /// <summary>
        /// Gets the URI of the specified file to download
        /// </summary>
        /// <param name="fileID">Id of file to download</param>
        /// <param name="callback">The callback method to use once completed</param>
        public void DownloadFile(int fileID, Action<IRestResponse> callback)
        {
            GetHeaderFile(fileID, callback, UrlHelper.DownloadFile());
        }

        /// <summary>
        /// Gets the URI of the specified file to stream
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="callback"></param>
        public void StreamFile(int fileID, Action<IRestResponse> callback)
        {
            GetHeaderFile(fileID, callback, UrlHelper.StreamFile());
        }

        /// <summary>
        /// Gets the URI of the specified mp4 to download
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="callback"></param>
        public void DownloadMp4(int fileID, Action<IRestResponse> callback)
        {
            GetHeaderFile(fileID, callback, UrlHelper.DownloadMp4());
        }

        /// <summary>
        /// Gets the URI of the specified mp4 stream
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="callback"></param>
        public void StreamMp4(int fileID, Action<IRestResponse> callback)
        {
            GetHeaderFile(fileID, callback, UrlHelper.StreamMp4());
        }

        private void GetHeaderFile(int fileID, Action<IRestResponse> callback, string url)
        {
            var request = NewRequest(url, Method.HEAD);

            request.AddUrlSegment("id", fileID.ToString(CultureInfo.InvariantCulture));

            RestClient.ExecuteAsync(request, callback);
        }

        /// <summary>
        /// Deletes a list of files. This method is Async
        /// </summary>
        /// <param name="fileIDs">Ids of the files to delete</param>
        /// <param name="callback">The callback method to use once completed</param>
        public void DeleteFiles(IEnumerable<int> fileIDs, Action<IRestResponse> callback)
        {
            var request = NewRequest(UrlHelper.DeleteFiles(), Method.POST);

            var ids = fileIDs.Aggregate(string.Empty, (current, id) => current + (id + ","));
            if (!string.IsNullOrEmpty(ids))
                ids = ids.Substring(0, ids.Length - 1);

            request.AddParameter("file_ids", ids);

            RestClient.ExecuteAsync(request, callback);
        }

        public void UploadFiles(byte[] content, Action<IRestResponse<GetTransferResponse>> callback)
        {
            var request = NewRequest(UrlHelper.UploadFile(), Method.POST);
            request.AddFile("file", content, "imported.torrent");
            request.AddQueryParameter("oauth_token", AuthKey);

            RestUploadClient.ExecuteAsync(request, callback);
        }

        //Non-critical api calls
        // GET /files/search/<query>/page/<page_no>
        // POST /files/upload
        // POST /files/create-folder
        // POST /files/delete
        // POST /files/rename
        // POST /files/move
    }
}