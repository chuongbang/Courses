using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Course.Core.Ultis
{
    public sealed class Property<T>
    {
        public string Name(Expression<Func<T, object>> expression)
        {
            return NameProperty<object>(expression);
        }
        public string Name(Expression<Func<T, int>> expression)
        {
            return NameProperty<int>(expression);
        }
        public string Name(Expression<Func<T, int?>> expression)
        {
            return NameProperty<int?>(expression);
        }
        public string Name(Expression<Func<T, DateTime>> expression)
        {
            return NameProperty<DateTime>(expression);
        }
        public string Name(Expression<Func<T, DateTime?>> expression)
        {
            return NameProperty<DateTime?>(expression);
        }        
        public string Name(Expression<Func<T, bool>> expression)
        {
            return NameProperty<bool>(expression);
        }        
        public string Name(Expression<Func<T, bool?>> expression)
        {
            return NameProperty<bool?>(expression);
        }        
        public string Name(Expression<Func<T, decimal>> expression)
        {
            return NameProperty<decimal>(expression);
        }        
        public string Name(Expression<Func<T, decimal?>> expression)
        {
            return NameProperty<decimal?>(expression);
        }

        public string NameProperty<TR>(Expression<Func<T, TR>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("'property' không có body");
            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null
                || propertyInfo.ReflectedType == null)
                throw new ArgumentException(string.Format("Expression '{0}' can't be cast to an Operand.", expression));
            return propertyInfo.Name;
        }

        public PropertyInfo GetProperty<TR>(Expression<Func<T, TR>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("'property' không có body");
            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null
                || propertyInfo.ReflectedType == null)
                throw new ArgumentException(string.Format("Expression '{0}' can't be cast to an Operand.", expression));
            return propertyInfo;
        }

        public string ActionName(Expression<Func<T, Action>> expression)
        {
            return MethodName(expression);
        }

        public string ActionName<TParam>(Expression<Func<T, Action<TParam>>> expression)
        {
            return MethodName(expression);
        }

        public string FuncName<TResult>(Expression<Func<T, Func<TResult>>> expression)
        {
            return MethodName(expression);
        }

        public string FuncName<TParam, TResult>(Expression<Func<T, Func<TParam, TResult>>> expression)
        {
            return MethodName(expression);
        }

        private bool IsNET45 = Type.GetType("System.Reflection.ReflectionContext", false) != null;

        private string MethodName(LambdaExpression expression)
        {
            var unaryExpression = (UnaryExpression)expression.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;

            if (IsNET45)
            {
                var methodCallObject = (ConstantExpression)methodCallExpression.Object;
                var methodInfo = (MethodInfo)methodCallObject.Value;
                return methodInfo.Name;
            }
            else
            {
                var methodInfoExpression = (ConstantExpression)methodCallExpression.Arguments.Last();
                var methodInfo = (MemberInfo)methodInfoExpression.Value;
                return methodInfo.Name;
            }

        }

    }
}
