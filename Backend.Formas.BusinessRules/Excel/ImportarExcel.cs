using Backend.Formas.Utilities.Telemetry;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Backend.Formas.BusinessRules
{
    public class ImportarExcel : IImportarExcel
    {
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        private ExcelPackage _package;

        public Stream file;
        private ExcelWorksheet hoja;

        public ImportarExcel(ITelemetryException telemetryException)
        {
            TelemetryException = telemetryException;
        }

        public void setFile(Stream _file)
        {
            file = _file;
        }

        public Stream getFile()
        {
            return file;
        }

        public void reader(string hj)
        {
            try
            {
                _package = new ExcelPackage(getFile());
                if (!string.IsNullOrEmpty(hj))
                {
                    hoja = _package.Workbook.Worksheets.FirstOrDefault(h => hj.Equals(h.Name, System.StringComparison.CurrentCultureIgnoreCase));
                    if (hoja == null)
                    {
                        hoja = _package.Workbook.Worksheets.FirstOrDefault();
                    }
                }
            }
            catch (Exception e)
            {
                TelemetryException.RegisterException(e);
            }
        }

        public bool isLoadMultiHojas()
        {
            return _package.Workbook.Worksheets.Count > 0 ? true : false;
        }

        public void setHoja(string hojaName)
        {
            hoja = _package.Workbook.Worksheets.FirstOrDefault(x => hojaName.Equals(x.Name, System.StringComparison.CurrentCultureIgnoreCase));
        }

        public bool isLoad()
        {
            return hoja != null;
        }

        public int Rows(string referencia)
        {
            if (!string.IsNullOrEmpty(referencia))
            {
                hoja = _package.Workbook.Worksheets.FirstOrDefault(x => referencia.Equals(x.Name, System.StringComparison.CurrentCultureIgnoreCase));
            }

            return hoja?.Dimension?.Rows ?? 0;
        }

        public async Task<ExcelWorksheets> getWorksheets()
        {
            return await Task.Run(() => _package.Workbook.Worksheets);
        }

        public string getCell(int row, int column)
        {
            return hoja.Cells[row, column].Value?.ToString().Trim();
        }

        public bool getCellExists(int row, int column, string value)
        {
            return hoja.Cells[row, column].Value?.ToString().Trim() == value;
        }
    }
}