using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System;

namespace CoreBusiness
{
    public class FilesCoreBusiness
    {
        public async Task<string> LeerArchivoDeExcelAsync()
        {
            try
            {
                var excelFile = new LinqToExcel.ExcelQueryFactory(@"C:\Users\juand\OneDrive\Escritorio\Prueba.xlsx");

                var result =
                    from row in excelFile.Worksheet("Ausentismo")
                    let item = new
                    {
                        Empleado = row["Empleado"].Cast<string>(),
                        Fecha = row["Fecha"].Cast<DateTime>(),
                        Contrato = row["Contrato"].Cast<string>(),
                    }

                    select item;
               

                var nuevoFila = result.ToList();


                return string.Empty;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}
