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
using static System.Console;
// GameGUI: the class used when playing/displaying the Game
public class GameGUI {
    // the constructor (clears screen)
    public GameGUI() {
        //Console.WriteLine("in GameGUI constructor");
        WriteLine("\x1B[2J");
    }
    // clear: clear the screen
    public void clear() {
        WriteLine("\x1B[2J");
    }
    // print of the stage
    public void printStage() {
        Write("\x1B[35m\x1B[1;4HHangman - Conor Williams - L00206159\x1B[0m");
        Write("\x1B[32m\x1B[3;5H|\x1B[0m");
        Write("\x1B[32m\x1B[4;5H|\x1B[0m");
        
        Write("\x1B[32m\x1B[2;5H-\x1B[0m");
        Write("\x1B[32m\x1B[2;4H-\x1B[0m");
        Write("\x1B[32m\x1B[2;3H-\x1B[0m");
        Write("\x1B[32m\x1B[2;2H-\x1B[0m");

        Write("\x1B[32m\x1B[3;2H|\x1B[0m");
        Write("\x1B[32m\x1B[4;2H|\x1B[0m");
        Write("\x1B[32m\x1B[5;2H|\x1B[0m");
        Write("\x1B[32m\x1B[6;2H|\x1B[0m");
        Write("\x1B[32m\x1B[7;2H|\x1B[0m");
        Write("\x1B[32m\x1B[8;2H|\x1B[0m");
        Write("\x1B[32m\x1B[9;2H|\x1B[0m");
        Write("\x1B[32m\x1B[10;2H|\x1B[0m");

        Write("\x1B[32m\x1B[11;2H-\x1B[0m");
        Write("\x1B[32m\x1B[11;3H-\x1B[0m");
        Write("\x1B[32m\x1B[11;4H-\x1B[0m");
        Write("\x1B[32m\x1B[11;5H-\x1B[0m");
        Write("\x1B[32m\x1B[11;6H-\x1B[0m");
        Write("\x1B[32m\x1B[11;7H-\x1B[0m");

        Write("\x1B[32m\x1B[9;3H-\x1B[0m");
        Write("\x1B[32m\x1B[10;4H\\\x1B[0m");

    }
    // printHead: prints the head in the gallows
    public void printHead() {
        WriteLine("\x1B[31m\x1B[5;5HO\x1B[0m");
    }
    // printBody: prints the body in the gallows
    public void printBody1() {
        WriteLine("\x1B[31m\x1B[6;5H|\x1B[0m");
       // WriteLine("\x1B[8;5H|");
    }
    public void printBody2() {
        WriteLine("\x1B[31m\x1B[7;5H|\x1B[0m");
    }
    // printLeftArm: prints the left arm in gallows
    public void printLeftArm() {
        WriteLine("\x1B[31m\x1B[6;4H/\x1B[0m");
        WriteLine("\x1B[31m\x1B[7;3H/\x1B[0m");
    }
    // printRightArm: prints the right arm in gallows
    public void printRightArm() {
        WriteLine("\x1B[31m\x1B[6;6H\\\x1B[0m");
        WriteLine("\x1B[31m\x1B[7;7H\\\x1B[0m");
    }
    // printLeftLeg: prints the left leg in gallows
    public void printLeftLeg() {
        WriteLine("\x1B[31m\x1B[8;4H/\x1B[0m");
        WriteLine("\x1B[31m\x1B[9;3H/\x1B[0m");
    
    }
    // printRightLeg: printts the right leg in gallows
    public void printRightLeg() {
        WriteLine("\x1B[31m\x1B[8;6H\\\x1B[0m");
        WriteLine("\x1B[31m\x1B[9;7H\\\x1B[0m");
    
    }
    // displayHangmanPart: picks the next body part
    //     to display, based on number of mistakes
    public void displayHangmanPart(int mistakes) {
        switch(mistakes) {
            case(1): {
                printHead();
                break;
            }
            case(2): {
                printBody1();
                break;
            }
            case(3): {
                printBody2();
                break;
            }
            case(4): {
                printLeftArm();
                break;
            }
            case(5): {
                printRightArm();
                break;
            }
            case(6): {
                printLeftLeg();
                break;
            }
            case(7): {
                printRightLeg();
                break;
            }
            default: {
                break;
            }
        }
    }
    // displayError: displays an error message in red
    public void displayError(string message) {
        Write($"\x1B[31m\x1B[7;15HError: {message}\x1B[0m");
    }
    // displayOk: display an ok message in green
    public void displayOk(string message) {
        Write($"\x1B[32m\x1B[7;15HSuccess: {message}\x1B[0m");
    }
    // displayMistakes: display number of mistakes
    public void displayMistakes(string message) {
        Write($"\x1B[31m\x1B[9;15H{message}\x1B[0m");
    }
    // displayAlready: displays what you have already
    //         tried
    public void displayAlready(string message) {
        Write($"\x1B[35m\x1B[5;15HAlready Used: {message}\x1B[0m"); 
    }
    // displayMovieMask: displays what you have worked 
    //      out so far
    public void displayMovieMask(string message) {
        Write($"\x1B[35m\x1B[13;3HMovie: {message}\x1B[0m"); 
    }
    // displayPrompt: displays the prompt for char
    public void displayPrompt(string message) {
        Write($"\x1B[33m\x1B[3;15H{message}\x1B[0m"); 
    }
    // displayLose: display the Lose message
    public void displayLose(string message) {
        Write($"\x1B[31m\x1B[15;2H{message} \x1B[33mPress any key...\x1B[0m"); 
    }
    // displalyWin: display the Win message
    public void displayWin(string message) {
        Write($"\x1B[32m\x1B[15;2H{message} \x1B[33mPress any key...\x1B[0m"); 
    }

}

////////////////////////////////////////////////////////
// EOF
////////////////////////////////////////////////////////
