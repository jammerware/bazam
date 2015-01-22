using System;

namespace BazamWPF.ViewModels
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RelatedPropertyAttribute : Attribute
    {
        public string RelatedPropertyName { get; set; }

        public RelatedPropertyAttribute(string relatedPropertyName)
        {
            this.RelatedPropertyName = relatedPropertyName;
        }
    }
}