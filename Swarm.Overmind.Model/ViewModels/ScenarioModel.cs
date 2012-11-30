using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using RestSharp;
using Swarm.Overmind.Domain.Entity.Entities;
using Swarm.Overmind.Model.Validators;
using Swarm.Overmind.Model.ViewModels.Resources;

namespace Swarm.Overmind.Model.ViewModels
{
	[Validator(typeof(ScenarioValidator))]
	public class ScenarioModel
	{
		public long Id { get; set; }
		[Display(Name = "Name", ResourceType = typeof(Index))]
		public string Name { get; set; }
		[Display(Name = "VirtualUsers", ResourceType = typeof(Index))]
		public int? VirtualUsers { get; set; }
		[Display(Name = "SleepTime", ResourceType = typeof(Index))]
		public TimeSpan? SleepTime { get; set; }
		[Display(Name = "RampUpTime", ResourceType = typeof(Index))]
		public TimeSpan? RampUpTime { get; set; }
		[Display(Name = "SamplingInterval", ResourceType = typeof(Index))]
		public TimeSpan? SamplingInterval { get; set; }
		[Display(Name = "RequestTimeout", ResourceType = typeof(Index))]
		public TimeSpan? RequestTimeout { get; set; }
		[Display(Name = "LogLevel", ResourceType = typeof(Index))]
		public LogLevel? LogLevel { get; set; }
		[Display(Name = "Method", ResourceType = typeof(Index))]
		public Method? Method { get; set; }
		[Display(Name = "Endpoint", ResourceType = typeof(Index))]
		public string Endpoint { get; set; }
		[Display(Name = "DataFile", ResourceType = typeof(Index))]
		public Guid? FileCode { get; set; }

		public SelectList FilesList { get; set; }
	}
}
