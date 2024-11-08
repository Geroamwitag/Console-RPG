using RPGGame;
namespace RPGGame{
    public class Weapon : Item
    {   
        // Weapon properties
        public string Rarity { get; set; }
        public int Atk { get; set; }

        // Weapon constructor
        public Weapon(string name, string description, string rarity, int atk) : base(name, description){
            Rarity = rarity;
            Atk = atk;
        }
    }
}