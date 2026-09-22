namespace Backend.Formas.Entities.DTO.Dominios
{
    public class HeaderFormaC30MasivaDTO
    { /// <summary>
      /// MCG  --- > Mejoras BDP ++ 
      /// 2022 - Feb - 15
      ///  Elementos de encabezado de la forma 9 Carga por 
      ///  Pden_ID Pozo formacion del modelo PPDM
      /// </summary>
      /// 
        public string Mes { get; set; }                 // Mes de Carga 
        public string Ano { get; set; }                 // año de carga   
        public string FileName { set; get; }            // Nombre del archivo
        public string Url { set; get; }                 // Url de la ubicacion del archivo 
        public string FORMA_CODIGO { set; get; }        // Nombre de la Forma
        public string MIEMBRO_ID { set; get; }          // Usuario Quien esta cargando la forma
    }
}