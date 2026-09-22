namespace Backend.Formas.Entities.Model
{
    public class Form
    {
        public decimal Formid { get; set; }
        public string Name { get; set; }
        public decimal? Formtypeid { get; set; }
        public string Ismaincamp { get; set; }

        public virtual Formtype Formtype { get; set; }
    }
}