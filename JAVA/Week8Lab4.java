/*
   Lab 4
   (Difficult)

   Create a simple game of Battleship using 2D arrays. The user should be able to position their ship in a 3x3 array, as should the computer. Each ‘player’ should take turns choosing spots in the grid until someone hits the other ship.

Note: For simplicity, each player will only have 1 ship, that takes up 1 gid slot

Note: For simplicity, do not worry about repeated guesses
*/

//Conor Paul Williams - x25147862@student.ncirl.ie
//
import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.net.URL;
import javax.imageio.ImageIO;
import java.io.*;
import java.util.Vector;
import java.util.Objects;


@SuppressWarnings("unchecked")
public class Week8Lab4 {
	//some globals...
	public static int shipPos;
	public static JFrame frame;
	public static int userTries = 0;
	public static int sizeX = 6;
	public static int sizeY = 6;
	public static int gridPlayer[][];
	public static int gridComp[][];
	public static int alreadyComp[][];
	public static JButton buttonsComp[][];
	public static JButton buttonsPlayer[][];
	public static boolean playerGo = true;
	public static boolean start = true;
	public static Vector <Tuple<Integer, Integer>> already = new Vector<>();
	public static void main(String[] args) {
		//grid will be size*size
		//flatten of a 2D array to a 1D of size*size
		gridPlayer = new int[sizeY][sizeX];
		gridComp = new int[sizeY][sizeX];
		//ship is one unit @ shipPos (random)
		int shipPosY = (int)(Math.random() * sizeY);
		int shipPosX = (int)(Math.random() * sizeX);
		gridComp[shipPosY][shipPosX] = -99;

		JOptionPane.showMessageDialog(null, "Mark your ship position in the 'P' grid");
		//gridPlayer[2][2] = -101;
		//comment this line for no cheat printed
		System.out.println("(cheat) Comp Ship@:" + shipPosY + "," + shipPosX );

		//create the frame with the buttons
		frame = new JFrame("This is battleship, find Computers battleship 'C' squares");
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setSize(600, 600);

		// Create a panel with a grid layout
		JPanel panel = new JPanel(new GridLayout(sizeY*2, sizeX)); // sizexsize grid

		buttonsComp = new JButton[sizeY][sizeX];

		for (int yy = 0; yy < sizeY; yy++) {
			for (int xx = 0; xx < sizeX; xx++) {
				buttonsComp[yy][xx] = new JButton();
				buttonsComp[yy][xx].setText("C:" + yy + "," + xx);
				buttonsComp[yy][xx].addActionListener(new ButtonClickListener());
				buttonsComp[yy][xx].setActionCommand("C," + Integer.toString(yy)+","+Integer.toString(xx));
				buttonsComp[yy][xx].setBackground(Color.BLUE);

				panel.add(buttonsComp[yy][xx]);
			}
		}
		//user cells
		buttonsPlayer = new JButton[sizeY][sizeX];
		for (int yy = 0; yy < sizeY; yy++) {
			for (int xx = 0; xx < sizeX; xx++) {
				//JButton button = new JButton();
				buttonsPlayer[yy][xx] = new JButton();
				buttonsPlayer[yy][xx].setText("P:" + yy + "," + xx);
				buttonsPlayer[yy][xx].addActionListener(new ButtonClickListener());
				buttonsPlayer[yy][xx].setActionCommand("P," + Integer.toString(yy)+","+Integer.toString(xx));
				///button.setActionCommand(Integer.toString(i));

				panel.add(buttonsPlayer[yy][xx]);
			}
		}


		// Add the panel to the frame
		frame.add(panel);
		frame.setVisible(true);
		frame.setResizable(true);
	}

	// ActionListener to handle button clicks
	static class ButtonClickListener implements ActionListener {
		@Override
		public void actionPerformed(ActionEvent e) {
			System.out.println("in action...");
			JButton source = (JButton) e.getSource();
			String buttonID = e.getActionCommand();
			System.out.println("clicked id: (" + source.getText() + ") " + buttonID);
			String qq[] = buttonID.split("[,]");
			//System.out.println(buttonID);
			//System.out.println(shipPosY);
			//System.out.println(shipPosX);
			/*
			System.out.println("start: " + start);
			System.out.println("qq[0]: " + qq[0]);
			System.out.println("qq[0]: " + qq[1]);
			System.out.println("qq[0]: " + qq[2]);
			System.out.println("playerGo: " + playerGo);
			*/

			if (start && qq[0].equals("P") ) {
				start = false;
				buttonsPlayer[Integer.valueOf(qq[1])][Integer.valueOf(qq[2])].setBackground(Color.GREEN);
				gridPlayer[Integer.valueOf(qq[1])][Integer.valueOf(qq[2])] = -101;
				JOptionPane.showMessageDialog(null, "Now your turn, select a 'C' square");
				return;
			} else if (start) {
				return;
			}


			if (qq[0].equals("P") && playerGo == true) {return;}
			//if (qq[0].equals("C") && playerGo == false) {return;}

			if (qq[0].equals("C") && playerGo == true) {
				userTries++;
				if (gridComp[Integer.valueOf(qq[1])][Integer.parseInt(qq[2])] == -99) {
					System.out.println("Player makes the hit and wins sinking computers battleship...");	
					try {
                                                frame.setVisible(false);
                                                Thread.sleep(300);
                                                frame.setVisible(true);
                                                Thread.sleep(300);
                                                frame.setVisible(false);
                                                Thread.sleep(300);
                                                frame.setVisible(true);
                                                Thread.sleep(300);
                                                frame.setVisible(false);
                                                Thread.sleep(300);
                                                frame.setVisible(true);
                                                Thread.sleep(300);
                                                frame.setVisible(false);
                                        } catch (InterruptedException ex1) {
                                        }

					String ex = " tries.";
					if (userTries == 1) {ex = " try.";}
					JOptionPane.showMessageDialog(null, "      Direct hit\n  You sank his ship\n" +
							"************************\nyou win in " +
							userTries + ex);
					Runtime.getRuntime().halt(0);
				}
				source.setBackground(Color.RED);
				source.setEnabled(false);
				do {//make a guess and ensure it is a unique guess
					Integer guessCompY = (int)(Math.random() * sizeY);
					Integer guessCompX = (int)(Math.random() * sizeX);
					Tuple <Integer, Integer> tup = new Tuple(guessCompY, guessCompX);
					if (already.contains(tup)) {System.out.println("already contains"); continue;}
					already.add(tup);
					System.out.println("guessComp: " + guessCompY + " " + guessCompX);
				
					playerGo = false;
					buttonsPlayer[guessCompY][guessCompX].doClick();
					buttonsPlayer[guessCompY][guessCompX].setBackground(Color.BLACK);
					break;

				} while (true);
				System.out.println("player just took go... computer go playerGo to FALSE ****");
			} else if (qq[0].equals("P") && playerGo == false) {
				if (gridPlayer[Integer.valueOf(qq[1])][Integer.parseInt(qq[2])] == -101) {
					System.out.println("Computer wins, he hit your green square, your ship...");	
					try {
                                                frame.setVisible(false);
                                                Thread.sleep(300);
                                                frame.setVisible(true);
                                                Thread.sleep(300);
                                                frame.setVisible(false);
                                                Thread.sleep(300);
                                                frame.setVisible(true);
                                                Thread.sleep(300);
                                                frame.setVisible(false);
                                                Thread.sleep(300);
                                                frame.setVisible(true);
                                                Thread.sleep(300);
                                                frame.setVisible(false);
                                        } catch (InterruptedException ex1) {
                                        }
					String ex = " tries.";
					if (userTries == 1) {ex = " try.";}
					JOptionPane.showMessageDialog(null, "****Computer wins...\nIn " + userTries + ex);
					Runtime.getRuntime().halt(0);
				}
				System.out.println("player go to true");
				playerGo = true;
				//buttonsPlayer[guessCompY][guessCompX].setBackground(Color.BLACK);
				/*
				source.setBackground(Color.RED);
				source.setEnabled(false);
				int guessCompY = (int)(Math.random() * sizeY);
				int guessCompX = (int)(Math.random() * sizeX);
				System.out.println("guessComp: " + guessCompX + " " + guessCompY);
				buttonsPlayer[guessCompY][guessCompX].doClick();
				buttonsPlayer[guessCompY][guessCompX].setBackground(Color.BLACK);
				*/
			}



		}
	}
}

@SuppressWarnings("unchecked")
class Tuple<X,Y> {
        public X first;
        public Y second;

        public Tuple(X first, Y second) {
                this.first = first;
                this.second = second;
        }
        @Override
        public boolean equals(Object o) {
                Tuple tu2 = (Tuple) o;
                if (this == o) return true;
                if (!(o instanceof Tuple)) return false;
                if (!this.first.equals(tu2.first)) {return false;}
                if (!this.second.equals(tu2.second)) {return false;}

                return true;
        }
        @Override
        public int hashCode() {
                return Objects.hash(first, second);
        }

}

