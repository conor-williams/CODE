/// conor paul williams - x25147862@student.ncirl.ie
/*
 * Rock Paper Scissors
Create an application that implements the functionality of Rock Paper Scissors. The user should first be asked to choose an option as an integer (Rock, Paper or Scissors) where Rock is 1, Paper is 2, and Scissors is 3.
The computer should also pick a random option (1, 2 or 3). Once both options have been chosen, the user should be told whether they have won lost or drawn in a format like the below.
You chose Rock, the Computer chose Paper
You lose!
Rules:
User	Computer	Result
Rock	Paper	Lose
Rock	Scissors	Win
Paper	Rock	Win
Paper 	Scissors	Lose
Any ties (rock/rock paper/paper scissors/scissors should produce a ‘draw’ message)

   */
import javax.swing.JOptionPane;

//the main public app class
public class RockApp {
	public static void main(String [] args) {
		//vars
		int userPick;
		int compPick = (((int)Math.random()) % 3);
		String msg;
		//for showOptionDialog
		Object [] options = {"Rock", "Paper", "Scissors"};
		//request inputs;
		userPick = JOptionPane.showOptionDialog(null, "Enter rock,paper,scissors: ", "continue", 
				JOptionPane.YES_NO_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, null, 
				options, options[2]);
		//constructor for Rock
		Rock rock = new Rock();
		//set vars
		rock.setUserPick(userPick);
		rock.setCompPick(compPick);
		//compute give and receive
		rock.computeWinner();
		//get the message
		msg = rock.getMsg();
		//output the output!
		//JOptionPane.showMessgeDialog(null, msg);
		System.out.println(msg);
	}
}

//the instantiable class
public class Rock {
	//vars
	private static final int ROCK = 0;
	private static final int PAPR = 1;
	private static final int SCIS = 2;
	private int userPick, compPick;
	private String msg;
	//constructor
	public Rock() {
		userPick = 0;
		compPick = 0;
		msg = "";
	}
	//setters
	public void setUserPick(int userPick) {
		this.userPick = userPick;
	}
	public void setCompPick(int compPick) {
		this.compPick = compPick;
	}
	//compute
	public void computeWinner() {
		if (userPick == compPick) {
			msg = "draw";
		} else {
			if (userPick == ROCK) {
				if (compPick == PAPR) {
					msg = "Lose";
				} else if (compPick == SCIS) {
					msg = "Win";
				}
			} else if (userPick == PAPR) {
				if (compPick == ROCK) {
					msg = "Win";
				} else if (compPick == SCIS) {
					msg = "Lose";
				}
			} else if (userPick == SCIS) {
				if (compPick == ROCK) {
					msg = "Lose";
				} else if (compPick == PAPR) {
					msg = "Win";
				}
			}
		}
		String userMsg = "unknown";
		if (userPick == ROCK) {
			userMsg = "rock";
		} else if (userPick == PAPR) {
			userMsg = "paper";
		} else if (userPick == SCIS) {
			userMsg = "scissors";
		}

		String compMsg = "unknown";
		if (compPick == ROCK) {
			compMsg = "rock";
		} else if (compPick == PAPR) {
			compMsg = "paper";
		} else if (compPick == SCIS) {
			compMsg = "scissors";
		}


		msg = msg + "\nuser chose: " + userMsg + "\ncomp chose: " + compMsg;
	}
	//getters
	public String getMsg() {
		return msg;
	}
}
