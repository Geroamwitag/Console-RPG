using System.Dynamic;

namespace RPGGame {
    public class Attack {

        // properties
        public string AttackName { get; set; }
        public string AttackType { get; set; }


        // constructor
        public Attack(string attackName, string attackType) {
            AttackName = attackName;
            AttackType =  attackType;
        }
    }
}