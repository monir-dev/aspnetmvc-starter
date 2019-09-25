using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace aspnetmvc_starter.Web.Validations.AttributeValidator
{
    public sealed class DateEndAttribute : ValidationAttribute
    {
        public string DateStartProperty { get; set; }
        public override bool IsValid(object value)
        {
            // Get Value of the DateStart property
            string dateStartString = HttpContext.Current.Request[DateStartProperty];
            DateTime dateEnd = (DateTime)value;
            //DateTime dateStart = DateTime.Parse(dateStartString);
            DateTime dateStart = Convert.ToDateTime(dateStartString);

            // Meeting start time must be before the end time
            return dateStart <= dateEnd;
        }


    }
}