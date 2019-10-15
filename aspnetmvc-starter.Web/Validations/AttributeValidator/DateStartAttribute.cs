using System;
using System.ComponentModel.DataAnnotations;

namespace aspnetmvc_starter.Web.Validations.AttributeValidator
{
    public sealed class DateStartAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateStart = (DateTime)value;
            // Meeting must start in the future time.
            return (dateStart > DateTime.Now);
        }
    }
}