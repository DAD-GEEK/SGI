namespace Models.DTO
{
    public class DatatableParamsDTO
    {
        public string draw { get; set; }
        public string end { get; set; }
        public string length { get; set; }
        public int page { get; set; }
        public string pages { get; set; }
        public string recordsDisplay { get; set; }
        public int recordsTotal { get; set; }
        public bool serverSide { get; set; }
        public string start { get; set; }
        public string searchValue { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }
    }
}
