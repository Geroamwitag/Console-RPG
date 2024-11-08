using RPGGame;
using System;
using System.IO;

class Program
{
    static void Main(string[] args) {

        // start game
        int Option = ShowMainMenu();


        Run(Option);

        // testing
        // Console.Clear();
        // ShowLoadData();
        // List<Weapon> all_weapons = GetWeapons(); 
        // Character testChar = LoadCharacterData("./saves/saveslot_0.txt");
        // testChar.EquipWeapon(all_weapons[0]);
        
        // SaveCharacterData(testChar, 0);
        
    }
    
    // generic functions
    static int ShowMainMenu()
    {
        while (true) {

            // gui print
            Console.Clear();
            Console.WriteLine("=== Very Cool RPG Game ===");
            Console.WriteLine("1. New Game");
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
                Console.Clear();
                DisplayCharacterStats(player);
                break;

            // load saved game 
            case 2:
                int saveSlot = ShowLoadData();
                Character character = LoadCharacterData($"./saves/saveslot_{saveSlot}.txt");
                Console.Clear();
                DisplayCharacterStats(character);
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

    static int ShowLoadData() {
        // clear ui
        Console.Clear();

        // get savefiles
        string saveDirectory = "saves"; // Change this to your actual save directory path
        string[] saveFiles = Directory.GetFiles(saveDirectory, "*.txt");

        // displays all availabe save files
        for (int i = 0; i < saveFiles.Length; i++) {
            string filePath = saveFiles[i];
            string playerName = File.ReadLines(filePath).First(); // Reads the first line of the file

            Console.WriteLine($"({i}) {playerName}");
        }

        // has the player select a file number
        Console.Write($"Select a save to load: ");
        string strinput = Console.ReadLine();
        bool intable = CheckIntInput(strinput, saveFiles.Length-1, 0);
        if (intable) {
            int option = int.Parse(strinput);
            return option;
        }
        else {
            return ShowLoadData();
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
        int SP = CharStats.SkillPoints;

        // create player
        Character player = new Character(CharName, baseHealth, Atk, Def, SP);

        // starting equipment
        List<Weapon> WeaponList = GetWeapons();
        Weapon startingWeapon = StartingWeapon(WeaponList);

        // equpi starting equipment
        player.EquipWeapon(startingWeapon);

        return player;
    }

    static void DisplayCharacterStats(Character character) {
        Console.WriteLine(
                    $"""
                    --
                    | Character stats:                                    
                    --
                    | player name:    {character.Name}                    
                    | equiped weapon: {character.EquipedWeapon.GetName()} 
                    | Atk:            {character.Atk}                     
                    | Def:            {character.Def}                     
                    | SP:             {character.SkillPoints}
                    --             
                    """
                );
    }

    static Stats AllocateStats(int availableStatPoints) {
        //reset gui
        Console.Clear();

        // initial availableStats
        int initialAvailableStats = availableStatPoints;

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
                int attackToAllocate = Convert.ToInt32(stringAtk);
                intAtkAllocation += attackToAllocate;

                // less than 10 more than 0 allocation
                if (intAtkAllocation > 10 || intAtkAllocation < 0 || intAtkAllocation > initialAvailableStats) {
                    Console.WriteLine("points must be between 0 and 10, and available");
                    Console.ReadKey(); // Pause to show the error
                    continue;
                }
                
                // remove allocated stats from available stats
                availableStatPoints -= attackToAllocate;
                
            // reloop if input is not a number    
            }
            catch (FormatException) {
                Console.WriteLine("input must be a number");
                Console.ReadKey(); // Pause to show the error
                continue;
            }

            Console.Clear();
            Console.Write($"Finish Atk allocation {intAtkAllocation} Atk points? (yes/no) ");
            string? finish = Console.ReadLine();
            if (finish == "yes" || finish == "YES" || finish == "Yes") {
                Console.Clear();
                break;
            }
            else {
                if (intAtkAllocation == 10) {
                return AllocateStats(initialAvailableStats);
                }
                continue;
            }
        }

        // clear console when switching to def allocation
        Console.Clear();

        // allocate def points
        while(availableStatPoints > 0) {
            // show remaining points
            Console.WriteLine($"Statpoints left: {availableStatPoints}");
            
            // def allocation
            Console.Write("Def points: ");
            string? stringDef = Console.ReadLine();

            // check input
            try {
                int defToAllocate = Convert.ToInt32(stringDef);
                intDefAllocation += defToAllocate;

                // less than 10 more than 0 allocation
                if (intDefAllocation > 10 || intDefAllocation < 0 || intDefAllocation > availableStatPoints) {
                    Console.WriteLine("Points must be between 0 and 10, and available");
                    Console.ReadKey(); // Pause to show the error
                    continue;
                }
                
                // remove def points
                availableStatPoints -= defToAllocate;
                
            }
            catch (FormatException) {
                Console.WriteLine("input must be a number");
                Console.ReadKey(); // Pause to show the error
                continue;

            
            }

            Console.Write($"Finish Def allocation {intDefAllocation} Def points? (yes/no) ");
            string? finish = Console.ReadLine();
            if (finish == "yes" || finish == "YES" || finish == "Yes") {
                break;
            }
            else {
                if (intDefAllocation == 10) {
                return AllocateStats(initialAvailableStats);
                }
                continue;
            }
    }

    Console.Clear();
    Console.WriteLine($"Stats allocated: Atk = {intAtkAllocation} Def = {intDefAllocation} SP left = {availableStatPoints}\npress Enter to continue");
    Console.ReadKey(); // Pause to show the message
    Console.Clear();
    return new Stats { Atk = intAtkAllocation, Def = intDefAllocation, SkillPoints = availableStatPoints };
    
    }

    static bool CheckIntInput(string input, int max, int min) {
        try {
            int intinput = Convert.ToInt32(input);
            if (intinput >= min && intinput <= max) {
                return true;
            }
            return false;
        }
        catch (FormatException) {
            return false;
        }
    }
    
    // weapon methods
   static List<Weapon> GetWeapons() {
        // define weapon objects
        Weapon BareFists = new Weapon("Bare Fists", "Pure masculin fists", "common", 0);
        Weapon ShotSword = new Weapon("Short Sword", "A basic short sword", "common", 5);
        Weapon GreatSword = new Weapon("Great Sword", "A basic great sword", "common", 10);

        // add weapon objects to weapon list
        List<Weapon> weaponList = new List<Weapon> {
            BareFists,
            ShotSword,
            GreatSword
            };

        return weaponList;
        
   }

    static Weapon StartingWeapon(List<Weapon> weaponsList) {
    Console.Clear();

    Console.WriteLine(
        $"""
        === Starting equpiment ===
        1- Barefists  :  Pure masculin fists, Atk = 0
        2- Short Sword:  A basic short sword, Atk = 5
        3- Great Sword:  A basic great sword, Atk = 10
        """
    );
    
    // select a weapon
    Console.Write("Select a starting weapon: ");
    string? option = Console.ReadLine();
    bool intbool = CheckIntInput(option, 3, 1);

    if (intbool) {
        int intOption = Convert.ToInt32(option);
        int weaponIDNX = intOption - 1;
        Weapon startingWeapon = weaponsList[weaponIDNX];
        Console.Clear();
        Console.WriteLine($"Starting weapon selected: {startingWeapon.GetName()}\npress Enter to continue");
        Console.ReadKey();
        Console.Clear();
        return startingWeapon;
    }
    Console.Clear();
    return StartingWeapon(weaponsList);

    
}

    static void SaveCharacterData(Character character, int saveslot) {
        string save_path = $"./saves/saveslot_{saveslot}.txt";

        // write data to text file
        using (StreamWriter writer = new StreamWriter(save_path, append: false))
        {
            writer.WriteLine(character.Name);
            writer.WriteLine(character.EquipedWeapon.GetName());
            writer.WriteLine(character.Atk - character.EquipedWeapon.Atk);
            writer.WriteLine(character.Def);
            writer.WriteLine(character.SkillPoints);
        }

        // success message
        Console.WriteLine("Save Complete.");
    }

    static Character LoadCharacterData(string SaveFilePath) {    
        
        string[] characterData = File.ReadAllLines(SaveFilePath);
        string charName = characterData[0];
        string weaponName = characterData[1];
        int charAtk = int.Parse(characterData[2]);
        int charDef = int.Parse(characterData[3]);
        int charSP = int.Parse(characterData[4]);

        // rebuild character
        Character character = new Character(charName, 100, charAtk, charDef, charSP);

        // re-equip equiped weapon
        List<Weapon> Weapons = GetWeapons();
        foreach (var Weapon in Weapons) {
            if (Weapon.GetName() == weaponName) {
                int weaponIndx = Weapons.IndexOf(Weapon);
                character.EquipWeapon(Weapons[weaponIndx]);
            }
        }

        return character;
    }

    static void StartGameMenu() {
        // clear UI
        Console.Clear();

        //
        string MainMenuUI =
        """
        1- Level Select
        2- Inventory
        """;
    }
}



// generic classes
class Stats {
    public int Atk { get; set; }
    public int Def { get; set; }
    public int SkillPoints {get; set; }
}
