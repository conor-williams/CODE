//////////////////////////////////////////
//
// Conor Williams - L00206159
//
// StatsGUI.cs ---- statistics display handling class ---
//     1. One display method: displays to the screen the stats menu
//         o. takes as argument, a dictionary of the stats
//         o. **the stats dictionary is created in Hangman.cs
//         o. checks the stats are not out of sync
//////////////////////////////////////////
using static System.Console;

public class StatsGUI {
    // StatsGUI: the constructor
    public StatsGUI() {}
    // display: display stats on screen
    public void display(Dictionary <string, object> gameStats) {
        Write("\x1B[2J");
        Write("\x1B[35m\x1B[1;4HHangman - Conor Williams - L00206159\x1B[0m");
        Write($"\x1B[34m\x1B[3;10HPlayerName: {(string)gameStats["playerName"]}\x1B[0m");
        Write($"\x1B[34m\x1B[4;10HCurrent Film Title (answer): {(string)gameStats["currentMovieTitle"]}\x1B[0m");
        string wonLost = (int)gameStats["currentWinLose"] == 0 ? "Won" : "Lost";
        int col = 32;
        if (wonLost == "Lost") {
            col = 31;
        }
        Write($"\x1B[{col}m\x1B[5;10HCurrent Game-Won/Lost: {wonLost}\x1B[0m");
        Write($"\x1B[34m\x1B[6;10HTotal Games Played: {(int)gameStats["totalNumberGamesPlayed"]}\x1B[0m");
        Write($"\x1B[32m\x1B[7;10HTotal Games Won: {(int)gameStats["totalNumberGamesWon"]}\x1B[0m");
        Write($"\x1B[31m\x1B[8;10HTotal Games Lost: {(int)gameStats["totalNumberGamesLost"]}\x1B[0m");
	// out of sync check
        if ((int)gameStats["totalNumberGamesPlayed"] != ((int)gameStats["totalNumberGamesWon"] + (int)gameStats["totalNumberGamesLost"])) {
        	Write($"\x1B[31m\x1B[9;10HError: Stats Out of Sync\x1B[0m");
	}
        Write($"\x1B[34m\x1B[10;7HPress any key to continue                 \x1B[0m");
    }
}

////////////////////////////////////////////////////////
// EOF
////////////////////////////////////////////////////////
