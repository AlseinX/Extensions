using System;

namespace Alsein.Utilities.Runtime
{
    public interface IDynamicInvocable
    {
        object DispatchInvocation(int methodId, Type[] genericArgs, object[] valueArgs);
    }
}