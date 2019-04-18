namespace Alsein.Extensions.Curses
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringWriterExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        public static void Write<TValue>(this IStringWriter writer, TValue value) => writer.Write(value.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Write(this IStringWriter writer, string format, params object[] args) => writer.Write(string.Format(format, args));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public static void WriteLine(this IStringWriter writer) => writer.Write("\n");

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        public static void WriteLine<TValue>(this IStringWriter writer, TValue value) => writer.Write(value + "\n");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteLine(this IStringWriter writer, string format, params object[] args) => writer.Write(string.Format(format, args) + "\n");
    }
}
