
using Mustache;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PfcAPI.Infrastucture.utility
{
    public static class CommonUtility
    {
        public static Random random = new Random();
        public static string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);

            return Convert.ToBase64String(hashedPassword);
        }
        public static string GenarateUserName(string name, int fileId)
        {
            var randomtext = RandomString(4);
            var _paddedName = name.Length > 4 ? name.Substring(0, 3) : name.PadRight(3, '0');
            var userCode = $"{_paddedName.Trim()?.ToUpper()}{"_"}{fileId}{randomtext}";
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
            try
            {
                string content = GetEmbeddedResource(filepath);
                string msg = CommonUtility.ParseMessage(content, payload);
                return msg;
            }
            catch (Exception)
            {

                throw;
            }
            //return string.Empty;
        }

        public static string GetEmbeddedResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            resourceName = CommonUtility.FormatResourceName(assembly, resourceName);
            //var names = this.GetType().Assembly.GetManifestResourceNames(resourceName);
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    return string.Empty;

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                var displayName = string.Empty;
                var displayNameAttr = prop?.GetCustomAttributes(typeof(DisplayNameAttribute), true);//?.Cast<DisplayNameAttribute>()?.Single()?.DisplayName;
                if (displayNameAttr.Length != 0)
                    displayName = (displayNameAttr[0] as DisplayNameAttribute).DisplayName;

                var objPropName = string.IsNullOrEmpty(displayName) ? prop.Name : displayName;
                dataTable.Columns.Add(objPropName, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
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

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(string.IsNullOrEmpty(cipherText) ? string.Empty : cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string MaskedString(string number)
        {
            var maskedNumberWithSpaces = string.Empty;
            if (!string.IsNullOrEmpty(number))
            {
                var lastDigits = number.Substring(number.Length - 4, 4);

                var requiredMask = new string('X', number.Length - lastDigits.Length);

                var maskedString = string.Concat(requiredMask, lastDigits);
                maskedNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");
            }
            return maskedNumberWithSpaces;
        }

        public static string ParseMessage<T1>(string message, T1 payload)
        {
            try
            {
                string content = message;

                if (!string.IsNullOrEmpty(content))
                {
                    FormatCompiler compiler = new FormatCompiler();
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
                    generator.ValueRequested += (sender, e) => e.Value = e.Value ?? "";

                    return generator.Render(payload);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return string.Empty;
        }
        public static byte[] ExportFromListToExcel<T>(List<T> table, string filename)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(filename);
            ws.Cells["A1"].LoadFromCollection(table, true, TableStyles.Light1);
            return pack.GetAsByteArray();
        }
        public static byte[] ExportFromDataTableToExcel(DataTable? table, string sheetName, string? filePassword)
        {
            using ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(sheetName);
            ws.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Light1);
            Byte[] bytedata = string.IsNullOrEmpty(filePassword) ? pack.GetAsByteArray() : pack.GetAsByteArray(filePassword);
            return bytedata;
        }
        public static byte[] ExportFromDataTableListToExcel(List<DataTable>? table)
        {
            using ExcelPackage pack = new ExcelPackage();
            table.ForEachWithIndex((data, index) =>
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add(string.Concat(data.TableName));
                ws.Cells["A1"].LoadFromDataTable(data, true, TableStyles.Light1);

            });
            //foreach(var (item, index) in table)
            //{
            //    var index= Current
            //    ExcelWorksheet ws = pack.Workbook.Worksheets.Add(sheetName);
            //    ws.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Light1);
            //}
            //ExcelWorksheet ws = pack.Workbook.Worksheets.Add(sheetName);
            //ws.Cells["A1"].LoadFromDataTable(table, true, TableStyles.Light1);
            //Byte[] bytedata = pack.GetAsByteArray("password");
            Byte[] bytedata = pack.GetAsByteArray();
            return bytedata;
        }
        public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            int idx = 0;
            foreach (T item in enumerable)
                handler(item, idx++);
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
            DateTime date = new DateTime(2020, month, 1);

            return date.ToString("MMM");
        }
    }

}
