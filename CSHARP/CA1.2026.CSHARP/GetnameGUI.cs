//////////////////////////////////////////
//
// Conor Williams - L00206159
//
// GetnameGUI.cs ---- get name handling class ---
//     1. One display method: displays to the screen the get name prompt
//
//////////////////////////////////////////
using static System.Console;

public class GetnameGUI {
    // GetnameGUI: the constructor
    public GetnameGUI() {}
    // display: display get name prompt on screen
    public void display() {
        Write("\x1B[2J");
        Write("\x1B[35m\x1B[1;4HHangman - Conor Williams - L00206159\x1B[0m");
        Write($"\x1B[34m\x1B[5;10HEnter Name: \x1B[0m");
    }
}

////////////////////////////////////////////////////////
// EOF
////////////////////////////////////////////////////////
