using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public interface IImportarExcel
    {
        void setFile(Stream _file);
        Stream getFile();
        void reader(string hj = "");
        bool isLoadMultiHojas();
        void setHoja(string hojaName);
        bool isLoad();
        int Rows(string referencia = "");
        Task<ExcelWorksheets> getWorksheets();
        string getCell(int row, int column);
        bool getCellExists(int row, int column, string value);
    }
}