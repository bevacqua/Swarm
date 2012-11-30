using System;
using System.Collections.Generic;
using Swarm.Overmind.Domain.Entity.Entities;

namespace Swarm.Overmind.Domain.Repository
{
	public interface IFileUploadRepository 
	{
		FileUpload GetByCode(Guid code);
		FileUpload GetById(long Id);
		IEnumerable<FileUpload> GetAllFiles();
		FileUpload Insert(FileUpload entity);
		void DeleteByCode(Guid code);
	}
}
