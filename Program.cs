using RPGGame;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {

        // start game
        int Option = ShowMainMenu();

        Run(Option);

        
        
    }
    
    // generic functions
    static int ShowMainMenu()
    {
        while (true) {

            // gui print
            Console.Clear();
            Console.WriteLine("=== Very Cool RPG Game ===");
            Console.WriteLine("1. Start New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("3. Options");
            Console.WriteLine("4. Exit");
            Console.WriteLine("=====================");

            // user input
            Console.Write("Choose an option: ");
            string? userInnput = Console.ReadLine();

            // input check
            try {
                int intInput = Convert.ToInt32(userInnput);

                List<int> ValidOptions = new List<int> {1, 2, 3, 4};
                if (ValidOptions.Contains(intInput)) {
                    return intInput;
                }
                else {
                    Console.WriteLine("Please choose a valid option");
                    return ShowMainMenu();
                }
            }
            catch (FormatException) {
                return ShowMainMenu();
            };

        }
        
    }

    static void Run(int choice) {
        switch (choice)
        {   
            // start new game
            case 1:
                Character player = CreateCharacter();
                Console.WriteLine($"{player.Name}: I am {player.Name} I have {player.Atk} atk and {player.Def}");
                break;

            // load saved game 
            case 2:
                Console.WriteLine("You chose option 2");
                break;
            
            // open options menu
            case 3:
                Console.WriteLine("You chose option 3");
                break;

            // close game
            case 4:
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }

    static Character CreateCharacter() {
        Console.Clear();
        Console.WriteLine("=== Create Your Character ===");

        // name
        Console.Write("Character name: ");
        string? CharName = Console.ReadLine();
        
        // base health
        int baseHealth = 100;

        // stats
        int availableStatPoints = 10;
        Stats CharStats = AllocateStats(availableStatPoints);
        int Atk =  CharStats.Atk;
        int Def = CharStats.Def;
    

        Character player = new Character(CharName, baseHealth, Atk, Def);

        return player;
    }

    static Stats AllocateStats(int availableStatPoints) {
        //reset gui
        Console.Clear();

        // predefine stats
        int intAtkAllocation = 0;
        int intDefAllocation = 0;
        
        // allocate
        while(availableStatPoints > 0) {
            Console.Clear();
            Console.WriteLine($"Allocate stats: {availableStatPoints} left");

            // atk allocation
            Console.Write("Atk points: ");
            string? stringAtk = Console.ReadLine();

            // check input
            try {
                intAtkAllocation = Convert.ToInt32(stringAtk);

                // less than 10 more than 0 allocation
                if (intAtkAllocation > 10 || intAtkAllocation < 0 || intAtkAllocation > availableStatPoints) {
                    Console.WriteLine("points must be between 0 and 10, and available");
                    Console.ReadKey(); // Pause to show the error
                    continue;
                }
                
                // remove allocated stats from available stats
                availableStatPoints -= intAtkAllocation;
                
            // reloop if input is not a number    
            }
            catch (FormatException) {
                Console.WriteLine("input must be a number");
                Console.ReadKey(); // Pause to show the error
                continue;
            }


            Console.Write($"Finish Atk allocation {intAtkAllocation} Def points?");
            string? finish = Console.ReadLine();
            if (finish == "yes" || finish == "YES" || finish == "Yes") {
                break;
            }
        }

        // allocate def points
        while(availableStatPoints > 0) {
            // show remaining points
            Console.WriteLine($"Statpoints left: {availableStatPoints}");
            
            // def allocation
            Console.Write("Def points: ");
            string? stringDef = Console.ReadLine();

            // check input
            try {
                intDefAllocation = Convert.ToInt32(stringDef);

                // less than 10 more than 0 allocation
                if (intDefAllocation > 10 || intDefAllocation < 0 || intDefAllocation > availableStatPoints) {
                    Console.WriteLine("Points must be between 0 and 10, and available");
                    Console.ReadKey(); // Pause to show the error
                    continue;
                }
                
                // remove def points
                availableStatPoints -= intDefAllocation;
                
            }
            catch (FormatException) {
                Console.WriteLine("input must be a number");
                Console.ReadKey(); // Pause to show the error
                continue;

            
            }

            Console.Write($"Finish Def allocation {intDefAllocation} Def points? ");
            string? finish = Console.ReadLine();
            if (finish == "yes" || finish == "YES" || finish == "Yes") {
                break;
            }
    }
        
    Console.Clear();
    return new Stats { Atk = intAtkAllocation, Def = intDefAllocation};
    
    }
}

// Character stats class
class Stats {
    public int Atk { get; set; }
    public int Def { get; set; }
}
