using System.Collections.Generic;

namespace Backend.Formas.Entities.DTOI
{
    public class RequesAprobacionExitosa
    {
        public string root { set; get; }
    }
    public class RequestAprobacion<T>
    {

        public RootResponse<T> root { set; get; }
    }

    public class RootResponse<T>
    {
        public string error { set; get; }
        public RecordResponse<T> detalle { set; get; }
    }

    public class RecordResponse<T>
    {
        public ElementResponse<T> record { set; get; }
    }
    public class ElementResponse<T>
    {
        public List<T> element { set; get; }
    }

    public class C4ModelRequest
    {
        public string PDEN_ID { set; get; }
        public string PDEN_TYPE { set; get; }
        public string PDEN_SOURCE { set; get; }
    }
}
