using System.ComponentModel.DataAnnotations;
using Database;

namespace Models
{
    public class Item
    {
        [Key]
        public string? Id {get;set;}
        public string? Name {get;set;}
        public string? OwnerId {get;set;}
        public string? Type {get;set;}
        public string? Description {get;set;}

        private Item() { } //For EF Core
        public Item(string Name, string OwnerId, string Type, string Description) {
            Id = Guid.NewGuid().ToString("N").Substring(0, 15); 
            this.Name = Name;
            this.OwnerId = OwnerId;
            this.Type = Type;
            this.Description = Description;
        }

        public static void AddFireStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                for (int i = 0; i < count; i++)
                {
                    var item = new Item("Fire Stone", ownerId, "Evolution", "A stone that can be used to evolve certain Fire-type Pokémon.");
                    context.Items.Add(item);
                }
                context.SaveChanges();
            }
        }

        public static void RemoveFireStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                var items = context.Items.Where(i => i.Name == "Fire Stone" && i.OwnerId == ownerId).Take(count).ToList();
                foreach (var item in items)
                {
                    context.Items.Remove(item);
                }
                context.SaveChanges();
            }
        }

        public static void AddWaterStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                for (int i = 0; i < count; i++)
                {
                    var item = new Item("Water Stone", ownerId, "Evolution", "A stone that can be used to evolve certain Water-type Pokémon.");
                    context.Items.Add(item);
                }
                context.SaveChanges();
            }
        }

        public static void RemoveWaterStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                var items = context.Items.Where(i => i.Name == "Water Stone" && i.OwnerId == ownerId).Take(count).ToList();
                foreach (var item in items)
                {
                    context.Items.Remove(item);
                }
                context.SaveChanges();
            }
        }

        public static void AddThunderStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                for (int i = 0; i < count; i++)
                {
                    var item = new Item("Thunder Stone", ownerId, "Evolution", "A stone that can be used to evolve certain Electric-type Pokémon.");
                    context.Items.Add(item);
                }
                context.SaveChanges();
            }
        }

        public static void RemoveThunderStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                var items = context.Items.Where(i => i.Name == "Thunder Stone" && i.OwnerId == ownerId).Take(count).ToList();
                foreach (var item in items)
                {
                    context.Items.Remove(item);
                }
                context.SaveChanges();
            }
        }

        public static void AddLeafStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                for (int i = 0; i < count; i++)
                {
                    var item = new Item("Leaf Stone", ownerId, "Evolution", "A stone that can be used to evolve certain Grass-type Pokémon.");
                    context.Items.Add(item);
                }
                context.SaveChanges();
            }
        }

        public static void RemoveLeafStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                var items = context.Items.Where(i => i.Name == "Leaf Stone" && i.OwnerId == ownerId).Take(count).ToList();
                foreach (var item in items)
                {
                    context.Items.Remove(item);
                }
                context.SaveChanges();
            }
        }

        public static void AddMoonStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                for (int i = 0; i < count; i++)
                {
                    var item = new Item("Moon Stone", ownerId, "Evolution", "A stone that can be used to evolve certain Pokémon.");
                    context.Items.Add(item);
                }
                context.SaveChanges();
            }
        }

        public static void RemoveMoonStone(string ownerId, int count) {
            using (var context = new DatabaseContext())
            {
                var items = context.Items.Where(i => i.Name == "Moon Stone" && i.OwnerId == ownerId).Take(count).ToList();
                foreach (var item in items)
                {
                    context.Items.Remove(item);
                }
                context.SaveChanges();
            }
        }

    }
}