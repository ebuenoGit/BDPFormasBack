using Backend.Formas.Entities.DAO;
using System.Collections.Generic;

namespace Backend.Formas.Entities.DTOI
{
    public class ConsultaFormasServices<T>
    {
        public Root<T> root {set;get;}
    }

    public class ObjectServiceForma
    {
        public ConsultaFormasServices root { set; get; }
    }

    public class ObjectServiceFormaOperador
    {
        public ConsultaFormasServicesOperador root { set; get; }
    }

    public class ConsultaFormasServices
    {
        public string root { set; get; }
        public ConsultaFormasModel registro { set; get; }
    }

    public class ConsultaFormasServicesOperador
    {
        public string root { set; get; }
        public ConsultaFormaOperadorModel registro { set; get; }
    }

    public class Root<T> { 
        public string error { set; get; }
        public List<T> registro { set; get; }
    }

}

