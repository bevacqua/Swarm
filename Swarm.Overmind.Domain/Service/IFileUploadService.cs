using System;
using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Service
{
	public interface IFileUploadService
	{
		IEnumerable<FileUpload> GetAllFiles();
		void UploadFile(FileUpload newFile);
		FileUpload GetUploadsByCode(Guid code);
		void DeleteByCode(Guid code);
		FileUpload GetById(long uploadedFileId);
		string GetFullPath(FileUpload fileUpload);
	}
}
