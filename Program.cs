using RPGGame;
using System;
using System.Diagnostics.Tracing;
using System.IO;

class Program
{
    static void Main(string[] args) {

        // start game
        RunGame();        
    }



    // Main functions
    static void RunGame() {
        // run the game
        int Option = ShowMainMenu();
        Run(Option);
    }


    static void Run(int choice) {
        switch (choice)
        {   
            // start new game
            case 1:
                Character player = CreateCharacter();
                Console.Clear();
                
                // go onto start game menu to start the game
                while (true) {
                    int StartGameOption = StartGameMenu();

                    View_Inventory_or_Select_Level(StartGameOption, player);

                    // if level select was selected
                    if (StartGameOption == 1) {
                        break;
                    }
                };
                break;

            // load saved game 
            case 2:
                int saveSlot = ShowLoadData();
                Character savedPlayer = LoadCharacterData($"./saves/saveslot_{saveSlot}.txt");
                Console.Clear();
                
                // go onto main menu to start the game
                while (true) {
                    int StartGameOptionTWO = StartGameMenu();

                    View_Inventory_or_Select_Level(StartGameOptionTWO, savedPlayer);

                    // if level select was selected
                    if (StartGameOptionTWO == 3) {
                        break;
                    }
                };

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



    // Generic functions
    static void View_Inventory_or_Select_Level(int option, Character character) {
        // conditions
        bool levelOption = (option == 1);
        bool InventoryOption = (option == 2);

        if (levelOption) {
            int SelectedLevel = LevelSelect();
            // start level here
            RunBanditLevel(character);
            // auto save char data
            SaveCharacterData(character, character.SaveSlot);
        }
        
        if (InventoryOption) {
            Console.Clear();
            character.CharacterInventory.Show();
            Console.ReadKey();
        }
    }


    static int SelectSaveSlot() {
        // get savefiles
        string saveDirectory = "saves";
        string[] saveFiles = Directory.GetFiles(saveDirectory, "*.txt");

        // selected save file will be the length of the available save slots
        int saveSlot = saveFiles.Count();
        return saveSlot;
    }

    static void SaveCharacterData(Character character, int saveslot) {
        string save_path = $"./saves/saveslot_{saveslot}.txt";

        // asign save slot to character
        character.SaveSlot = saveslot;

        // write data to text file
        using (StreamWriter writer = new StreamWriter(save_path, append: false))
        {
            writer.WriteLine(character.Name);
            writer.WriteLine(character.EquipedWeapon.GetName());
            writer.WriteLine(character.Atk - character.EquipedWeapon.Atk);
            writer.WriteLine(character.Def);
            writer.WriteLine(character.SkillPoints);
            writer.WriteLine(character.ExpPoints);
            writer.WriteLine(character.SaveSlot);
        }

        // success message
        Console.WriteLine("Data Saved");
        Console.ReadKey();
        Console.Clear();
    }


    static void RequestSave(Character character) {
        Console.Clear();
        Console.Write("Save char? (y/n) ");
        string? option = Console.ReadLine();
        switch (option) {
            case "y":
                SaveCharacterData(character, character.SaveSlot);
                break;
            case "n":
                Console.WriteLine("Data not saved");
                Console.ReadKey();
                break;
            default:
                Console.Clear();
                Console.WriteLine("input a valid option.");
                Console.ReadKey();
                RequestSave(character);
                break;
        }
    }

    static void CharacterCreator_SaveRequest(Character character) {
        Console.Clear();
        Console.Write("Save char? (y/n) ");
        string? option = Console.ReadLine();
        switch (option) {
            case "y":
                int saveSlot = SelectSaveSlot();
                SaveCharacterData(character, saveSlot);
                break;
            case "n":
                Console.WriteLine("Data not saved");
                Console.ReadKey();
            break;
            default:
                Console.Clear();
                Console.WriteLine("input a valid option.");
                Console.ReadKey();
                CharacterCreator_SaveRequest(character);
        break;
        }
    }


    static Character LoadCharacterData(string SaveFilePath) {    
        string[] characterData = File.ReadAllLines(SaveFilePath);
        string charName = characterData[0];
        string weaponName = characterData[1];
        int charAtk = int.Parse(characterData[2]);
        int charDef = int.Parse(characterData[3]);
        int charSP = int.Parse(characterData[4]);
        int charExp = int.Parse(characterData[5]);
        int charSaveSlot = int.Parse(characterData[6]);

        // rebuild character
        Character character = new Character(charName, 100, charAtk, charDef, charSP);

        // add exp
        character.ExpPoints += charExp;

        // re-equip equiped weapon
        List<Weapon> Weapons = GetWeapons();
        foreach (var Weapon in Weapons) {
            if (Weapon.GetName() == weaponName) {
                int weaponIndx = Weapons.IndexOf(Weapon);
                character.EquipWeapon(Weapons[weaponIndx]);
                character.CharacterInventory.AddItem(Weapons[weaponIndx]);
            }
        }

        return character;
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



    // UI functions
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


    static int StartGameMenu() {
        while (true) {
            // clear UI
            Console.Clear();

            // Ui's
            string GameMenuUI =
            """ 
            ===
            Game Menu
            1- Level Select
            2- Inventory
            3- Return to main menu
            ===
            """;
            

            // display main menu
            Console.WriteLine(GameMenuUI);
            Console.Write("select an option: ");
            string? GameMenuOption = Console.ReadLine();
            bool validMainOption = CheckIntInput(GameMenuOption, 3, 1);

            if (validMainOption) {
                int intOption = int.Parse(GameMenuOption);

                switch (intOption) {
                    case 1:
                        // return level select option
                        return 1;
                        
                    case 2:
                        // Inventory
                        return 2;

                    case 3:
                        // return to main menu
                        RunGame();
                        break;

                    default:
                        // error message
                        Console.Clear();
                        Console.WriteLine("Something went wrong");
                        return StartGameMenu();
                        
                    
                }
            }
        }
        

    }


    static int ShowLoadData() {
        // clear ui
        Console.Clear();

        // get savefiles
        string saveDirectory = "saves";
        string[] saveFiles = Directory.GetFiles(saveDirectory, "*.txt");

        // if no saves 
        if (saveFiles.Length == 0) {
            Console.WriteLine("No save data found");
            Console.ReadKey();
            Console.Clear();
            // rerun game
            RunGame();
            return 0;
        }

        // displays all availabe save files
        for (int i = 0; i < saveFiles.Length; i++) {
            string filePath = saveFiles[i];
            string playerName = File.ReadLines(filePath).First(); // Reads the first line of the file

            Console.WriteLine($"({i}) {playerName}");
        }

        // has the player select a file number
        Console.Write($"Select a save to load: ");
        string? strinput = Console.ReadLine();
        bool intable = CheckIntInput(strinput, saveFiles.Length-1, 0);
        if (intable) {
            int option = int.Parse(strinput);
            return option;
        }
        else {
            return ShowLoadData();
        }
    }


    static int LevelSelect() {
        Console.Clear();
        string LevelSelectUi = 
            """
            ==
            Level Select
            1 - Level 1, Bandit Camp
            ==
            """;
        
        // display UI an request option
        while (true) {
            Console.Clear();
            Console.WriteLine(LevelSelectUi);
            Console.Write("select a level: ");
            string? levelOption = Console.ReadLine();
            bool validLevelOption = CheckIntInput(levelOption, 2, 1);

            if (validLevelOption) {
                int selectedLevel = int.Parse(levelOption);
                Console.Clear();
                return selectedLevel;
            }
            else {
                Console.WriteLine("Please select a valid option");
                Console.ReadKey();
            }
        }
        
    }


    static void ShowInventory(Character character) {
        Console.Clear();
        Inventory charInventory = character.CharacterInventory;
        charInventory.Show();
        Console.Clear();
    }


    // Character creator functions
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

        // equip starting equipment
        player.EquipWeapon(startingWeapon);
        player.CharacterInventory.AddItem(startingWeapon);

        // save character?
        CharacterCreator_SaveRequest(player);
        return player;
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



    // Generation functions
    static void RunBanditLevel(Character player) {
        List<Enemy> BanditEnemies = GetBanditEnemies();
        Level BanditLevel = new Level(10, BanditEnemies[0], BanditEnemies[1], BanditEnemies[2]);
        BanditLevel.StartBattle(player, BanditEnemies[0]);
    }


    static List<Enemy> GetBanditEnemies() {
        /*
        0 -> reg enemy
        1 -> mini boss enemy
        2 -> boss enemy
        */

        // define enemy objects
        Enemy bandit = new Enemy("Bandit",20, 2, 2);
        Enemy banditCaptian = new Enemy("Bandit Captian",50,10,5);
        Enemy banditLeader = new Enemy("Bandit Leader",70,12,7);

        // add weapon objects to weapon list
        List<Enemy> banditEnemyList = new List<Enemy> {
            bandit,
            banditCaptian,
            banditLeader
            };

        return banditEnemyList;
        
    }


    static List<Weapon> GetWeapons() {
        /*
        0 -> weak weapon
        1 -> average weapon
        2 -> strong weapon
        */

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



}


// generic classes
class Stats {
    public int Atk { get; set; }
    public int Def { get; set; }
    public int SkillPoints {get; set; }
}
