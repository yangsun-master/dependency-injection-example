namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;

    public class AssemblyCreater
    {
        private AssemblyBuilder assemblyBuilder;
        private ModuleBuilder moduleBuilder;

        public AssemblyCreater()
        {
            var assemblyName = new AssemblyName("DynamicLibrary");
            this.assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save);
            this.moduleBuilder = this.assemblyBuilder.DefineDynamicModule("DynamicModule", "DynamicLibrary.dll");
        }

        public void CreateDynamicMethodCreaterType(List<ServiceMetadata> serviceMetadatas)
        {
            var typeBuilder = moduleBuilder.DefineType("DynamicLibrary.DynamicMethodCreater", TypeAttributes.Public);
            foreach (var serviceMetadata in serviceMetadatas)
            {
                var methodBuilder = typeBuilder.DefineMethod(
                    "New" + serviceMetadata.ImplType.Name,
                    MethodAttributes.Public | MethodAttributes.Static,
                    typeof(object),
                    new[] { typeof(object[])} );
                var il = methodBuilder.GetILGenerator();

                for (int i = 0; i < serviceMetadata.ParameterTypes.Length; i++)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Castclass, serviceMetadata.ParameterTypes[i]);
                }

                il.Emit(OpCodes.Newobj, serviceMetadata.Constructor);
                il.Emit(OpCodes.Ret);
            }

            typeBuilder.CreateType();
        }

        public void CreateLambdaExpressionCreaterType(List<ServiceMetadata> serviceMetadatas)
        {
            var typeBuilder = moduleBuilder.DefineType("DynamicLibrary.LambdaExpressionCreater", TypeAttributes.Public);
            foreach (var serviceMetadata in serviceMetadatas)
            {
                var methodBuilder = typeBuilder.DefineMethod(
                    "New" + serviceMetadata.ImplType.Name,
                    MethodAttributes.Public | MethodAttributes.Static,
                    typeof(object),
                    new[] { typeof(object[]) });
                var parametersExpr = Expression.Parameter(typeof(object[]));
                var constructorArgumentList = new List<Expression>();
                for (int i = 0; i < serviceMetadata.ParameterTypes.Length; i++)
                {
                    var indexExpr = Expression.Constant(i);
                    var arrayItemExpr = Expression.ArrayIndex(parametersExpr, indexExpr);
                    var castExpr = Expression.Convert(arrayItemExpr, serviceMetadata.ParameterTypes[i]);
                    constructorArgumentList.Add(castExpr);
                }

                var newExpr = Expression.New(serviceMetadata.Constructor, constructorArgumentList.ToArray());
                var lambda = Expression.Lambda<NewObjDelegate>(newExpr, parametersExpr);
                lambda.CompileToMethod(methodBuilder);
            }

            typeBuilder.CreateType();
        }

        public void Save()
        {
            this.assemblyBuilder.Save("DynamicLibrary.dll");
        }
    }
}
