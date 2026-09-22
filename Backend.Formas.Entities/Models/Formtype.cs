using System.Collections.Generic;

namespace Backend.Formas.Entities.Model
{
    public class Formtype
    {
        public Formtype()
        {
            Form = new HashSet<Form>();
        }

        public decimal Formtypeid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Form> Form { get; set; }
    }
}