namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public delegate object NewObjDelegate(object[] args);

    public class ServiceMetadata
    {
        public Type ImplType { get; set; }

        public ConstructorInfo Constructor { get; set; }

        public Type[] ParameterTypes { get; internal set; }

        public NewObjDelegate NewObj { get; internal set; }

        public int ArgumentsLength { get; set; }

        public object[] ArgumentArray { get; set; }
        public ServiceMetadata[] ArgumentMetadataArray { get; set; }
    }
}
