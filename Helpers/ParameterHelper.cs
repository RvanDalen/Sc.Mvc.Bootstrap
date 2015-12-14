using System.Text;
using Sitecore.Mvc.Presentation;

namespace Sc.Mvc.Bootstrap.Helpers
{
    public static class ParameterHelper
    {
        public static string GetClassNames(this RenderingParameters renderingParameters, string key, string defaultValue = null)
        {
            var names = new StringBuilder();
            var database = Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database;

            foreach (var reference in (renderingParameters[key] + "").Split('|'))
            {
                var item = database.GetItem(reference);
                if (item != null) names.AppendFormat(" {0}", item.Name);
            }

            if (names.Length == 0) names.Append(defaultValue);

            return names.ToString().Trim();
        }
    }
}
