namespace RPGGame {
    public class Level {

        // properties
        public int ExpDrop          { get; set; }
        public Enemy RegularEnemy   { get; set; }
        public Enemy MiniBossEnemy  { get; set; }
        public Enemy BossEnemy      { get; set; }
        
        // constructor
        public Level(int expDrop, Enemy regularEnemy, Enemy miniBossEnemy, Enemy bossEnemy ) {
            ExpDrop = expDrop;
            RegularEnemy = regularEnemy;
            MiniBossEnemy = miniBossEnemy;
            BossEnemy = bossEnemy;
        }

        // methods
        public void StartBattle(Character character, Enemy enemy) {
            // scenario
            Console.WriteLine(
                $"""
                {character.Name} encounterd {enemy.Name}
                and the battle commenced.

                Press Enter to start battle
                """
            );
            Console.ReadKey();

            // battle loop
            bool battleActive = true;
            while (battleActive) {
                // player turn
                Console.Clear();
                Console.WriteLine($"{character.Name}'s turn");

                // add skill select
                DrawAttacks(character);
                Attack attack = SelectAttack(character);
                // if e was selected, exit the battle
                if (attack == null) {
                    ExitBattle();
                    break;
                }
                Console.WriteLine($"You used {attack.AttackName}!");
                Console.ReadKey();
                character.Attack(enemy);


                // check if won
                if (enemy.Health <= 0) {
                    Console.WriteLine("You win");
                    Console.ReadKey();
                    EndBattle(character, enemy);
                    break;
                }
               
                // enemy turn
                Console.Clear();
                Console.WriteLine($"{enemy.Name}'s turn");
                Console.ReadKey();
                Console.WriteLine($"{enemy.Name} Attacks!");
                Console.ReadKey();
                enemy.Attack(character);
                // check if loss
                if (character.Health <= 0) {
                    Console.WriteLine("You lose");
                    Console.ReadKey();
                    break;
                }
            }

        }
        

        public void DrawAttacks(Character character) {
            List<Attack> Attacks = character.AttackSlots;
            Console.WriteLine("Attack!!");
            Console.WriteLine("_-_-");
            for (int i = 0; i < Attacks.Count; i++) {
                Console.WriteLine($"| ({i+1}) {Attacks[i].AttackName} ({Attacks[i].AttackType})");
            }
            if (character.AttackSlots.Count == 0) {
                Console.WriteLine("You have no attacks equiped!");
            }
            Console.WriteLine($"| (e) Exit battle ");
            Console.WriteLine("_--__");
        }


        public Attack SelectAttack(Character character) {
            Console.Write("");
            string AttackOption = Console.ReadLine();

            // if no attacks equpied
            if (character.AttackSlots.Count == 0) {
                return null;
            }
            
            // if 1 attack equiped
            if (AttackOption == "1" && character.AttackSlots.Count >= 1) {
                return character.AttackSlots[0];
            }

            // if 2 attacks equiped
            if (AttackOption == "2" && character.AttackSlots.Count >= 2) {
                return character.AttackSlots[1];
            }   
            // if 3 attacks equiped
            if (AttackOption == "3" && character.AttackSlots.Count >= 3) {
                return character.AttackSlots[2];
            }
            // if e, endbattle
            if (AttackOption == "e") {
                return null;
            }


            // if invalid input
            Console.Clear();
            Console.WriteLine($"{character.Name}'s turn");
            DrawAttacks(character);
            Console.WriteLine("Invalid input");
            return SelectAttack(character);
        }


        public void AttackEffect(Attack attack) {

        }



        public void EndBattle(Character character, Enemy enemy) {
            Console.Clear();
            Console.WriteLine($"You got {enemy.ExpDrop} Exp");
            DropExp(character, enemy.ExpDrop);
            Console.ReadKey();
            Console.Clear();
        }


        public void ExitBattle() {
            Console.Clear();
            Console.WriteLine($"You escaped");
            Console.ReadKey();
            Console.Clear();
        }


        private void DropExp(Character character, int Exp) {
            character.ExpPoints += Exp;
        }

        

    }
}