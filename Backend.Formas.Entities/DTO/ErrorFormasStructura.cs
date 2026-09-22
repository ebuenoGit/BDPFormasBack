namespace Backend.Formas.Entities.DTO
{
    public class ErrorFormasStructura
    {

        public int? sheet { get; set; }
        public string column { get; set; }
        public int row { get; set; }
        public string message { get; set; }
        public string value { get; set; }

    }
}
