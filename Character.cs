using RPGGame;

namespace RPGGame {
    // class inheritence
    public class Character : Living {

        // stat properties
        public int SkillPoints                      { get; set; }
        public int ExpPoints                        { get; set; }

        // inventory properties
        public Inventory CharacterInventory         { get; set; }
        public List<Attack> AttackSlots             { get; set; }
        public int MaxAttackSlots                   { get; set; }
        public Weapon EquipedWeapon                 { get; set; }
        private Weapon Barefists =                  new Weapon("Barefists", "pure masculin fists", "Common", 0);

        // special properties
        public int SaveSlot                         { get; set; }
        
        

        

        // Character constructor, inherited from Living
        public Character(string name, int health, int atk, int def, int skillpoints) : base(name, health, atk, def) {
            CharacterInventory = new Inventory();
            EquipedWeapon = Barefists;
            SkillPoints = skillpoints;
            ExpPoints = 0;
            SaveSlot = 0;
            MaxAttackSlots = 3;
            AttackSlots = new List<Attack> {};
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

        public void ShowEqupiedWeapon() {
            Console.WriteLine(EquipedWeapon.GetName());
        }

        public void AddAttackToSlot(Attack attack) {
            if (AttackSlots.Count < 3) {
                AttackSlots.Add(attack);
            }
            else {
                Console.WriteLine("Slots full");
            }
        }

        public void RemoveAttackFromSlot(Attack attack) {
            AttackSlots.Remove(attack);
            Console.WriteLine($"{attack} unslotted");
        }
            
        

    }
}