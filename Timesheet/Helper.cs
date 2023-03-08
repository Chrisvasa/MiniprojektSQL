using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Timesheet
{
    public class Helper
    {
        // Formats the string to First letter being uppercase, and the rest in lowercase
        public static string FormatString(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower();
            }
            return input;
        }

        public static bool isValid(string input)
        {
            return Regex.IsMatch(input, @"^[a-öA-Ö]+$");
        }

        public static bool Confirm()
        {
            while(true)
            {
                char userInput = Console.ReadKey(true).KeyChar;
                string answer = userInput.ToString().ToUpper();

                if (answer.Substring(0, 1) == "Y")
                {
                    return true;
                }
                else if(answer.Substring(0, 1) == "N")
                {
                    return false;
                }
            }
        }
    }
}
