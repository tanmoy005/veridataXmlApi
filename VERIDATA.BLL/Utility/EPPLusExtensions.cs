using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace VERIDATA.BLL.utility
{
    public static class EPPLusExtensions
    {
        public static IEnumerable<T> ConvertTableToObjects<T>(this ExcelTable table) where T : new()
        {
            //DateTime Conversion
            Func<double, DateTime> convertDateTime = new(excelDate =>
            {
                if (excelDate < 1)
                {
                    throw new ArgumentException("Excel dates cannot be smaller than 0.");
                }

                DateTime dateOfReference = new(1900, 1, 1);

                if (excelDate > 60d)
                {
                    excelDate -= 2;
                }
                else
                {
                    excelDate--;
                }

                return dateOfReference.AddDays(excelDate);
            });

            //Get the properties of T
            List<System.Reflection.PropertyInfo> tprops = new T()
                .GetType()
                .GetProperties()
                .ToList();

            //Get the cells based on the table address
            ExcelCellAddress start = table.Address.Start;
            ExcelCellAddress end = table.Address.End;
            List<ExcelRangeBase> cells = new();

            //Have to use for loops insteadof worksheet.Cells to protect against empties
            for (int r = start.Row; r <= end.Row; r++)
            {
                for (int c = start.Column; c <= end.Column; c++)
                {
                    cells.Add(table.WorkSheet.Cells[r, c]);
                }
            }

            List<IGrouping<int, ExcelRangeBase>> groups = cells
                .GroupBy(cell => cell.Start.Row)
                .ToList();

            //Assume the second row represents column data types (big assumption!)
            List<Type?> types = groups
                .Skip(1)
                .First()
                .Select(rcell => rcell.Value?.GetType())
                .ToList();

            //Assume first row has the column names
            var colnames = groups
                .First()
                .Select((hcell, idx) => new { Name = hcell.Value.ToString(), index = idx })
                .Where(o => tprops.Select(p => p.Name).Contains(o.Name))
                .ToList();

            //Everything after the header is data
            IEnumerable<List<object>> rowvalues = groups
                .Skip(1) //Exclude header
                .Select(cg => cg.Select(c => c.Value).ToList());

            //Create the collection container
            IEnumerable<T> collection = rowvalues
                .Select(row =>
                {
                    T tnew = new();
                    colnames.ForEach(colname =>
                    {
                        //This is the real wrinkle to using reflection - Excel stores all numbers as double including int
                        object val = row[colname.index];
                        Type? type = types[colname.index];
                        System.Reflection.PropertyInfo prop = tprops.First(p => p.Name == colname.Name);

                        //If it is numeric it is a double since that is how excel stores all numbers
                        if (type == typeof(double))
                        {
                            if (!string.IsNullOrWhiteSpace(val?.ToString()))
                            {
                                //Unbox it
                                double unboxedVal = (double)val;

                                //FAR FROM A COMPLETE LIST!!!
                                if (prop.PropertyType == typeof(int))
                                {
                                    prop.SetValue(tnew, (int)unboxedVal);
                                }
                                else if (prop.PropertyType == typeof(double))
                                {
                                    prop.SetValue(tnew, unboxedVal);
                                }
                                else if (prop.PropertyType == typeof(DateTime))
                                {
                                    prop.SetValue(tnew, convertDateTime(unboxedVal));
                                }
                                else
                                {
                                    throw new NotImplementedException(string.Format("Type '{0}' not implemented yet!", prop.PropertyType.Name));
                                }
                            }
                        }
                        else
                        {
                            //Its a string
                            prop.SetValue(tnew, val);
                        }
                    });

                    return tnew;
                });


            //Send it back
            return collection;
        }

    }
}
