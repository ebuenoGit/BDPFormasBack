using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Formas.Entities.Models
{
    public class DetalleFormaOperador
    {

       public string  pden_land_right { set; get; }
        public string volume_date { set; get; }
        public string inv_inicial { set; get; }
        public string inv_final { set; get; }
        public string entregas { set; get; }
        public string vol_muerto_vasijas { set; get; }
        public string vol_muerto_lineas { set; get; }
        public string perdidas_gravables { set; get; }
        public string perdidas { set; get; }
        public string consumos { set; get; }
        public string prod_gravable { set; get; }
        public string produccion { set; get; }
        public string api { set; get; }
        public string bsw { set; get; }
        public string sulphur { set; get; }
    }
}
