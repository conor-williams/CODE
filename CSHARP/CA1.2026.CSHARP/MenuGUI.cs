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
using static System.Console;
// MenuGUI: the class for displaying the Menu
public class MenuGUI {
    // MenuGUI: constructor
    public MenuGUI() {
        //Console.WriteLine("in MenuGUI constructor");
        
    }
    // displayMenu: display the menu
    public void displayMenu() {
        Write("\x1B[2J");
        Write("\x1B[35m\x1B[1;4HHangman - Conor Williams - L00206159\x1B[0m");
        Write("\x1B[32m\x1B[3;10H1. Play Again\x1B[0m");
        Write("\x1B[32m\x1B[4;10H2. View Statistics\x1B[0m");
        Write("\x1B[32m\x1B[5;10H3. Quit\x1B[0m");

        Write("\x1B[33m\x1B[7;10HEnter Selection(1,2 or 3): \x1B[0m");
    }
    // clear: clear the screen
    public void clear() {
        WriteLine("\x1B[2J");
    }
    // displayError: displays an error message in red
    public void displayError(string message) {
        Write($"\x1B[31m\x1B[9;10HError: {message}\x1B[0m");
    }
    // displayQuit: displays the quit message
    public void displayQuit(string message) {
        Write($"\x1B[32m\x1B[13;0H{message}\x1B[0m");
    }
    
}

////////////////////////////////////////////////////////
// EOF
////////////////////////////////////////////////////////
