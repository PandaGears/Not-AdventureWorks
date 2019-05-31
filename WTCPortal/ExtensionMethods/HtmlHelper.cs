using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WTCPortal.ExtensionMethods
{
    public static class HtmlHelper
    {
        public static MvcHtmlString DisplayColumnNameFor<TModel, TClass, TProperty>(this HtmlHelper<TModel> helper,
            IEnumerable<TClass> model, Expression<Func<TClass, TProperty>> expression)

        {

            var name = ExpressionHelper.GetExpressionText(expression);

            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            
            try
            {
                var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(

                () => Activator.CreateInstance<TClass>(), typeof(TClass), name);



                var returnName = metadata.DisplayName;

                if (string.IsNullOrEmpty(returnName))

                    returnName = metadata.PropertyName;

                return new MvcHtmlString(returnName);

            }
            catch (ArgumentException e)
            {
             
                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
                throw e;
            }

        }
    }
}