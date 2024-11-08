using RPGGame;

namespace RPGGame {
    // class inheritence
    public class Character : Living {

        // Character specific properties
        public Inventory CharacterInventory { get; set; }
        public Weapon EquipedWeapon { get; set; }
        private Weapon Barefists = new Weapon("Barefists", "pure masculin fists", "Common", 0);


        // Character constructor, inherited from Living
        public Character(string name, int health, int atk, int def) : base(name, health, atk, def) {
            CharacterInventory = new Inventory();
            EquipedWeapon = Barefists;
        }

        // Character specific Methods
        public void EquipWeapon(Weapon weapon) {
            EquipedWeapon = weapon;
            this.Atk += weapon.Atk;
        }

        public void UnEquipWeapon(Weapon weapon) {
            EquipedWeapon = Barefists;
            this.Atk -= weapon.Atk;
        }

        public void ShowEqupiedWeapon(){
            Console.WriteLine(EquipedWeapon.GetName());
        }

 
            
        

    }
}