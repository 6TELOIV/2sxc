﻿#if NETSTANDARD
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace ToSic.Sxc.WebApi.Adam
{
    public class HttpUploadedFile
    {
        public HttpUploadedFile(HttpRequest request) => Request = request;
        public HttpRequest Request { get; }

        // https://stackoverflow.com/questions/45871479/net-core-2-how-to-check-if-the-request-is-mime-multipart-content
        public bool IsMultipart() => Request.GetMultipartBoundary() != null;

        public bool HasFiles() => Request.Form.Files.Any();

        public int Count => Request.Form.Files.Count;

        public (string, Stream) GetStream(int i = 0)
        {
            var originalFile = Request.Form.Files[i];
            // TODO: sanitize fileName
            return (originalFile.FileName, originalFile.OpenReadStream());
        }

    }
}
#endif