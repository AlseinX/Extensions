using System;

namespace Alsein.Utilities.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="genericArgs"></param>
    /// <param name="valueArgs"></param>
    /// <returns></returns>
    public delegate object VariableArgsHandler(Type[] genericArgs, object[] valueArgs);
}