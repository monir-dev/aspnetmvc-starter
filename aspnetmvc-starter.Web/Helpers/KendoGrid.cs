using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.WebPages;
using aspnetmvc_starter.Web.Dtos;

namespace aspnetmvc_starter.Web.Helpers
{
    public class KendoGrid<T> where T : class
    {
        public static IEnumerable<T> Grid(IQueryable<T> query, HttpRequestBase Request, out int skip, out int pageSize)
        {
            return GridFilterAndOrder(out skip, out pageSize, query, Request);
        }

        public static IEnumerable<T> GridFilterAndOrder(out int skip, out int pageSize, IEnumerable<T> query, HttpRequestBase Request)
        {
            var take = 0;
            var page = 0;
            string OrderByField = "";
            string OrderByType = "";
            List<KendoFilter> filters = new List<KendoFilter>();

            GridParams(out take, out skip, out page, out pageSize, out OrderByField, out OrderByType, out filters, Request);


            if (!String.IsNullOrEmpty(OrderByField) && !String.IsNullOrEmpty(OrderByType))
            {
                query = (OrderByType == "desc")
                    ? query.OrderByDescending(o => o.GetProperty(OrderByField))
                    : query.OrderBy(o => o.GetProperty(OrderByField));
            }

            foreach (var filter in filters)
            {
                if (filter.Operator == "eq")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString() == filter.Value);
                }
                else if (filter.Operator == "neq")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString() != filter.Value);
                }
                else if (filter.Operator == "startswith")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString().StartsWith(filter.Value));
                }
                else if (filter.Operator == "endswith")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString().EndsWith(filter.Value));
                }
                else if (filter.Operator == "contains")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null &&
                        o.GetProperty(filter.Name).ToString().ToLower().Contains(filter.Value.ToLower()));
                }
                else if (filter.Operator == "doesnotcontain")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null &&
                        !o.GetProperty(filter.Name).ToString().ToLower().Contains(filter.Value.ToLower()));
                }
                else if (filter.Operator == "isnull")
                {
                    query = query.Where(
                        o => o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString() == null);
                }
                else if (filter.Operator == "isnotnull")
                {
                    query = query.Where(
                        o => o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString() != null);
                }
                else if (filter.Operator == "isempty")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && o.GetProperty(filter.Name).ToString().IsEmpty());
                }
                else if (filter.Operator == "isnotempty")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && !o.GetProperty(filter.Name).ToString().IsEmpty());
                }
                else if (filter.Operator == "isnullorempty")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && String.IsNullOrEmpty(o.GetProperty(filter.Name).ToString()));
                }
                else if (filter.Operator == "isnotnullorempty")
                {
                    query = query.Where(o =>
                        o.GetProperty(filter.Name) != null && !String.IsNullOrEmpty(o.GetProperty(filter.Name).ToString()));
                }
            }

            return query;
        }

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