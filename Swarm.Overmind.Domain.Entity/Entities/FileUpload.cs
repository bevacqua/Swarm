using System;

namespace Swarm.Overmind.Domain.Entity.Entities
{
	public class FileUpload
	{
		public long Id { get; set; }
		public Guid Code { get; set; }
		public DateTime Date { get; set; }
		public byte[] Contents { get; set; }
		public string Path { get; set; }
	}
}
