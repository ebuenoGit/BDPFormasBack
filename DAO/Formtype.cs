using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.DAO
{
    public class Formtype
    {
        public Formtype()
        {
            Form = new HashSet<Form>();
        }

        public Guid Formtypeid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Form> Form { get; set; }
    }
}