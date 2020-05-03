namespace DependencyInjectionExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class LambdaExpressionServiceFactory : ServiceFactoryBase
    {
        public override NewObjDelegate CreateDelegate(ServiceMetadata serviceMetadata)
        {
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
            var newDelegate = lambda.Compile();
            return newDelegate;
        }
    }
}
