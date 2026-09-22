using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form
    {
        public Guid Formid { get; set; }
        public string Name { get; set; }
        public Guid? Formtypeid { get; set; }
        public string Ismaincamp { get; set; }

        public virtual Formtype Formtype { get; set; }
    }
}