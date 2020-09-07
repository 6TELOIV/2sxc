﻿using System;
using System.IO;
using ToSic.Eav.Security.Permissions;
using ToSic.Sxc.Adam;
using ToSic.Sxc.WebApi.Errors;

namespace ToSic.Sxc.WebApi.Adam
{
    internal class AdamTransUpload : AdamTransactionBase<AdamTransUpload>
    {
        public AdamTransUpload() : base("Adm.TrnDel") { }

        internal UploadResultDto UploadOne(Stream stream, string subFolder, string fileName)
        {
            var file = UploadOne(stream, fileName, subFolder, false);

            return new UploadResultDto
            {
                Success = true,
                Error = "",
                Name = file.Name,
                Id = file.Id,
                Path = file.Url,
                Type = Classification.TypeName(file.Extension)
            };
        }

        internal IFile UploadOne(Stream stream, string originalFileName, string subFolder, bool skipFieldAndContentTypePermissionCheck)
        {
            Log.Add($"upload one subfold:{subFolder}");

            HttpExceptionAbstraction exp;
            if (!skipFieldAndContentTypePermissionCheck)
            {
                if (!State.Security.UserIsPermittedOnField(GrantSets.WriteSomething, out exp))
                    throw exp;

                // check that if the user should only see drafts, he doesn't see items of published data
                if (!State.Security.UserIsNotRestrictedOrItemIsDraft(State.ItemGuid, out var permissionException))
                    throw permissionException;
            }

            var folder = State.ContainerContext.Folder();

            if (!string.IsNullOrEmpty(subFolder))
                folder = State.ContainerContext.Folder(subFolder, false);

            // start with a security check...
            var fs = State.AdamAppContext.EnvironmentFs;
            var parentFolder = fs.GetFolder(folder.Id);

            // validate that dnn user have write permissions for folder in case dnn file system is used (usePortalRoot)
            if (State.UseTenantRoot && !State.Security.CanEditFolder(parentFolder.Id))
                throw HttpException.PermissionDenied("can't upload - permission denied");

            // we only upload into valid adam if that's the scenario
            if (!State.Security.SuperUserOrAccessingItemFolder(parentFolder.Path, out exp))
                throw exp;

            #region check content-type extensions...

            // Check file size and extension
            var fileName = string.Copy(originalFileName);
            if (!State.Security.ExtensionIsOk(fileName, out var exceptionAbstraction))
                throw exceptionAbstraction;

            // check metadata of the FieldDef to see if still allowed extension
            var additionalFilter = State.Attribute.Metadata.GetBestValue<string>("FileFilter");
            if (!string.IsNullOrWhiteSpace(additionalFilter)
                && !new SecurityCheckHelpers().Init(Log).CustomFileFilterOk(additionalFilter, originalFileName))
                throw HttpException.NotAllowedFileType(fileName, "field has custom file-filter, which doesn't match");

            // note 2018-04-20 2dm: can't do this for wysiwyg, as it doesn't have a setting for allowed file-uploads

            #endregion

            var maxSizeKb = fs.MaxUploadKb();
            if (stream.Length > 1024 * maxSizeKb)
                throw new Exception($"file too large - more than {maxSizeKb}Kb");

            // remove forbidden / troubling file name characters
            fileName = fileName
                .Replace("+", "plus")
                .Replace("%", "per")
                .Replace("#", "hash");

            if (fileName != originalFileName)
                Log.Add($"cleaned file name from'{originalFileName}' to '{fileName}'");

            var eavFile = fs.Add(parentFolder, stream, fileName, true);

            //// Make sure the image does not exist yet, cycle through numbers (change file name)
            //fileName = state.AdamAppContext.EnvironmentFs.FindUniqueFileName(parentFolder, fileName);

            //// Everything is ok, add file to DNN
            //var dnnFolder = FolderManager.Instance.GetFolder(folder.Id);
            //var dnnFile = FileManager.Instance.AddFile(dnnFolder, Path.GetFileName(fileName),
            //    stream);

            //var dtoMake= new AdamItemDtoMaker(state.Security);
            //var dto = dtoMake.Create(dnnFile2, state);

            //var adamcontext = new AdamAppContext(_block.Context.Tenant, state.App, _block, 10, Log);
            //var eavFile = new File(state.AdamAppContext /*adamcontext*/)
            //{
            //    Created = dnnFile.CreatedOnDate,
            //    Extension = dnnFile.Extension,
            //    FullName = dnnFile.FileName,
            //    Folder = dnnFile.Folder,
            //    FolderId = dnnFile.FolderId,
            //    Id = dnnFile.FileId,
            //    Modified = dnnFile.LastModificationTime
            //};

            return eavFile;
        }
    }
}
