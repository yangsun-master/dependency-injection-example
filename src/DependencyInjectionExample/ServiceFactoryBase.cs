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
            serviceMetadata.ArgumentsLength = serviceMetadata.ParameterTypes.Length;
            serviceMetadata.ArgumentArray = new object[serviceMetadata.ArgumentsLength];
            serviceMetadata.ArgumentMetadataArray = new ServiceMetadata[serviceMetadata.ArgumentsLength];
            serviceMetadata.NewObj = this.CreateDelegate(serviceMetadata);
            this.serviceTypeToMetadata.Add(serviceType, serviceMetadata);
        }

        public abstract NewObjDelegate CreateDelegate(ServiceMetadata serviceMetadata);

        public void Build()
        {
            foreach (var serviceMetadata in this.serviceTypeToMetadata.Values)
            {
                for (int i = 0; i < serviceMetadata.ArgumentsLength; i++)
                {
                    serviceMetadata.ArgumentMetadataArray[i] = this.serviceTypeToMetadata[serviceMetadata.ParameterTypes[i]];
                }
            }
        }

        public TService GetService<TService>() where TService : class
        {
            var serviceType = typeof(TService);
            return (TService)this.GetService(serviceType);
        }

        public object GetService(Type serviceType)
        {
            ServiceMetadata serviceMetadata = this.serviceTypeToMetadata[serviceType];
            return this.GetService(serviceMetadata);
        }

        private object GetService(ServiceMetadata serviceMetadata)
        {
            for (int i = 0; i < serviceMetadata.ArgumentsLength; i++)
            {
                serviceMetadata.ArgumentArray[i] = this.GetService(serviceMetadata.ArgumentMetadataArray[i]);
            }

            return serviceMetadata.NewObj(serviceMetadata.ArgumentArray);
        }

        public List<ServiceMetadata> GetServiceMetadatas()
        {
            return this.serviceTypeToMetadata.Values.ToList();
        }
    }
}
