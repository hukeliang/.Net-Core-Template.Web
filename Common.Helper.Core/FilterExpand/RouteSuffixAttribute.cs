using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helper.Core.FilterExpand
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RouteSuffixAttribute : Attribute, IRouteTemplateProvider
    {
        int? IRouteTemplateProvider.Order => _order;
        public int Order
        {
            get { return _order ?? 0; }
            set { _order = value; }
        }

        public string Name { get; set; }
        public string Template { get; }

        private int? _order;

        public RouteSuffixAttribute(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            Template = $"{template}.html";
        }

        public RouteSuffixAttribute(string template, string suffix)
        {
            if (template == null || suffix == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            Template = $"{template}.{suffix}";
        }
       

    }
}
