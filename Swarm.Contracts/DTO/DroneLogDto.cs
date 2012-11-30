using System;
using System.Collections.Generic;

namespace Swarm.Contracts.DTO
{
	public class DroneLogDto
	{
		public string LoggerName { get; set; }
		public string Level { get; set; }
		public string Message { get; set; }
		public string ThreadName { get; set; }
		public DateTime TimeStamp { get; set; }
		public string ExceptionString { get; set; }
		public string Domain { get; set; }
		public IDictionary<string, object> Properties { get; set; }

		// TODO: make use of parameters sent over the wire (contain additional information such as exception objects)
		public IDictionary<string, object> Parameters { get; set; }
	}
}
