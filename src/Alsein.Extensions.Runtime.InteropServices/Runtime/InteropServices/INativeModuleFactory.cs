namespace Alsein.Extensions.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface INativeModuleFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        INativeModule LoadModule(string filename);
    }
}
