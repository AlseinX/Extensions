using System;
using System.Collections.Generic;
using System.Linq;

namespace Alsein.Extensions.DependencyInjection.Internal
{
    internal sealed class Container : Scope, IContainer
    {
        public Container() : base(default)
        {
        }
    }
}