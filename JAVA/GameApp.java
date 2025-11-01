/// conor paul williams - x25147862@student.ncirl.ie
/*
 * Lab 4
 Freeform Lab: Create a short text adventure story based game where the user must make choices to continue the story. The story should use either logical operators or nested if statements.
Inspiration: http://textadventures.co.uk/

*/
import javax.swing.JOptionPane;

//the main public app class
public class GameApp {
	public static void main(String [] args) {
		//vars
		String name;
		String msg;
		//request inputs;
		name = JOptionPane.showInputDialog(null, "Enter name : ");
		//constructor for Game
		Game game = new Game();
		//set vars
		game.setName(name);
		//compute give and receive
		game.computeGame();
		//get the message
		msg = game.getMsg();
		//output the output!
		JOptionPane.showMessageDialog(null, name + ", " + msg);
		//System.out.println(msg);

	}
}

//the instantiable class
public class Game {
	//vars
	private String name;
	private String msg;
	//Room Cost per night
	//constructor
	public Game() {
		name = "";
		msg = "";
	}
	//setters
	public void setName(String name) {
		this.name = name;
	}
	//compute
	public void computeGame() {
		Room room0 = new Room();
		room0.setRoomNum('0');
		room0.setText("This is the main market square\nChoose:");
		Object [] options0 = {"North", "East", "South", "West"};
		room0.setOptions(options0);
		////
		Room room1 = new Room();
		room1.setRoomNum('1');
		room1.setText("This is the coffe stall\nThis is where you play if you are\n" +
				"carrying something special\nChoose:");
		Object [] options1 = {"South", "Play"};
		room1.setOptions(options1);
		////
		GoldRoom room2 = new GoldRoom();
		room2.setRoomNum('2');
		room2.setText("This is the coins stall\nThe vendor has 2 coins\nA penny and a gold coin\nChoose:");
		Object [] options2 = {"West", "Buy Penny", "Buy Gold Coin"};
		room2.setOptions(options2);

		Room room3 = new Room();
		room3.setRoomNum('3');
		room3.setText("This is the nuclear stall\nYou can buy some plutonium\nChoose:");
		Object [] options3 = {"North", "Buy Plutonium"};
		room3.setOptions(options3);
		//
		Room room4 = new Room();
		room4.setRoomNum('4');
		room4.setText("This is the food stall\nYou can buy some food\nChoose");
		Object [] options4 = {"East", "Buy Food"};
		room4.setOptions(options4);


		char currentRoom = room0.getRoomNum();

		boolean dead = false;
		boolean win = false;
		do {
			if (currentRoom == room0.getRoomNum()) {
				int pick = JOptionPane.showOptionDialog(null, room0.getText(), "continue",
						JOptionPane.DEFAULT_OPTION, JOptionPane.PLAIN_MESSAGE, 
						null, room0.getOptions(), room0.getOptions()[0]);
				if (pick == 0) {
					currentRoom = room1.getRoomNum();
				} else if (pick == 1) {
					currentRoom = room2.getRoomNum();
				} else if (pick == 2) {
					currentRoom = room3.getRoomNum();
				} else if (pick == 3) {
					currentRoom = room4.getRoomNum();
				}
			} else if (currentRoom == room1.getRoomNum()) {
				int pick = JOptionPane.showOptionDialog(null, room1.getText(), "continue",
						JOptionPane.DEFAULT_OPTION, JOptionPane.PLAIN_MESSAGE, 
						null, room1.getOptions(), room1.getOptions()[0]);
				if (pick == 0) {
					currentRoom = room0.getRoomNum();
				} else if (pick == 1) {
					if (!room2.getGoldCoin()) {
						JOptionPane.showMessageDialog(null, "You do not have something " +
								"in your posession\n"  +
								"to allow you to play the game\n\nkeep looking, continue on");
					} else {

						String coffeeMsg = "Great, you hold the Gold Coin\nHere is the puzzle: \n\n" +
							"The Vendor has three types of coffee in front of him\n"+
							"One is light, one is medium and one is strong\n"+
							"Choose his favourite: ";
						Object [] opts = {"Light", "Medium", "Strong"};

						int pick2 = JOptionPane.showOptionDialog(null, coffeeMsg, "continue",
								JOptionPane.DEFAULT_OPTION, JOptionPane.PLAIN_MESSAGE, 
								null, opts, opts[0]);
						if (pick2 == 0) {
							msg = "Light is not his " +
								"favourite, you lose!";
							dead = true;

						} else if (pick2 == 1) {
							msg = "Medium is not his favourite, you lose!";
							dead = true;
						} else if (pick2 == 2) {

							msg = "Strong, ah yes, strong, " +
								"like Jack Reacher likes it, you win!!";
							win = true;
						}
					}
				}

			} else if (currentRoom == room2.getRoomNum()) {
				int pick = JOptionPane.showOptionDialog(null, room2.getText(), "continue",
						JOptionPane.DEFAULT_OPTION, JOptionPane.PLAIN_MESSAGE, 
						null, room2.getOptions(), room2.getOptions()[0]);
				if (pick == 0) {
					currentRoom = room0.getRoomNum();
				} else if (pick == 2) {
					JOptionPane.showMessageDialog(null, "You buy the gold coin");
					room2.setGoldCoin(true);
				} else if (pick == 1) {
					msg = "You buy the penny but...\n" +
						"It is a bent penny, you lose!";
					dead = true;
				}

			} else if (currentRoom == room3.getRoomNum()) {
				int pick = JOptionPane.showOptionDialog(null, room3.getText(), "continue",
						JOptionPane.DEFAULT_OPTION, JOptionPane.PLAIN_MESSAGE, 
						null, room3.getOptions(), room3.getOptions()[0]);
				if (pick == 0) {
					currentRoom = room0.getRoomNum();
				} else if (pick == 1) {
					msg = "You buy the plutonium but...\n" +
						"it is dangerously radioactive, you lose!";
					dead = true;
				}
			} else if (currentRoom == room4.getRoomNum()) {
				int pick = JOptionPane.showOptionDialog(null, room4.getText(), "continue",
						JOptionPane.DEFAULT_OPTION, JOptionPane.PLAIN_MESSAGE, 
						null, room4.getOptions(), room4.getOptions()[0]);
				if (pick == 0) {
					currentRoom = room0.getRoomNum();
				} else if (pick == 1) {
					msg = "You buy food but...\nit is poisonous, you lose!";
					dead = true;
				}
			}
		} while (!dead && !win);

	}
	//getters
	public String getMsg() {
		return msg;
	}
}
public class Room {
	private static final int NUM_ROOMS = 5;
	//vars

	private char roomNum;
	private String text;
	private Object [] options;

	//construtor
	public Room() {
		text = "";
	}
	//setters
	public void setRoomNum(char roomNum) {
		this.roomNum = roomNum;
	}
	public void setText(String text) {
		this.text = text;
	}
	public void setOptions(Object [] options) {
		this.options = options;
	}
	///getters
	public char getRoomNum() {
		return roomNum;

	}
	public Object[] getOptions() {
		return options;
	}

	public String getText() {
		return text;
	}
}
public class GoldRoom extends Room {//only one room has the gold coin
	//vars
	private boolean goldCoin;
	//consturctor
	public GoldRoom() {
		goldCoin = false;
	}
	//setters

	public void setGoldCoin(boolean goldCoin) {
		this.goldCoin = goldCoin;
	}
	//getters
	public boolean getGoldCoin() {
		return goldCoin;
	}
}
