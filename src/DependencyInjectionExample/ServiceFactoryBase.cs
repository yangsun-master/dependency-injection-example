namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ServiceFactoryBase
    {
        protected Dictionary<Type, ServiceMetadata> serviceTypeToMetadata = new Dictionary<Type, ServiceMetadata>();

        public void AddService<TService, TImpl>() where TService : class where TImpl : class
        {
            Type serviceType = typeof(TService);
            ServiceMetadata serviceMetadata = new ServiceMetadata();
            serviceMetadata.ImplType = typeof(TImpl);
            serviceMetadata.Constructor = serviceMetadata.ImplType.GetConstructors()[0];
            serviceMetadata.ParameterTypes = serviceMetadata.Constructor.GetParameters().Select(p => p.ParameterType).ToArray();
            serviceMetadata.NewObj = this.CreateDelegate(serviceMetadata);
            this.serviceTypeToMetadata.Add(serviceType, serviceMetadata);
        }

        public abstract NewObjDelegate CreateDelegate(ServiceMetadata serviceMetadata);

        public TService GetService<TService>() where TService : class
        {
            var serviceType = typeof(TService);
            return (TService)this.GetService(serviceType);
        }

        public object GetService(Type serviceType)
        {
            ServiceMetadata serviceMetadata = this.serviceTypeToMetadata[serviceType];
            int length = serviceMetadata.ParameterTypes.Length;
            object[] parameterServices = new object[length];
            for (int i = 0; i < length; i++)
            {
                parameterServices[i] = this.GetService(serviceMetadata.ParameterTypes[i]);
            }

            return serviceMetadata.NewObj(parameterServices);
        }

        public List<ServiceMetadata> GetServiceMetadatas()
        {
            return this.serviceTypeToMetadata.Values.ToList();
        }
    }
}
