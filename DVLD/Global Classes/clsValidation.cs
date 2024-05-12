﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DVLD.Global_Classes
{
    public class clsValidation
    {
        public static bool EmailValidation(string emailAddress)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            var regex = new Regex(pattern);
            return regex.IsMatch(emailAddress);
        }
        public static bool ValidateInteger(string number) 
        {
            var pattern = @"^[0-9]*$";
            var regex = new Regex(pattern);
            return regex.IsMatch(number);
        }
        public static bool ValidateFloat(string Number)
        {
            var pattern = @"^[0-9]*(?:\.[0-9]*)?$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }
        public static bool IsNumber(string Number) 
        { 
        return (ValidateInteger(Number)||ValidateFloat(Number));
        }
    }
}
