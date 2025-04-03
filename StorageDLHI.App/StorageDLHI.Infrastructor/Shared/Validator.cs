using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StorageDLHI.Infrastructor.Shared
{
    public static class Validator
    {
        // Method to validate email
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        // Method to validate phone number
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            // Example pattern for US phone numbers (can be adjusted as needed)
            string phonePattern = @"^\+?[1-9]\d{1,14}$"; // E.164 format
            return Regex.IsMatch(phoneNumber, phonePattern);
        }

        // Method to validate if a string is digital (numeric)
        public static bool IsDigital(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string digitalPattern = @"^\d+$"; // Only digits
            return Regex.IsMatch(input, digitalPattern);
        }
    }
}
