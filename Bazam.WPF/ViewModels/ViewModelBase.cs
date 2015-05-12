using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Bazam.WPF.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void ChangeProperty<T>(Expression<Func<T, object>> property, object value) where T : ViewModelBase
        {
            MemberExpression member = property.Body as MemberExpression;

            if (member == null) {
                UnaryExpression unary = property.Body as UnaryExpression;
                if (unary == null || (member = unary.Operand as MemberExpression) == null)
                    throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", property.ToString()));
            }

            PropertyInfo propInfo = member.Member as PropertyInfo;
            FieldInfo fieldInfo = member.Member as FieldInfo;

            if (propInfo == null && fieldInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' doesn't refer to a field or a property.", property.ToString()));

            if (propInfo == null) {
                propInfo = propInfo.ReflectedType.GetProperty(fieldInfo.Name.Substring(1));
                if (propInfo == null)
                    throw new ArgumentException(string.Format("Expression '{0}' refers to a field, but a property with the name equal to the field's name excluding the leading '_' could not be found.", property.ToString()));
            }
            else {
                fieldInfo = propInfo.ReflectedType.GetField("_" + propInfo.Name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo == null)
                    throw new ArgumentException(string.Format("Expression '{0}' refers to a property, but a field with the name equal to the property's name with a leading '_' could not be found.", property.ToString()));
            }

            object oldValue = fieldInfo.GetValue(this);
            if (oldValue != value) {
                fieldInfo.SetValue(this, value);

                RaisePropertyChanged(propInfo.Name);

                IEnumerable<RelatedPropertyAttribute> relatedProperties = propInfo.GetCustomAttributes<RelatedPropertyAttribute>();
                if (relatedProperties != null) {
                    foreach (RelatedPropertyAttribute r in relatedProperties) {
                        RaisePropertyChanged(r.RelatedPropertyName);
                    }
                }
            }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}