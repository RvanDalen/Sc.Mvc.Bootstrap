using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using System.Collections;
using System.Linq;
using Sitecore.Shell.Applications.ContentEditor;

namespace Sc.Mvc.Bootstrap.Web.UI
{
    public class BootstrapClassTreelist : TreeList
    {
        protected new void Add()
        {
            if (Disabled)
                return;
            var viewStateString = GetViewStateString("ID");
            var treeviewEx = FindControl(viewStateString + "_all") as TreeviewEx;
            Assert.IsNotNull(treeviewEx, typeof(DataTreeview));
            var listbox = FindControl(viewStateString + "_selected") as Listbox;
            Assert.IsNotNull(listbox, typeof(Listbox));
            var selectionItem = treeviewEx.GetSelectionItem(Language.Parse(ItemLanguage), Sitecore.Data.Version.Latest);
            if (selectionItem == null)
            {
                SheerResponse.Alert("Select an item in the Content Tree.");
            }
            else
            {
                if (HasExcludeTemplateForSelection(selectionItem))
                    return;
                if (IsDeniedMultipleSelection(selectionItem, listbox))
                {
                    SheerResponse.Alert("You cannot select the same item twice.");
                }
                if (IsDeniedMultipleSelectionInSection(selectionItem, listbox))
                {
                    SheerResponse.Alert("You cannot select add another item from the same section.");
                }
                else
                {
                    if (!HasIncludeTemplateForSelection(selectionItem))
                        return;
                    SheerResponse.Eval("scForm.browser.getControl('" + viewStateString + "_selected').selectedIndex=-1");
                    var listItem = new ListItem();
                    listItem.ID = GetUniqueID("L");
                    Sitecore.Context.ClientPage.AddControl(listbox, listItem);
                    listItem.Header = GetHeaderValue(selectionItem);
                    listItem.Value = string.Format("{0}|{1}", listItem.ID, selectionItem.ID);
                    SheerResponse.Refresh(listbox);
                    SetModified();
                }
            }
        }

        private bool IsDeniedMultipleSelectionInSection(Item item, Listbox listbox)
        {
            Assert.ArgumentNotNull(listbox, "listbox");
            if (item == null || item.Parent == null)
                return true;

            var children = item.Parent.Children.Select(child => child.ID.ToString());

            foreach (Control control in listbox.Controls)
            {
                var strArray = control.Value.Split('|');
                if (strArray.Length >= 2 && children.Contains(strArray[1]))
                    return true;
            }
            return false;
        }


        private bool IsDeniedMultipleSelection(Item item, Listbox listbox)
        {
            Assert.ArgumentNotNull(listbox, "listbox");
            if (item == null)
                return true;
            if (AllowMultipleSelection)
                return false;
            foreach (Control control in listbox.Controls)
            {
                var strArray = control.Value.Split('|');
                if (strArray.Length >= 2 && strArray[1] == item.ID.ToString())
                    return true;
            }
            return false;
        }

        private bool HasExcludeTemplateForSelection(Item item)
        {
            if (item == null)
                return true;
            return HasItemTemplate(item, ExcludeTemplatesForSelection);
        }

        private bool HasIncludeTemplateForSelection(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            if (IncludeTemplatesForSelection.Length == 0)
                return true;
            return HasItemTemplate(item, IncludeTemplatesForSelection);
        }

        private static bool HasItemTemplate(Item item, string templateList)
        {
            Assert.ArgumentNotNull(templateList, "templateList");
            if (item == null || templateList.Length == 0)
                return false;
            var strArray = templateList.Split(',');
            var arrayList = new ArrayList(strArray.Length);
            for (var index = 0; index < strArray.Length; ++index)
                arrayList.Add(strArray[index].Trim().ToLowerInvariant());
            return arrayList.Contains(item.TemplateName.Trim().ToLowerInvariant());
        }
    }
}
