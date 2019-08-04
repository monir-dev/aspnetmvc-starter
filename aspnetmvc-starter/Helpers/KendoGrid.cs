using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using aspnetmvc_starter.Dtos;

namespace aspnetmvc_starter.Helpers
{
    public class KendoGrid
    {
        public static void GridParams(out int take, out int skip, out int page, out int pageSize, out string OrderByField, out string OrderByType, out List<KendoFilter> filters, HttpRequestBase Request)
        {
            take = int.Parse(Request.Params["take"]);
            skip = int.Parse(Request.Params["skip"]);
            page = int.Parse(Request.Params["page"]);
            pageSize = int.Parse(Request.Params["pageSize"]);
            
            OrderByField = !String.IsNullOrEmpty(Request.Params["sort[0][field]"]) ? Request.Params["sort[0][field]"] : "";
            OrderByType = !String.IsNullOrEmpty(Request.Params["sort[0][dir]"]) ? Request.Params["sort[0][dir]"] : "";
            
            var localFilters = new List<KendoFilter>();
            foreach (var filterName in Request.Params)
            {
                if (Regex.IsMatch(filterName.ToString(), @"filter\[filters\]\[\d\]\[value\]"))
                {
                    Match match = Regex.Match(filterName.ToString(), @"(\d)");
                    var i = int.Parse(match.Groups[0].Value);

                    var filter = new KendoFilter();
                    filter.Name = Request.Params[$"filter[filters][{i}][field]"];
                    filter.Operator = Request.Params[$"filter[filters][{i}][operator]"];
                    filter.Value = Request.Params[$"filter[filters][{i}][value]"];
                    localFilters.Add(filter);
                }
            }

            filters = localFilters;
        }

        public static string GenerateButtons(string action, int id, bool edit, bool delete, bool view)
        {
            StringBuilder buttons = new StringBuilder();

            if (view == true)
            {
                buttons.Append($"<a href='/{action}/View/{id}' class='btn btn-outline-info grid-row-view'><i class='far fa-eye'></i></a>");
            }

            if (edit == true)
            {
                buttons.Append($" <a href='/{action}/Edit/{id}' class='btn btn-outline-warning grid-row-edit'><i class='far fa-edit'></i></a>");
            }

            if (delete == true)
            {
                buttons.Append($" <a href='/{action}/Delete/{id}' class='btn btn-outline-danger grid-row-delete'><i class='far fa-trash-alt'></i></a>");
            }
            
            return buttons.ToString();
        }
    }
}