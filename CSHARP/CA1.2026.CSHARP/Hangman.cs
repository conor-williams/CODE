/////////////////////////////////////////////////////////
//
// Conor Williams - L00206159
//
// Hangman.cs -- main game file --
//    ---- This is the starting point ---
//    1. Check there is screen space
//    2. Create the List of movies 
//    3. Seed Random
//    4. Pick the movie randomly from the list
//    5. Instantiate the HangmanGame Class
//    6. Create the empty dictionary of stats
//    7. Request the player name
//    8. Instantiane the GameGUI
//    9. Play a game
//    10. (Once above game over) Instantiante the MenuGUI
//    11. Activate main menu
//
/////////////////////////////////////////////////////////
// using System.Console (saves Writing Console.WriteLine)
using static System.Console;
using System.Collections.Generic;

// widthWindowNeed - screen width we need
int widthWindowNeeded = 70;
// heightWindowNeeded - screen height we need
int heightWindowNeeded = 15;

// WindowWidth and WindowHeight are actual (read from the console)
if (WindowWidth < widthWindowNeeded || WindowHeight < heightWindowNeeded)
{
    WriteLine("Console window is too small.");
    WriteLine($"Please make the window larger (min: {widthWindowNeeded}x{heightWindowNeeded}) and run the program again.");
    WriteLine($"It is currently: (W:{WindowWidth}xH:{WindowHeight})");

    Environment.Exit(1);
}

// movies is out list of movies
var movies = new List<string> {"Rambo", "12 Angry Men", "The Matrix",
    "Saw", "Avatar", "Batman", "Spiderman",
    "Gran Turismo", "The Girl With The Dragon Tattoo", "Fast and Furious 3",
    "The Omen", "The Hulk", "Titanic", "The Godfather", "Citizen Kane",
    "The Shawshank Redemption", "Pulp Fiction", "Raging Bull", 
    "Lord of the Rings", "Forrest Gump", "Parasite", "Toy Story", 
    "Psycho", "Casablanca", "Gone with the Wind", "The Wizard of Oz",
    "Lawrence of Arabia", "Vertigo", "Schindler's List"};

//var movies = new List<string> {"Rambo", "The Matrix"};

// Start Random (only once)
Random randomMovie = new Random();
// Instantiate the hangman
HangmanGame hangmanGame = new HangmanGame();
// stats GUI
StatsGUI gStatsGUI = new StatsGUI(); // moved to a dictionary
// stats dictionary contains the stats
Dictionary <string, object> gameStats = new() {
    ["playerName"] = "not given", 
    ["currentMovieTitle"] = String.Empty,
    ["currentWinLose"] = -1,
    ["totalNumberGamesPlayed"] = 0,
    ["totalNumberGamesWon"] = 0,
    ["totalNumberGamesLost"] = 0
};
// Get the Player Name
hangmanGame.getPlayerName(gameStats);
// Instantiate the GameGUI
GameGUI gGameGUI = new GameGUI();

// Play the Hangman Game
hangmanGame.playHangMan(gGameGUI, movies, gameStats, randomMovie);
// Instantiate the MenuGUI
MenuGUI gMenuGUI = new MenuGUI();
// Start/Activate the main menu
hangmanGame.activateMainMenu(gGameGUI, gMenuGUI, gStatsGUI, movies, gameStats, randomMovie);

////////////////////////////////////////////////////////
// EOF
////////////////////////////////////////////////////////
