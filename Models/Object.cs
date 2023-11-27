public class Object
{
    public required int Id { get; set; } // Is used as the primary key in a relational database in entity framework
    public required string Model { get; set; }
    public required string SerialNumber { get; set; }
    public required string Location { get; set; }
}
