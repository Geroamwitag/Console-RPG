namespace RPGGame {
    public class Living {

        // Living properties
        public string Name { get; set; }
        public int Health { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }

        // Living constructor
        public Living(string name, int health, int atk, int def) {
            Name = name;
            Health = health;
            Atk = atk;
            Def = def;
        }

        // Living Methods
        public void Say(String speach) {
            Console.WriteLine($"{Name}: {speach}");
        }

        public void DetailHealth() {
            Console.WriteLine($"{this.Name} has {Health} health remaining");
        }

        public void Attack(Living enemy) {
            int damage = DamageCalculation(this, enemy);
            enemy.Health -= damage;
            if (enemy.Health <= 0) {
                enemy.Health = 0;
            }
            Console.WriteLine($"Damage dealt {damage} , {enemy.Name} has {enemy.Health} health remaining");
            Console.ReadKey();
        }



        // calculates damage delt upon an attack
        private int DamageCalculation(Living entityAttacking, Living entityAttacked) {

            int Damage = entityAttacking.Atk - entityAttacked.Def;
            if (Damage < 0) {
                Damage = 0;
            }
            
            return Damage;
        }


    }
}