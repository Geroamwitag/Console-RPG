using System.Runtime.CompilerServices;

namespace RPGGame{
    public class Item {
        
        // item properties
        private string Name { get; set; }
        private string Description { get; set; }

        // item constructor
        public Item(string name, string description) {
            Name = name;
            Description = description;
        }

        public string GetName() {
            return Name;
        }

    }
}