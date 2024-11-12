using RPGGame;
using System;
using System.Diagnostics.Tracing;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main(string[] args) {

        // start game
        RunGame();        
    }



    // Main functions
    static void RunGame() {

        // start at main menu screen
        DrawMainMenuGUI();
        MainMenuOptionSelect();
        
    }


    public static void MainMenuOptionSelect() {
        Console.Write("Select an option: ");
        string? MainMenuOption = Console.ReadLine();

        switch (MainMenuOption) {
            case "1":
                Console.Clear();
                CreateCharacter();
                break;

            // main starting point
            case "2":

                // load character data
                Console.Clear();
                int LoadDataOption = LoadData();
                Character player = LoadCharacterData($"./saves/saveslot_{LoadDataOption}.txt");
                Console.Clear();
                
                // game loop
                DrawGameMenuUI();
                GameMenuOptionSelect(player);
                break;


            case "3":
                Console.Clear();
                Environment.Exit(0);
                break;
            default:

                // redraw ui with error message
                Console.Clear();
                DrawMainMenuGUI();
                Console.WriteLine("Please Select a valid option");
                MainMenuOptionSelect();
                break;
        }
    }


    // Generic functions
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
                RequestSave(character);
                break;
        }
    }


    static void CharacterCreator_SaveRequest(Character character) {
        
        // character save for new character
        Console.Clear();
        Console.Write("Save char? (y/n) ");
        string? option = Console.ReadLine();
        switch (option) {
            case "y":
                int saveSlot = SelectSaveSlot();
                SaveCharacterData(character, saveSlot);
                Console.Clear();
                Console.WriteLine("Go to load game to start");
                Console.ReadKey();
                RunGame();
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


    public static void DisplayCharacterStats(Character character) {
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
    public static void DrawMainMenuGUI() {
        string MainMenuUI =
        $"""
        === Main Menu ===
        1- Create Character
        2- Load Game
        3- Exit
        """;
        Console.Clear();
        Console.WriteLine(MainMenuUI);
    }


    public static void DrawGameMenuUI() {
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
            Console.WriteLine(GameMenuUI);
    }
    
    public static void  GameMenuOptionSelect(Character character) {
            Console.Write("select an option: ");
            string? GameMenuOption = Console.ReadLine();

            switch (GameMenuOption) {
                case "1":
                    Console.Clear();
                    DrawLevelSelect();
                    LevelSelectOptionSelect(character);
                    break;
                    
                case "2":
                    // Inventory
                    Console.Clear();
                    ShowInventory(character);


                    // return to game menu
                    Console.Clear();
                    DrawGameMenuUI();
                    GameMenuOptionSelect(character);
                    break;

                case "3":
                    // return to main menu
                    RunGame();
                    break;

                default:
                    // error message
                    Console.Clear();
                    DrawGameMenuUI();
                    Console.WriteLine("Input a valid option");
                    GameMenuOptionSelect(character);
                    break;
                        
                }
            }

    public static void DrawLevelSelect() {
        string LevelSelectUI = 
        """
        === Level Select ===
        1- Bandit Camp
        """;
        Console.WriteLine(LevelSelectUI);
    }

    public static void LevelSelectOptionSelect(Character character) {
        Console.Write("Select a level: ");
        string? LevelSelectOption = Console.ReadLine();

        switch (LevelSelectOption) {

            // bandit camp
            case "1":
                Console.Clear();

                // replace with actual bandit camp
                RunBanditLevel(character);

                // save data
                Console.Clear();
                RequestSave(character);

                // loop back to game menu
                DrawGameMenuUI();
                GameMenuOptionSelect(character);

                break;

            // add more cases for more options

            default:
                Console.Clear();
                DrawLevelSelect();
                Console.WriteLine("Input a valid option");
                LevelSelectOptionSelect(character);
                break;
        }
    }
        

    public static int LoadData() {
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
            return LoadData();
        }
    }


    static void ShowInventory(Character character) {
        Console.Clear();
        Inventory charInventory = character.CharacterInventory;
        charInventory.Show();
        Console.ReadKey();
        Console.Clear();
    }


    // Character creator functions
    public static void CreateCharacter() {
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
    public static void RunBanditLevel(Character player) {
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
