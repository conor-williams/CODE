//////////////////////////////////////////////////
//
// Conor Williams - L00206159
//
// HangmanGame.cs ---- 1 class with 4 important methods ----
//    o. The HangmanGame class has 4 methods
//        1. getPlayerName: request the users player name
//        2. pickAMovieAndRemoveFromList: picks a movie and removes from list
//        3. playHangman: plays the hangman game
//        4. activateMainMenu: activates the main menu
//    o. it has one constant: TOTAL_MISTAKES -- it holds the allowed mistakes
//     
//////////////////////////////////////////////////
using static System.Console;
using System.Text;
using System.Collections.Generic;
//using System.Threading;

public class HangmanGame {
    // TOTAL_MISTAKES: Total number of mistakes allowed
    public const int TOTAL_MISTAKES = 7;
    // get the player name
    public void getPlayerName(Dictionary <string, object> gameStats) {
        // Instantiate the GetnameGUI
        GetnameGUI gGetnameGUI = new GetnameGUI();
        gGetnameGUI.display();
        gameStats["playerName"] = (string)(ReadLine() ?? "");
    }

    // pick the movie from the list (and then remove it from the list)
    // return a tuple of movie and mask
    public (string, char []) pickAMovieAndRemoveFromList(List<string> movies, Dictionary<string, object> gameStats, Random randomMovie) {        
        int whichMoviePos;
        if (movies.Count == 0) {
            WriteLine("Error: no more movies");
            string tmp1 = "Error";
            var tmp2 = tmp1.ToCharArray();
            return (tmp1, tmp2);
        }
        //randomly pick the movie
        whichMoviePos = randomMovie.Next(0, movies.Count);
        //WriteLine($"The movie is: {movies[whichMoviePos]}");
        string whichMovie = movies[whichMoviePos];
        gameStats["currentMovieTitle"] = (string)whichMovie;
        var whichMovieMask = whichMovie.ToCharArray();
        //create the movie mask 
        for (int ii = 0, end = whichMovie.Length; ii < end; ii++) {
            if (whichMovie[ii] == '\'' || char.IsLetter(whichMovie[ii])
                 || char.IsDigit(whichMovie[ii])) {
                whichMovieMask[ii] = '_';
            }
        }
        movies.RemoveAt(whichMoviePos);
        return  (whichMovie, whichMovieMask);
    }

    // play the hangman game
    public void playHangMan(GameGUI gGameGUI, List<string> movies, Dictionary<string, object> gameStats, Random randomMovie) {
        // clear all the pressed keys
        while (Console.KeyAvailable) {Console.ReadKey(true);}
        (string whichMovie, char [] whichMovieMask) = pickAMovieAndRemoveFromList(movies, gameStats, randomMovie);
        var alreadyEntered = new List<char>();
        // print the stage
        gGameGUI.printStage();
        // display the mask
        string tmp3 = new string(whichMovieMask);
        gGameGUI.displayMovieMask(tmp3);
        bool notFoundMovie = false;
        int mistakes = 0;
        // display already
        gGameGUI.displayAlready("");
        // display the mistakes
        gGameGUI.displayMistakes($"mistakes: {mistakes}");
        do {
            notFoundMovie = false;
            // display the prompt
            gGameGUI.displayPrompt("Enter Letter (upper or lower) or Number or right tick: ");
            var letterRead = ReadKey();
            // get the letter
            char letter = letterRead.KeyChar;
            bool validDigit = char.IsDigit(letter);
            bool validLetter = char.IsLetter(letter);
            char lowerLetter = char.ToLower(letter);
            bool isRightTick = letter == '\''? true: false;
            
            bool usedAlready = alreadyEntered.Contains(lowerLetter);
            if ((validLetter || validDigit || isRightTick) && !usedAlready) {
                alreadyEntered.Add(lowerLetter);
                gGameGUI.displayError("                                                ");
                string tmp2 = string.Join("", alreadyEntered);
                // display what has already been tried
                gGameGUI.displayAlready(tmp2);
                    
                char letterUpper = char.ToUpper(letter);
                // check if movie contains the letter
                if (whichMovie.Contains(lowerLetter) || whichMovie.Contains(letterUpper)) {
                    for (int ii = 0, end = whichMovie.Length; ii < end; ii++) {
                        if (whichMovie[ii] == lowerLetter) {
                            whichMovieMask[ii] = lowerLetter;
                        }
                        if (whichMovie[ii] == letterUpper) {
                            whichMovieMask[ii] = letterUpper;
                        }
                    }
                    // display positive message
                    gGameGUI.displayOk($"{lowerLetter} is in movie title");
                } else {
                    // display new part of the man
                    gGameGUI.displayHangmanPart(++mistakes);

                    // display error
                    gGameGUI.displayError($"{lowerLetter} not in movie title");
                    // display mistakes
                    gGameGUI.displayMistakes($"mistakes: {mistakes}");
                    if (mistakes >= TOTAL_MISTAKES) {
                        // display lose
                        gGameGUI.displayLose("Lose: Game Over");
                        // set stats
                        gameStats["currentWinLose"] = (int)1;
                        gameStats["totalNumberGamesLost"] = (int)((int)gameStats["totalNumberGamesLost"] + 1);
                        gameStats["totalNumberGamesPlayed"] = (int)((int)gameStats["totalNumberGamesPlayed"] + 1);
                        // sleep for two seconds
                        //Thread.Sleep(2000);
                        ReadKey();
                        notFoundMovie = true;
                        break;
                    }
                }
                string tmp1 = new string(whichMovieMask);
                // display the current movie mask
                gGameGUI.displayMovieMask(tmp1);
            } else if (!(validLetter || isRightTick)) {
                // display error
                gGameGUI.displayError("                                                ");
                gGameGUI.displayError("Invalid Character");
            } else if (usedAlready) {
                // display error
                gGameGUI.displayError("                                                ");
                gGameGUI.displayError($"Used Already: ({lowerLetter})");   
            }
            // check have we found the answer
            for (int ii = 0, end = whichMovieMask.Length; ii < end; ii++) {
                if (whichMovieMask[ii] == '_') {
                    notFoundMovie = true;
                    break;
                }
            }
        } while (notFoundMovie);
        if (notFoundMovie == false) {
            // display win
            gGameGUI.displayWin("Win: Game Over");
            // set stats
            gameStats["currentWinLose"] = (int)0;
            gameStats["totalNumberGamesWon"] = (int)((int)gameStats["totalNumberGamesWon"] + 1);
            gameStats["totalNumberGamesPlayed"] = (int)((int)gameStats["totalNumberGamesPlayed"] + 1);
            // sleep for 3 seconds
            //Thread.Sleep(3000);
            ReadKey();
        }

    }

    // activate/create the main menu
    public void activateMainMenu(GameGUI gGameGUI, MenuGUI gMenuGUI, StatsGUI gStatsGUI, List<string> movies, Dictionary <string, object> gameStats, Random randomMovie) {
        // loop the menu
        while (true) {
            // clear the keys
            while (Console.KeyAvailable) {Console.ReadKey(true);}
            // clear screen
            gMenuGUI.clear();
            // display menu
            gMenuGUI.displayMenu();
            // read key
            var letterRead = ReadKey();
            // get the letter
            char letter = letterRead.KeyChar;
            bool validLetter = letter == '1' || letter == '2' || letter == '3';
            // is it valid (1, 2, 3)
            if (validLetter) {
                if (letter == '1') {
                    if (movies.Count == 0) {
                        // display error
                        gMenuGUI.displayError("No More Movies");
                        Thread.Sleep(1500);
                    } else {
                        // play hangman
                        gGameGUI.clear();
                        playHangMan(gGameGUI, movies, gameStats, randomMovie);
                    }
                } else if (letter == '2') {
                    // display the stats
                    gStatsGUI.display(gameStats);
                    ReadKey();
                } else if (letter == '3') {
                    // quit to command line
                    gMenuGUI.displayQuit("Quit Pressed... Thanks for playing.");
                    Environment.Exit(0);
                }
            } else {
                // display error
                gMenuGUI.displayError("Invalid Selection");
                // sleep for 1.5 seconds
                Thread.Sleep(1500);
            }
        }
    }
}

////////////////////////////////////////////////////////
// EOF
////////////////////////////////////////////////////////

