using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Mustache;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using VERIDATA.Model.Configuration;
using static VERIDATA.DAL.utility.CommonEnum;

namespace VERIDATA.BLL.utility
{
    public static class CommonUtility
    {
        public static Random random = new();
        public static ApiConfiguration config;
        private static readonly Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

        public static void Initialize(ApiConfiguration Configuration)
        {
            config = Configuration;
        }

        public static string hashPassword(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] asByteArray = Encoding.Default.GetBytes(password);
            byte[] hashedPassword = sha.ComputeHash(asByteArray);

            return Convert.ToBase64String(hashedPassword);
        }

        public static string GenarateUserName(string name, int fileId)
        {
            string randomtext = RandomString(4);
            string _paddedName = name.Length > 4 ? name[..3] : name.PadRight(3, '0');
            string userCode = $"{_paddedName.Trim()?.ToUpper()}{"_"}{fileId}{randomtext}";
            return userCode;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateRandomPassword(int length)
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");

            return GuidString;
        }

        public static string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." +
                resourceName
                    .Replace(" ", "_")
                    .Replace("\\", ".")
                    .Replace("/", ".");
        }

        public static string ParseHtmlData<T1>(string filepath, T1 payload)
        {
            string content = GetEmbeddedResource(filepath);
            string msg = CommonUtility.ParseMessage(content, payload);
            return msg;
            //return string.Empty;
        }

        public static string GetEmbeddedResource(string resourceName)
        {
            string embededResource = string.Empty;
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (string.IsNullOrEmpty(resourceName))
            {
                resourceName = CommonUtility.FormatResourceName(assembly, resourceName);
                using Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
                if (resourceStream == null)
                {
                    return string.Empty;
                }

                using StreamReader reader = new(resourceStream);

                embededResource = reader.ReadToEnd();
            }
            return embededResource;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table
                Type? type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
                //Setting column names as Property names
                string displayName = string.Empty;
                object[]? displayNameAttr = prop?.GetCustomAttributes(typeof(DisplayNameAttribute), true);//?.Cast<DisplayNameAttribute>()?.Single()?.DisplayName;
                if (displayNameAttr.Length != 0)
                {
                    displayName = (displayNameAttr[0] as DisplayNameAttribute).DisplayName;
                }

                string objPropName = string.IsNullOrEmpty(displayName) ? prop.Name : displayName;
                _ = dataTable.Columns.Add(objPropName, type);
            }
            foreach (T item in items)
            {
                object[] values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                _ = dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static string CustomEncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            if (!string.IsNullOrEmpty(cipherText))
            {
                byte[] buffer = Convert.FromBase64String(string.IsNullOrEmpty(cipherText) ? string.Empty : cipherText);

                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new(cryptoStream);
                return streamReader.ReadToEnd();
            }
            return cipherText;
        }

        public static string MaskedString(string number)
        {
            string maskedNumberWithSpaces = string.Empty;
            if (!string.IsNullOrEmpty(number))
            {
                string lastDigits = number.Substring(number.Length - 4, 4);

                string requiredMask = new('X', number.Length - lastDigits.Length);

                string maskedString = string.Concat(requiredMask, lastDigits);
                maskedNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");
            }
            return maskedNumberWithSpaces;
        }

        public static string ParseMessage<T1>(string message, T1 payload)
        {
            string content = message;

            if (!string.IsNullOrEmpty(content))
            {
                FormatCompiler compiler = new();
                Generator generator = compiler.Compile(content);

                generator.KeyNotFound += (sender, e) =>
                {
                    e.Substitute = "";
                    e.Handled = true;
                };
                generator.KeyFound += (sender, e) =>
                {
                    e.Substitute = e.Substitute == null ? "" : e.Substitute.ToString() == "True" ?
                        Convert.ToString(e.Substitute).ToLowerInvariant() : e.Substitute;
                };
                generator.ValueRequested += (sender, e) => e.Value ??= "";

                return generator.Render(payload);
            }
            return string.Empty;
        }

        public static byte[] ExportFromListToExcel<T>(List<T> table, string filename)
        {
            using ExcelPackage pack = new();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            _ = ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Light1);
            return pack.GetAsByteArray();
        }

        public static byte[] ExportFromDataTableToExcel(DataTable? table, string sheetName, string? filePassword)
        {
            string? password = string.IsNullOrEmpty(filePassword) ? filePassword : DecryptString(config.EncriptKey, filePassword);
            using ExcelPackage pack = new();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(sheetName);
            _ = ws.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Light1);
            byte[] bytedata = string.IsNullOrEmpty(filePassword) ? pack.GetAsByteArray() : pack.GetAsByteArray(password);
            return bytedata;
        }

        public static byte[] ExportFromDataTableListToExcel(List<DataTable>? table)
        {
            using ExcelPackage pack = new();
            table.ForEachWithIndex((data, index) =>
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add(string.Concat(data.TableName));
                _ = ws.Cells["A1"].LoadFromDataTable(data, true, TableStyles.Light1);
            });

            byte[] bytedata = pack.GetAsByteArray();
            return bytedata;
        }

        public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            int idx = 0;
            foreach (T item in enumerable)
            {
                handler(item, idx++);
            }
        }

        public static bool IsEmailValidate(string? EmailId)
        {
            bool isEmail = false;
            if (!string.IsNullOrEmpty(EmailId))
            {
                isEmail = Regex.IsMatch(EmailId, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            }
            return isEmail;
        }

        public static string getMonthName(int month)
        {
            DateTime date = new(2020, month, 1);

            return date.ToString("MMM");
        }

        public static string GetMonthYearFullName(string monthYear, string type, string format)
        {
            // Parse the input string to DateTime
            DateTime date = DateTime.ParseExact(monthYear, format, CultureInfo.InvariantCulture);
            if (type.Equals("month", StringComparison.OrdinalIgnoreCase))
            {
                return date.ToString("MMMM");
            }
            // Return the year if type is "year"
            else if (type.Equals("year", StringComparison.OrdinalIgnoreCase))
            {
                return date.ToString("yyyy");
            }
            else
            {
                throw new ArgumentException("Invalid type. Expected 'month' or 'year'.");
            }
            // Return the full month name (e.g., "May", "June")
        }

        public static string GetMailType(string Type)
        {
            string mailtype = string.Empty;
            if (!string.IsNullOrEmpty(Type))
            {
                mailtype = Type switch
                {
                    RemarksType.Adhaar => MailType.AdhrValidation,
                    RemarksType.UAN => MailType.UANValidation,
                    RemarksType.Passport => MailType.Passport,
                    RemarksType.Pan => MailType.Pan,
                    RemarksType.Others => MailType.Others,
                    RemarksType.Manual => MailType.Manual,
                    _ => string.Empty,
                };
            }
            return mailtype;
        }

        public static bool CheckMobileNumber(string mobile, string hashMobileNo, string sharePhrase, string aadhaarLast4Digits)
        {
            bool isMobileNumberMatched = false;
            string concatenatedString = mobile + sharePhrase;
            string hashText = Sha256Hash(concatenatedString);
            int lastAadhaarChar = int.Parse(aadhaarLast4Digits[^1].ToString());
            if (lastAadhaarChar > 1)
            {
                for (int i = 2; i <= lastAadhaarChar; i++)
                {
                    hashText = Sha256Hash(hashText);
                }
            }

            //string finalHash = RepeatSha256Hash(initialHash, lastDigit - 1);
            if (hashText == hashMobileNo)
            {
                isMobileNumberMatched = true;
            }
            return isMobileNumberMatched;
        }

        public static string Sha256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public static string RepeatSha256Hash(string input, int additionalHashes)
        {
            byte[] hashBytes = Encoding.UTF8.GetBytes(input);

            using (SHA256 sha256 = SHA256.Create())
            {
                for (int i = 0; i < additionalHashes; i++)
                {
                    hashBytes = sha256.ComputeHash(hashBytes);
                }

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public static string NormalizeWhitespace(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return System.Text.RegularExpressions.Regex.Replace(input?.Trim(), @"\s+", " ");
        }
    }
}