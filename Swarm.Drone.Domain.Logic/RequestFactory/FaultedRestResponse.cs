using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace Swarm.Drone.Domain.Logic.RequestFactory
{
	internal class FaultedRestResponse : IRestResponse
	{
		public FaultedRestResponse(IRestRequest request, Exception fault)
		{
			Request = request;
			StatusCode = HttpStatusCode.BadRequest;

			Cookies = new List<RestResponseCookie>();
			Headers = new List<Parameter>();

			ResponseStatus = ResponseStatus.Error;
			ErrorMessage = fault.Message;
			ErrorException = fault;
		}

		public IRestRequest Request { get; set; }
		public string ContentType { get; set; }
		public long ContentLength { get; set; }
		public string ContentEncoding { get; set; }
		public string Content { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public string StatusDescription { get; set; }
		public byte[] RawBytes { get; set; }
		public Uri ResponseUri { get; set; }
		public string Server { get; set; }
		public IList<RestResponseCookie> Cookies { get; private set; }
		public IList<Parameter> Headers { get; private set; }
		public ResponseStatus ResponseStatus { get; set; }
		public string ErrorMessage { get; set; }
		public Exception ErrorException { get; set; }
	}
}