using System.Collections.Generic;

namespace Backend.Formas.BusinessRules
{
    public static class ValidaFormaCell
    {
        public static List<string> ValidateCell(string type, dynamic Cell, string variable, int i, bool condicion = false)
        {
            var res = "";
            string error = "";

            try
            {
                if (type == "s")
                {
                    if (string.IsNullOrEmpty(Cell))
                    {
                        error = $"El valor del campo {variable} no debe ser vacio  en la linea {i}";
                    }
                    else
                    {
                        res = Cell;
                    }
                }

                if (type == "n")
                {
                    if (Cell == null)
                    {
                        error = $"El valor del campo {variable} no debe ser menor 0 ó mayor 100  en la linea {i}";
                        res = "0";
                    }

                    float value = float.Parse(Cell);
                    if (condicion)
                    {
                        if (value < 0 || value > 100)
                        {
                            error =
                                $"El valor del campo {variable} no debe ser menor 0 ó mayor 100  en la linea {i}";

                        }
                    }

                    if (Cell != null)
                    {
                        if (value == 0)
                        {
                            res = "0";
                        }
                        else
                        {
                            res = Cell;
                        }
                    }
                    else
                    {
                        res = "0";
                        error = $"El valor del campo {variable} no debe ser vacio en la linea {i}";

                    }
                }
            }
            catch
            {
                error = $"El valor del campo {variable} no debe ser vacio en la linea {i}";
            }

            List<string> respone = new List<string>()
            {
                res,
                error
            };
            return respone;
        }

    }
}