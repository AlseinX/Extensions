namespace Alsein.Extensions.Runtime.InteropServices
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    /// <typeparam name="TVar"></typeparam>
    public static class P<TModule, TVar>
    {
        /// <summary>
        /// 
        /// </summary>
        public static IIndexer<string, TVar> Var { get; } = new Indexer();

        private class Indexer : IIndexer<string, TVar>
        {
            public TVar this[string name]
            {
                get => (TVar)P.GetModule(typeof(TModule)).GetGlobalVariable(name, typeof(TVar));
                set => P.GetModule(typeof(TModule)).SetGlobalVariable(name, value);
            }
        }
    }
}
