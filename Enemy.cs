namespace RPGGame
{
    public class Enemy : Living {
        public int ExpDrop;
        // Enemy constructor
        public Enemy(string name, int health, int atk, int def, int expDrop) : base(name, health, atk, def) {
            ExpDrop = expDrop;
        }

        // Enemy methods

    }
}