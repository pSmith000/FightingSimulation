using System;
using System.Collections.Generic;
using System.Text;

namespace FightingSimulation
{
    public struct Monster
    {
        public string name;
        public float health;
        public float attack;
        public float defense;
    }
    class Game
    {

        bool gameOver = false;
        Monster currentMonster1;
        Monster currentMonster2;
        int currentMonsterIndex = 0;
        int currentScene = 0;

        //Monsters
        Monster wompus;
        Monster thwompus;
        Monster backupWompus;
        Monster unclePhil;

        Monster[] monsters;

        public void Run()
        {
            Start();

            while (!gameOver)
            {
                Update();
            }

            End();
        }

        void Start()
        {
            //Initialize Monsters
            wompus.name = "Wompus";
            wompus.attack = 15.0f;
            wompus.defense = 5.0f;
            wompus.health = 20.0f;

            thwompus.name = "Thwompus";
            thwompus.attack = 15.0f;
            thwompus.defense = 10.0f;
            thwompus.health = 15.0f;

            backupWompus.name = "Backup Wompus";
            backupWompus.attack = 25.6f;
            backupWompus.defense = 5.0f;
            backupWompus.health = 3.0f;

            unclePhil.name = "Uncle Phil";
            unclePhil.attack = 1000000000f;
            unclePhil.defense = 0.0f;
            unclePhil.health = 1.0f;

            monsters = new Monster[] { wompus, thwompus, backupWompus, unclePhil };

            ResetCurrentMonsters();

        }

        /// <summary>
        /// Called every game loop
        /// </summary>
        void Update()
        {
            UpdateCurrentScene();
            Console.Clear();
        }

        void End()
        {
            Console.WriteLine("Guhbah fren");
        }

        /// <summary>
        /// Resets the current fighters to be the first two monsters in the array.
        /// </summary>
        void ResetCurrentMonsters()
        {
            currentMonsterIndex = 0;
            //Set starting fighters
            currentMonster1 = monsters[currentMonsterIndex];
            currentMonsterIndex++;
            currentMonster2 = monsters[currentMonsterIndex];
        }

        void UpdateCurrentScene()
        {
            switch (currentScene)
            {
                case 0:
                    DisplayStartMenu();
                    break;

                case 1:
                    Battle();
                    UpdateCurrentMonsters();
                    Console.ReadKey();
                    break;

                case 2:
                    DisplayRestartMenu();
                    break;

                default:
                    Console.WriteLine("Invalid scene index");
                    break;
            }
        }

        /// <summary>
        /// Gets input from the player based on some decision  
        /// </summary>
        /// <param name="description">The context for the decision</param>
        /// <param name="option1">The first choice</param>
        /// <param name="option2">The second choice</param>
        /// <param name="pauseInvalid">If true, the player must press a key to continue after inputting
        /// an incorrect value</param>
        /// <returns>A number representing which of the two option was chosen. Returns 0 if an invalid input
        /// was recieved</returns>
        int GetInput(string description, string option1, string option2, bool pauseInvalid = false)
        {
            //Print the context and options
            Console.WriteLine(description);
            Console.WriteLine("1. " + option1);
            Console.WriteLine("2. " + option2);

            //Get player input
            string input = Console.ReadLine();
            int choice = 0;

            //If the player types 1...
            if (input == "1")
            {
                //...set the return variable to 1
                choice = 1;
            }
            //If the player types 2...
            else if (input == "2")
            {
                //...set the return variable to 2
                choice = 2;
            }
            //If the player did not type a 1 or 2...
            else
            {
                //...let them know the input was invalid
                Console.WriteLine("Invalid Input");

                //If we want to pause when an invalid input is recieved...
                if (pauseInvalid)
                {
                    //...make the player press a key to continue
                    Console.ReadKey(true);
                }
            }

            //Return the player choice
            return choice;
        }

        /// <summary>
        /// Display the starting menu. Gives the player the option to start or quit the game
        /// </summary>
        void DisplayStartMenu()
        {
            //Get player choice
            int choice = GetInput("Welcome to Monster Fight Simulator and Uncle Phil!", "Start Simulation", "Quit Application");

            //If they chose to start...
            if (choice == 1)
            {
                //...start at the batle scene
                currentScene = 1;
            }
            //Otherwise if they chose to exit...
            else if (choice == 2)
            {
                //end the game
                gameOver = true;
            }
        }

        /// <summary>
        /// Displays the restart menu. Gives the player the option to restrat or exit the program
        /// </summary>
        void DisplayRestartMenu()
        {
            //Get the player choice
            int choice = GetInput("Simulation Over. Would you like to play again?", "Yes", "No");

            //If the player chose to restart...
            if (choice == 1)
            {
                //...set the current scene to be at the starting scene
                ResetCurrentMonsters();
                currentScene = 0;
            }
            //If the player chose to quit...
            else if (choice == 2)
            {
                //...end the game
                gameOver = true;
            }
        }


        Monster GetMonster(int monsterIndex)
        {
            Monster monster;
            monster.name = "None";
            monster.attack = 1;
            monster.defense = 1;
            monster.health = 1;

            if (monsterIndex == 0)
            {
                monster = unclePhil;
            }
            else if (monsterIndex == 1)
            {
                monster = backupWompus;
            }
            else if (monsterIndex == 2)
            {
                monster = wompus;
            }
            else if (monsterIndex == 3)
            {
                monster = thwompus;
            }

            return monster;
        }
        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        void Battle()
        {
            //Print monster 1 stats
            PrintStats(currentMonster1);
            //Print monster 2 stats
            PrintStats(currentMonster2);

            //Monster 1 attacks monster 2
            float damageTaken = Fight(currentMonster1, ref currentMonster2);
            Console.WriteLine(currentMonster2.name + " has taken " + damageTaken);

            //Monster 2 attacks monster 1
            damageTaken = Fight(currentMonster2, ref currentMonster1);
            Console.WriteLine(currentMonster1.name + " has taken " + damageTaken);
        }

        bool TryEndSimulation()
        {
            bool simulationOver = currentMonsterIndex >= monsters.Length;

            if (simulationOver)
            {
                currentScene = 2;
            }

            return simulationOver;
        }

        /// <summary>
        ///Changes one of the current fighters to be the next in the list
        ///if it has died. Ends the game if all of the fighters in the list have been used.
        /// </summary>
        void UpdateCurrentMonsters()
        {
            //When the game runs out of monsters to cycle through the game is set to over
            if (currentMonsterIndex >= monsters.Length)
            {
                //Go to the restart menu
                currentScene = 2;
            }
            //If monster1 has died...
            if (currentMonster1.health <= 0)
            {
                currentMonsterIndex++;

                if (TryEndSimulation())
                {
                    return;
                }

                currentMonster1 = monsters[currentMonsterIndex];
            }
            //If monster 2 has died...
            if (currentMonster2.health <= 0)
            {
                currentMonsterIndex++;

                if (TryEndSimulation())
                {
                    return;
                }

                currentMonster2 = monsters[currentMonsterIndex];
            }
            
        }

        string StartBattle(ref Monster monster1, ref Monster monster2)
        {
            string matchResult = "No Contest";

            while (monster1.health > 0 && monster2.health > 0)
            {
                //Print monster 1 stats
                PrintStats(monster1);
                //Print monster 2 stats
                PrintStats(monster2);

                //Monster 1 attacks monster 2
                float damageTaken = Fight(monster1, ref monster2);
                Console.WriteLine(monster2.name + " has taken " + damageTaken);

                //Monster 2 attacks monster 1
                damageTaken = Fight(monster2, ref monster1);
                Console.WriteLine(monster1.name + " has taken " + damageTaken);

                Console.ReadKey(true);
                Console.Clear();
            }

            if (monster1.health <= 0 && monster2.health <= 0)
            {
                matchResult = "Draw";
            }

            else if (monster1.health > 0)
            {
                matchResult = monster1.name;
            }

            else if (monster2.health > 0)
            {
                matchResult = monster2.name;
            }

            return matchResult;
        }

        float Fight(Monster attacker, ref Monster defender)
        {
            float damageTaken = CalculateDamage(attacker, defender);
            defender.health -= damageTaken;
            return damageTaken;
        }

        void PrintStats(Monster monster)
        {
            Console.WriteLine("Name: " + monster.name);
            Console.WriteLine("Health: " + monster.health);
            Console.WriteLine("Attack: " + monster.attack);
            Console.WriteLine("Defense: " + monster.defense);
        }

        float CalculateDamage(float attack, float defense)
        {
            float damage = attack - defense;

            if (damage <= 0)
            {
                damage = 0;
            }

            return damage;
        }

        float CalculateDamage(Monster attacker, Monster defender)
        {
            return attacker.attack - defender.defense;
        }
        
    }
}
