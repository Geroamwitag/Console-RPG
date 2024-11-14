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

                Press Enter to Continue
                """
            );
            Console.ReadKey();

            // battle loop
            while (true) {
                // player turn
                Console.Clear();
                Console.WriteLine($"{character.Name}'s turn");

                // add skill select
                Console.WriteLine("Press any key to attack");
                Console.ReadKey();
                character.Attack(enemy);


                // check if won
                if (enemy.Health <= 0) {
                    Console.WriteLine("You win");
                    Console.ReadKey();
                    EndBattle(character);
                    break;
                }
               
                // enemy turn
                Console.Clear();
                Console.WriteLine($"{enemy.Name}'s turn");
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
        

        public void EndBattle(Character character) {
            Console.Clear();
            DropExp(character);
            Console.WriteLine($"You got {ExpDrop} Exp");
            Console.ReadKey();
            Console.Clear();
        }


        private void DropExp(Character character) {
            character.ExpPoints += ExpDrop;
        }
    }
}