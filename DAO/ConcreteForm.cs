using System;

namespace Backend.Formas.Entities.DAO
{
    public class Concreteform
    {
        public Guid Concreteformid { get; set; }
        public decimal? Maincampid { get; set; }
        public string Company { get; set; }
        public string Contract { get; set; }
        public string Battery { get; set; }
        public string Tank { get; set; }
        public decimal? Month { get; set; }
        public decimal? Year { get; set; }
        public string Explotationmodality { get; set; }
        public string Annotations { get; set; }
        public decimal? Version { get; set; }
        public Guid Currentstate { get; set; }
        public decimal? Generationflag { get; set; }
        public decimal? Campid { get; set; }
        public string Pdenid { get; set; }
        public Guid Formid { get; set; }
        public decimal? Generationjobid { get; set; }
        public decimal? Iqistatus { get; set; }
        public string Usersigning { get; set; }
        public string Minrepsigning { get; set; }
        public string Formname { get; set; }
    }
}