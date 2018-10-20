using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Abstractions
{
    public class ListenConfig
    {
        public string Name { get; set; }
        public Types Type { get; set; }

        public Privacy Privacy { get; set; }

        public Scope Scope { get; set; }

        public Type ReflectionType { get; set; }
        public Func<IDispatchRequest, object> Map { get; set; }
        public Type Handler { get; set;}
    }
}