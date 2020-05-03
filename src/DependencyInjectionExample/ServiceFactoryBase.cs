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
            ServiceMetadata serviceMetadata;
            if (!this.serviceTypeToMetadata.TryGetValue(serviceType, out serviceMetadata))
            {
                return null;
            }

            List<object> parameterServices = new List<object>();
            foreach (var paramType in serviceMetadata.ParameterTypes)
            {
                var parameterService = this.GetService(paramType);
                parameterServices.Add(parameterService);
            }

            return serviceMetadata.NewObj(parameterServices.ToArray());
        }

        public List<ServiceMetadata> GetServiceMetadatas()
        {
            return this.serviceTypeToMetadata.Values.ToList();
        }
    }
}
