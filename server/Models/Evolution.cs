using System.ComponentModel.DataAnnotations;

// To note down what each pokemon can evolve into and what item is needed for it to evolve
namespace Models
{
    public class Evolution
    {
        [Key]
        public string? Id {get;set;}
        public string? Name {get;set;}
        public string? EvolveInto {get;set;}
        public string? EvolveItem {get;set;}
        public string? ItemQuantity {get;set;}
        public int EvolveLevel {get;set;}

        private Evolution() { } //For EF Core
        public Evolution(string Name, string EvolveInto, string EvolveItem, string ItemQuantity, int EvolveLevel) {
            Id = Guid.NewGuid().ToString("N").Substring(0, 15); 
            this.Name = Name;
            this.EvolveInto = EvolveInto;
            this.EvolveItem = EvolveItem;
            this.ItemQuantity = ItemQuantity;
            this.EvolveLevel = EvolveLevel;
        }
    }
}