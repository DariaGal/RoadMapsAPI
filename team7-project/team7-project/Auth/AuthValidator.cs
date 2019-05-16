using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace team7_project.Auth
{
    public static class AuthValidator
    {
        public static bool Password(string password, out string errorMessage)
        {
            var passwordRegex = new Regex(@"^[A-Za-z0-9!@#$%^&*()_+=\[{\]};:<>|./?,'-]+$");
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            errorMessage = string.Empty;
            bool isValidated = true;

            if (!passwordRegex.IsMatch(password))
            {
                isValidated = false;
                errorMessage += "Password can have latin symbols, numbers and special characters. ";
            }

            if (!hasMinimum8Chars.IsMatch(password))
            {
                isValidated = false;
                errorMessage += "Password should not be less than 8 characters. ";
            }

            if (!hasNumber.IsMatch(password))
            {
                isValidated = false;
                errorMessage += "Password should contain At least one number. ";
            }

            if(!hasUpperChar.IsMatch(password))
            {
                isValidated = false;
                errorMessage += "Password should contain At least one upper case letter.";
            }

            return isValidated;
        }

        public static bool Login(string login,out string errorMessage)
        {
            var loginRegex = new Regex(@"^[A-Za-z0-9_-]+$");

            var isValidated = true;
            errorMessage = string.Empty;

            if(!loginRegex.IsMatch(login))
            {
                isValidated = false;
                errorMessage = "Login can contain latin symbols, numbers and characters \"-\", \"_\". ";
            }

            return isValidated;
        }
    }
}
