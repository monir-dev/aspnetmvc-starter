using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace aspnetmvc_starter.Web.Utility
{
    public static class SelectListItemExtension
    {
        /// <summary>
        /// Dropdown Selected List Show
        /// Create by : Rasel 
        /// </summary>
        /// <typeparam name="T">Any Type Object</typeparam>
        /// <param name="objectList">Object Collection</param>
        /// <param name="valueField">Dropdown Value Field</param>
        /// <param name="textField">Dropdown Text Field</param>
        /// <param name="isEdit">Used For Edit. If yes write 'true' else write 'false'</param>
        /// <param name="selectedValue">Dropdown Selected Value</param>
        /// <param name="selectText">Dropdown Selected Value</param>
        /// <returns></returns>
        public static List<SelectListItem> PopulateDropdownList<T>(this List<T> objectList, string valueField, string textField, bool isEdit = false, string selectedValue = "0", string selectText = "- Select One -")
        {
            try
            {
                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = "0";
                }
                var selectedList = new SelectList(objectList, valueField, textField);
                List<SelectListItem> items;
                IEnumerable<SelectListItem> listOfItems;
                if (isEdit)
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = (obj.Value == selectedValue), Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();
                    items.Add(selectedValue == "0"
                                  ? new SelectListItem { Text = selectText, Value = "0", Selected = true }
                                  : new SelectListItem { Text = selectText, Value = "0", Selected = false });
                }
                else
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = false, Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();
                    
                    if (!string.IsNullOrEmpty(selectText))
                        items.Add(new SelectListItem { Text = selectText, Value = selectedValue, Selected = true });
                    else items.Add(new SelectListItem { Text = " ", Value = "", Selected = false });
                }

                return items.OrderBy(s => s.Text).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<SelectListItem> PopulateNullableDropdownList<T>(this List<T> objectList, string valueField, string textField, bool isEdit = false, string selectedValue = "", string selectText = "- Select One -")
        {
            try
            {
                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = "";
                }
                var selectedList = new SelectList(objectList, valueField, textField);
                List<SelectListItem> items;
                IEnumerable<SelectListItem> listOfItems;
                if (isEdit)
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = (obj.Value == selectedValue), Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();
                    items.Add(selectedValue == ""
                        ? new SelectListItem { Text = selectText, Value = "", Selected = true }
                        : new SelectListItem { Text = selectText, Value = "", Selected = false });
                }
                else
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = false, Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();

                    if (!string.IsNullOrEmpty(selectText))
                        items.Add(new SelectListItem { Text = selectText, Value = selectedValue, Selected = true });
                    else items.Add(new SelectListItem { Text = " ", Value = "", Selected = false });
                }

                return items.OrderBy(s => s.Text).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<SelectListItem> PopulateOrganizationTypeDropdownList<T>(this List<T> objectList, string valueField, string textField, bool isEdit = false, string selectedValue = "0", string selectText = "- Select Organization Type -")
        {
            try
            {
                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = "0";

                }
                var selectedList = new SelectList(objectList, valueField, textField);
                List<SelectListItem> items;
                IEnumerable<SelectListItem> listOfItems;
                if (isEdit)
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = (obj.Value == selectedValue), Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();
                    items.Add(selectedValue == "0"
                                  ? new SelectListItem { Text = selectText, Value = "0", Selected = true }
                                  : new SelectListItem { Text = selectText, Value = "0", Selected = false });
                }
                else
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = false, Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();
                    items.Add(new SelectListItem { Text = selectText, Value = selectedValue, Selected = true });
                }

                //return items.OrderByDescending(s => s.Selected).ToList();
                return items.OrderBy(s => s.Text).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<SelectListItem> PopulateOrganizationTypeNameDropdownList<T>(this List<T> objectList, string valueField, string textField, bool isEdit = false, string selectedValue = "0", string selectText = "- Select Organization Type Name -")
        {
            try
            {
                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = "0";

                }
                var selectedList = new SelectList(objectList, valueField, textField);
                List<SelectListItem> items;
                IEnumerable<SelectListItem> listOfItems;
                if (isEdit)
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = (obj.Value == selectedValue), Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();
                    items.Add(selectedValue == "0"
                                  ? new SelectListItem { Text = selectText, Value = "0", Selected = true }
                                  : new SelectListItem { Text = selectText, Value = "0", Selected = false });
                }
                else
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = false, Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();
                    items.Add(new SelectListItem { Text = selectText, Value = selectedValue, Selected = true });
                }

                //return items.OrderByDescending(s => s.Selected).ToList();
                return items.OrderBy(s => s.Text).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}