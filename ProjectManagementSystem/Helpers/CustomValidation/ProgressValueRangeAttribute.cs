using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Helpers.CustomValidation
{
    public class ProgressValueRangeAttribute : ValidationAttribute
    {
        private int Minimum { get; set; }
        private int Maximum { get; set; }

        public ProgressValueRangeAttribute()
        {
            Minimum = 0;
            Maximum = 100;
        }


        public override bool IsValid(object value)
        {
            int val = Convert.ToInt32(value);
            if (value == null) return true;


            return val >= Minimum && val <= Maximum;
        }
    }
}