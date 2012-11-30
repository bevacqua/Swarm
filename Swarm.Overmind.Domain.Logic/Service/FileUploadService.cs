using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Repository;
using Swarm.Overmind.Domain.Service;

namespace Swarm.Overmind.Domain.Logic.Service
{
	public class FileUploadService : BaseService, IFileUploadService
	{
		private readonly IFileUploadRepository uploadRepository;

		public FileUploadService(IFileUploadRepository fileUpload)
        {
			if (fileUpload == null)
			{
				throw new ArgumentNullException("fileUpload");
			}
			uploadRepository = fileUpload;
		}

		public IEnumerable<FileUpload> GetAllFiles()
		{
			return uploadRepository.GetAllFiles();
		}

		private string GetFullLocalPath(string path)
		{
			return urlHelper.MapPath(string.Format("~/App_Data/Uploads/{0}", path));
		}

		public void UploadFile(FileUpload newFile)
		{
			FileStream stream = null;
			try
			{
				var physicalPath = GetFullLocalPath(newFile.Path);
				stream = new FileStream(physicalPath, FileMode.Create, FileAccess.Write);
				if (stream.CanWrite)
				{
					stream.Write(newFile.Contents, 0, newFile.Contents.Length);
				}
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
			uploadRepository.Insert(newFile);
		}


		public FileUpload GetUploadsByCode(System.Guid code)
		{
			return uploadRepository.GetByCode(code);
		}
		
		public void DeleteByCode(Guid code)
		{
			uploadRepository.DeleteByCode(code);
		}

		public string GetFullPath(FileUpload fileUpload)
		{
			return GetFullLocalPath(fileUpload.Path);
		}

		public FileUpload GetById(long uploadedFileId)
		{
			return uploadRepository.GetById(uploadedFileId);
		}
	}
}