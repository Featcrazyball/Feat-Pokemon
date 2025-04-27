using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Item
    {
        [Key]
        public string? Id {get;set;}
        public string? Name {get;set;}
        public string? OwnderId {get;set;}
        public string? Type {get;set;}
        public string? Description {get;set;}

        private Item() { } //For EF Core
        public Item(string Name, string OwnderId, string Type, string Description) {
            Id = Guid.NewGuid().ToString("N").Substring(0, 15); 
            this.Name = Name;
            this.OwnderId = OwnderId;
            this.Type = Type;
            this.Description = Description;
        }

    }
}