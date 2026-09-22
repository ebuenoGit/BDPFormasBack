
using System;

namespace Backend.Formas.Entities.DAO
{
    public class Acumulados
    {
        public int Id { get; set; }
        public Guid Id_Forma { get; set; }
        public float GasAcumulado { get; set; }
        public float CrudoAcumulado { get; set; }
        public float AguaAcumulado { get; set; }
    }
}
