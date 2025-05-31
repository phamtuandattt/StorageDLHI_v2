using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StorageDLHI.Infrastructor.Commons
{
    public static class Common
    {
        public const string REGEX_VALID_VIETNAM_ADDRESS = @"[^\p{L}0-9\s,./-]";
        public const string REGEX_VALID_NAME = @"[^\p{L}\s]";
        public const string REGEX_VALID_CERT = @"[^0-9-]";
        public const string REGEX_VALID_PHONE = @"[^0-9-]";
        public const string REGEX_VALID_VIETTAT = @"[^a-zA-Z-]";
        public const string REGEX_VALID_EMAIL = @"[^a-zA-Z0-9._%+\-@]";
        public const string REGEX_VALID_DES = @"[^\p{L}0-9\s,./()\%\-]";
        public const string REGEX_VALID_CODE = @"[^A-Z0-9-]";
        public const string REGEX_VALID_DIGIT = @"[^0-9]";

        public static Int32 CheckOrReturnNumber(string numberString)
        {
            return !string.IsNullOrEmpty(numberString.Trim())
                && numberString.Trim().Length > 0
                ? Int32.Parse(numberString.Trim()) : 0;
        }

        public static bool IsValidVietnameseAddress(string input)
        {
            var pattern = @"^[\p{L}0-9\s,./-]+$";
            return Regex.IsMatch(input, pattern);
        }

    }
}
