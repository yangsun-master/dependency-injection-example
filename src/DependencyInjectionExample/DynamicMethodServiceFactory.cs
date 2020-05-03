namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    public class DynamicMethodServiceFactory : ServiceFactoryBase
    {
        public override NewObjDelegate CreateDelegate(ServiceMetadata serviceMetadata)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(
                "NewInstance",
                typeof(object),
                new Type[] { typeof(object[]) });
            var il = dynamicMethod.GetILGenerator();

            for (int i = 0; i < serviceMetadata.ParameterTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                il.Emit(OpCodes.Castclass, serviceMetadata.ParameterTypes[i]);
            }

            il.Emit(OpCodes.Newobj, serviceMetadata.Constructor);
            il.Emit(OpCodes.Ret);
            var newObj = (NewObjDelegate)dynamicMethod.CreateDelegate(typeof(NewObjDelegate));
            return newObj;
        }
    }
}
