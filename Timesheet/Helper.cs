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
            if (!string.IsNullOrWhiteSpace(input)) // If string is not empty
            {
                return input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower();
            }
            return input;
        }

        // Checks if a given string only has allowed letters from a-ö
        public static bool isValid(string input)
        {
            return Regex.IsMatch(input, @"^[a-öA-Ö]+$"); // If its a match, returns the string
        }

        // Method that prompts user to confirm, and then returns answer as true or false
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
