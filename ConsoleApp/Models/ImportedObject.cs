namespace ConsoleApp.Models
{
    class ImportedObject : ImportedObjectBaseClass
    {
        
        public string Schema { get; set; } = default;

        public string ParentName { get; set; } = default;
        public string ParentType { get; set; } = default;

        public string DataType { get; set; } = default;
        public bool IsNullable { get; set; } = default;

        public double NumberOfChildren { get; set; } = default;
    }
}
