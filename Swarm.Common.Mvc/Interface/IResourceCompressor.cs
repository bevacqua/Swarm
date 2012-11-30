using System.Collections.Generic;

namespace Swarm.Common.Mvc.Interface
{
	public interface IResourceCompressor
	{
		string MinifyStylesheet(string source, bool wrapResultInTags = true);
		string MinifyStylesheet(IEnumerable<string> sources, bool wrapResultInTags = true);

		string MinifyJavaScript(string source, bool wrapResultInTags = true);
		string MinifyJavaScript(IEnumerable<string> sources, bool wrapResultInTags = true);
	}
}