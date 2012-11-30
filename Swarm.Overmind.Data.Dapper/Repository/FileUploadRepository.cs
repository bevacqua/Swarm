using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Domain.Repository;

namespace Swarm.Overmind.Data.Dapper.Repository
{
	public class FileUploadRepository : IFileUploadRepository
	{
		private readonly IDbConnection connection;

		public FileUploadRepository(IDbConnection connection)
        {
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			this.connection = connection;
        }

		public FileUpload Insert(FileUpload entity)
		{
			connection.Insert(entity);
			return entity;
		}

		public FileUpload GetById(long Id)
		{
			const string sql = @"
				
				SELECT [FileUpload].*
				FROM [FileUpload]
				WHERE [FileUpload].[Id] = @Id
			";
			IEnumerable<FileUpload> files = connection.Query<FileUpload>(sql, new { Id });
			return files.FirstOrDefault();
		}

		public FileUpload GetByCode(Guid code)
		{
			const string sql = @"
				
				SELECT [FileUpload].*
				FROM [FileUpload]
				WHERE [FileUpload].[Code] = @code
			";
			IEnumerable<FileUpload> files = connection.Query<FileUpload>(sql, new { code });
			return files.FirstOrDefault();
		}

		public IEnumerable<FileUpload> GetAllFiles()
		{
			const string sql = @"
				
				SELECT [FileUpload].*
				FROM [FileUpload]
			";
			IEnumerable<FileUpload> files = connection.Query<FileUpload>(sql);
			return files.ToList();
		}
		
		public void DeleteByCode(Guid code)
		{
			var entity = GetByCode(code);
			if (entity != null)
				connection.Delete(entity);
		}
	}
}
