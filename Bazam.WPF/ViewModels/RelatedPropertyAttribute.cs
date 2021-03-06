﻿using System;

namespace Bazam.Wpf.ViewModels
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RelatedPropertyAttribute : Attribute
    {
        public string RelatedPropertyName { get; set; }

        public RelatedPropertyAttribute(string relatedPropertyName)
        {
            this.RelatedPropertyName = relatedPropertyName;
        }
    }
}