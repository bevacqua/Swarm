namespace Swarm.Common.Interface
{
    public interface IResourceHelper<out TRawString>
    {
        string String(string key, params object[] args);
        string Shared(string file, string key, params object[] args);
        TRawString Raw(string key, params object[] args);
        TRawString RawShared(string file, string key, params object[] args);
    }
}