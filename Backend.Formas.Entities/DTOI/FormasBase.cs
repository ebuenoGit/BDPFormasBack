namespace Backend.Formas.Entities.DTOI
{
    public class FormasBase<T>
    {
        public FormaBase<T> FORMAS { set; get; }
        public Root root { set; get; }
    }

    public class FormaBase<T>
    {
        public FormaGenerica<T> FORMA { set; get; }
    }

    public class FormaGenerica<T>
    {
        public T REGISTRO { set; get; }
        public string VAL_PRODUCCION { set; get; }
        public string FORMA_CODIGO { set; get; }
    }


    //datos de respuestas
    public class RegistrosC4
    {
        public string MUNICIPIO { set; get; }
        public string PDEN_ID { set; get; }
        public string COD_MUNICIPIO { set; get; }
    }

    public class RegistrosF16
    {
        public string POZO { get; set; }
        public string METODO_PRODUCCION { get; set; }
        public string UWI { set; get; }
        public string PRODUCTION_METHOD { set; get; }
    }

    public class RegistrosF22
    {
        public string PDEN_ID { set; get; }
    }

}
