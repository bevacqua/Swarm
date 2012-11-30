using System;
using Swarm.Common.Interface;
using Swarm.Common.IoC;
using Swarm.Common.Mvc.Interface;

namespace Swarm.Overmind.Domain.Logic.Service
{
	public abstract class BaseService
	{
		private readonly Lazy<IUrlHelper> urlHelperLazy;
		private readonly Lazy<IMapper> mapperLazy;

	    protected IUrlHelper urlHelper
	    {
		    get { return urlHelperLazy.Value; }
	    }

		protected IMapper mapper
		{
			get { return mapperLazy.Value; }
		}

        protected BaseService()
		{
			urlHelperLazy = IoC.Container.Resolve<Lazy<IUrlHelper>>();
			mapperLazy = IoC.Container.Resolve<Lazy<IMapper>>();
		}
	}
}