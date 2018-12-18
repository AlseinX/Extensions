namespace Alsein.Utilities.Runtime.InteropServices
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
        INativeModule LoadAssembly(string filename);
    }
}
