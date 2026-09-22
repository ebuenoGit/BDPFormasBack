using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class RequestFormas<T>
    {
        public RequestFormas()
        {
            data = new List<T>();
            errors = new List<ErrorFormasStructura>();
        }

        public List<T> data { set; get; }
        public List<ErrorFormasStructura> errors { set; get; }
        public bool permisoDeCargue { set; get; }
    }
}
