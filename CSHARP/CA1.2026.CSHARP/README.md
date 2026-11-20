# Hangman - Conor Williams - L00206159 
There are 6 files  
	1. Hangman.cs 
	2. HangmanGame.cs 
	3. GameGUI.cs 
	4. GetnameGUI.cs 
	5. MenuGUI.cs 
	6. StatsGUI.cs 
 
## Hangman.cs 
--- 
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
--- 
 
## HangmanGame.cs 
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
--- 
 
## GameGUI.cs 
//////////////////////////////////////////////////////// 
// 
// Conor Williams - L00206159 
// 
// GameGUI.cs ---- handles the Game GUI ---- 
//     o. This class has methods which: 
//         1. clears the screen 
//         2. prints the stage (gallows) 
//         3. print the body parts depending on number of mistakes 
//         4. prints messages associated with the game such as 
//                 letters already used 
//     o. the gallows is displayed with the body parts 
//     o. depending on the number of mistakes, that number 
//             of body parts is displayed 
// 
//////////////////////////////////////////////////////// 
--- 
 
## GetnameGUI.cs 
////////////////////////////////////////// 
// 
// Conor Williams - L00206159 
// 
// GetnameGUI.cs ---- get name handling class --- 
//     1. One display method: displays to the screen the get name prompt 
// 
////////////////////////////////////////// 
--- 
 
## MenuGUI.cs 
///////////////////////////////////////////////////// 
// 
// Conor Williams - L00206159 
// 
// MenuGUI.cs ---- handles the menu - mainly display the menu ---- 
//     o. In DisplayMenu it shows the following 3 items: 
//          1. Play Again 
//          2. View Statistics 
//          3. Quit 
//     o. Pressing 1, play hangman again 
//     o. Pressing 2, viewing of current stats 
//     o. Pressing 3, causes the program to quit 
//     o. Other methods 
//          o. clear - clears the screen 
//          o. displayError - dislays an error (if not 1,2,3 pressed) 
//          o. displayQuit - display quit and exits program 
// 
///////////////////////////////////////////////////// 
--- 
 
## StatsGUI.cs 
////////////////////////////////////////// 
// 
// Conor Williams - L00206159 
// 
// StatsGUI.cs ---- statistics display handling class --- 
//     1. One display method: displays to the screen the stats menu 
//         o. takes as argument, a dictionary of the stats 
//         o. **the stats dictionary is created in Hangman.cs 
//         o. check the stats are not out of sync 
////////////////////////////////////////// 
--- 
 
Please see the comments in the files for more information. 
 
