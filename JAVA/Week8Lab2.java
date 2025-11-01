/*
 * Lab 2
 * Write a program that allows a user to enter 5 results for each year of a 4 year course. The app should:

 * Give an average for each year

 * Give a total average
 */
//Conor Paul Williams - x25147862@student.ncirl.ie
///import from javax.swing and java.awt libs
import javax.swing.*;
import java.awt.*;

public class Week8Lab2 {
	public static void main(String [] args) {
		int numResults = 5;
		int numYears = 4;

		//build up the panel
		JPanel panel = new JPanel(new GridLayout(numYears+1, numResults+1));
		panel.add(new JLabel("Enter:"));
		for (int ii = 0; ii < numResults; ii++) {
			panel.add(new JLabel("result" + Integer.toString(ii+1)));
		}

		JTextField resField[][] = new JTextField[numYears][numResults];
		for (int jj = 0; jj < numYears; jj++) {
			panel.add(new JLabel("Year:" + Integer.toString(jj+1)));
			for (int ii = 0; ii < numResults; ii++) {
				resField[jj][ii] = new JTextField("", 10);
				//add to panel
				panel.add(resField[jj][ii]);
			}
		}

		//show the panel
		int result = JOptionPane.showConfirmDialog(null, panel, "Enter Details", JOptionPane.OK_CANCEL_OPTION);
		if (result == JOptionPane.OK_OPTION) {
			//JOptionPane.showMessageDialog(null, "result: " + res);
			double yearAvg[] = new double[numYears];
			double total = 0.0;
			String msg = "";
			double yearTot = 0.0;
			//generate the averages
			for (int yy = 0; yy < numYears; yy++) {
				for (int xx = 0; xx < numResults; xx++) {
					if (resField[yy][xx].getText().equals("")) {
					       continue;	
					}
					
					double res = Double.parseDouble(resField[yy][xx].getText());
					total += res;
					yearTot += res;

				}
				yearAvg[yy] = yearTot;
				double yearAv = yearTot/(double)numResults;
				msg += "Avg Year " + yy + ": " + yearAv + " (" + getGrade(yearAv) + ")\n";
				yearTot = 0.0;

			}

			//display results
			double tAvg = total/(numResults*numYears);
			JOptionPane.showMessageDialog(null, msg+ "\ntotalAvg: " + tAvg + " (" + getGrade(tAvg) + ")");
		}
	}
	//get the grades!
	public static char getGrade(double mark) {
		if (mark < 30.0) {
			return 'F';
		} else if (mark < 40.0) {
			return 'E';
		} else if (mark < 55.0) {
			return 'D';
		} else if (mark < 70.0) {
			return 'C';
		} else if (mark < 85.0) {
			return 'B';
		} else {
			return 'A';
		}
	}

}
