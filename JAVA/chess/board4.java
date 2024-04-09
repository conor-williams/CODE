/* cw 23/07/2020*/
import java.awt.*;
import java.awt.event.*;
import java.awt.geom.*;

import javax.swing.JFrame;
import javax.swing.JPanel;

public class board4 extends JPanel implements MouseListener {
    Graphics2D g2;
    static JFrame frame;
    boolean drawit = false;
    int moveTo;
    int fromtoSquare[][] = new int[2][2];
    Color goRedOrBlackColor;
    int goRedOrBlack = 5;
    boolean blackToGo = true;

    int allPieces[][] =  {
			{1, 2, 3, 4, 5, 3, 2, 1},
		     	{0, 0, 0, 0, 0, 0, 0, 0},
                     	{6,6,6,6,6,6,6,6},
			{6,6,6,6,6,6,6,6},
			{6,6,6,6,6,6,6,6},
			{6,6,6,6,6,6,6,6},
		     	{7, 7, 7, 7, 7, 7, 7, 7},
			{8, 9, 10, 11, 12, 10, 9, 8}

    	};

    {
        fromtoSquare[0][0] = -1;
        fromtoSquare[1][0] = -1;
    }

    public static void main(String[] args) {
        frame = new JFrame();
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setPreferredSize(new Dimension(500, 520));
        frame.getContentPane().add(new board4());
        frame.pack();
        frame.setVisible(true);
    }

    public board4(){

        setBackground(Color.WHITE);
        addMouseListener(this);
    }

    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g);
      g2 = (Graphics2D)g;
      g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING,
         RenderingHints.VALUE_ANTIALIAS_ON);
      Font font = new Font("Serif", Font.PLAIN, 20);
      g2.setFont(font);
      g2.drawString("Chess Board - cw 2020", 50, 70); 
      font = new Font("Serif", Font.PLAIN, 14);
      g2.setFont(font);
       
      g2.drawString("a", 40, 440);
      g2.drawString("b", 80, 440);
      g2.drawString("c", 120, 440);
      g2.drawString("d", 160, 440);
      g2.drawString("e", 200, 440);
      g2.drawString("f", 240, 440);
      g2.drawString("g", 280, 440);
      g2.drawString("h", 320, 440);

     
      // Draw the grid - 
      for (int i = 0; i <= 8; i++) { 
	  if (i != 0 && i != 8) {
              g2.draw3DRect(20 + i*40, 100, 1, 360, true);
          }  else {
	         g2.draw3DRect(20 + i*40, 100, 3, 320, true);
	   }
       }

      for (int i = 0; i <= 8; i++) {
	  if (i != 0 && i != 8) {
	          g2.draw3DRect(20, 100+i*40, 360, 1, true);
	  } else {
		  g2.draw3DRect(20, 100+i*40, 320, 3, true);
	  }		
	  if (i != 8) {
      	      g2.drawString(Integer.toString(i+1), 370, 100+(i+1)*40-5);
          }
	}


	if (fromtoSquare[0][0] != -1 && fromtoSquare[1][0] != -1) {
	     //move piece...
	     System.out.println("both not minus");
	            int prev = allPieces[fromtoSquare[0][1]][fromtoSquare[0][0]]; 
		    if (prev != 6 && ((prev < 6 && blackToGo == false) || (prev > 6 && blackToGo))) {
			allPieces[fromtoSquare[0][1]][fromtoSquare[0][0]] = 6;  
			allPieces[fromtoSquare[1][1]][fromtoSquare[1][0]] = prev;  
		        goRedOrBlack = prev;
			System.out.println("goRedOrBlack is: " + goRedOrBlack);
			
                    }
             fromtoSquare[0][0] = -1;
	     fromtoSquare[1][0] = -1;
        } else if (fromtoSquare[0][0] !=-1) {
		if (allPieces[fromtoSquare[0][1]][fromtoSquare[0][0]] == 6 ) {
			System.out.println("is blank...");
			fromtoSquare[0][0] = -1;
      		        fromtoSquare[1][0] = -1;
		}
	}

	drawAllPieces();
	if (goRedOrBlack < 6) {
	    goRedOrBlackColor = Color.black;
	    blackToGo = true;
         } else {
	    goRedOrBlackColor = Color.red;
	    blackToGo = false;
	 }			
       
	g2.setColor(goRedOrBlackColor);
	g2.draw3DRect(460, 460, 20, 20, true);
	
   }

   public void drawAllPieces() {
    Font font = new Font("Serif", Font.PLAIN, 24);
      g2.setFont(font);
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			int toxy[] = new int[2];
             		toxy = toMouse(j + 1, i + 1);
			
			if (allPieces[i][j] == 0) {
				g2.setColor(Color.red);
				drawPawn(toxy[0], toxy[1]);
				g2.setColor(Color.black);
				//System.out.println("drawing Pawn.." + toxy[0] + " " + toxy[1]);
			} else if (allPieces[i][j] == 1) {
				g2.setColor(Color.red);
				 g2.drawString("r", toxy[0]+20, toxy[1]+25); 
				g2.setColor(Color.black);
			} else if (allPieces[i][j] == 2) {
				g2.setColor(Color.red);
				g2.drawString("h", toxy[0]+20, toxy[1]+25);
				g2.setColor(Color.black);
			} else if (allPieces[i][j] == 3) {
				g2.setColor(Color.red);
				g2.drawString("b", toxy[0]+20, toxy[1]+25);
				g2.setColor(Color.black);
			} else if (allPieces[i][j] == 4) {
				g2.setColor(Color.red);
				g2.drawString("K", toxy[0]+20, toxy[1]+25);
				g2.setColor(Color.black);
			} else if (allPieces[i][j] == 5) {
				g2.setColor(Color.red);
				g2.drawString("Q", toxy[0]+20, toxy[1]+25);
				g2.setColor(Color.black);
			} else if (allPieces[i][j] == 7) {
				g2.setColor(Color.black);
				drawPawn(toxy[0], toxy[1]);
				g2.setColor(Color.black);
				//System.out.println("drawing Pawn.." + toxy[0] + " " + toxy[1]);
			} else if (allPieces[i][j] == 8) {
				g2.setColor(Color.black);
				 g2.drawString("r", toxy[0]+20, toxy[1]+25); 
				g2.setColor(Color.black);
			} else if (allPieces[i][j] == 9) {
				g2.setColor(Color.black);
				g2.drawString("h", toxy[0]+20, toxy[1]+25);
				g2.setColor(Color.black);
			} else if (allPieces[i][j] == 10) {
				g2.setColor(Color.black);
				g2.drawString("b", toxy[0]+20, toxy[1]+25);
				g2.setColor(Color.black);
			} else if (allPieces[i][j] ==11) {
				g2.setColor(Color.black);
				g2.drawString("K", toxy[0]+20, toxy[1]+25);
				g2.setColor(Color.black);
			} else if (allPieces[i][j] == 12) {
				g2.setColor(Color.black);
				g2.drawString("Q", toxy[0]+20, toxy[1]+25);
				g2.setColor(Color.black);
			}
		}
	}
   }


   public void drawPawn(int i, int y) {
        g2.draw3DRect(i+10, y+10, 20, 20, true);
	g2.draw3DRect(i+20, y+10, 1, 20, true);
	g2.draw3DRect(i+10, y+20, 20, 1, true);

   }
   public int[] toMouse(int sx, int sy) {
       int xstart = 20;
       int xinterval = 40;
	int yinterval = 40;
       int ystart = 100;
       int xy[] = new int[2];
       
       xy[0] = (sx-1)*xinterval + xstart;
       xy[1] = (sy-1)*yinterval + ystart;
	
       return xy;

 
   }

   public int[] toSquare(int mx, int my) {
       int xstart = 20;
       int xinterval = 40;
	int yinterval = 40;
       int ystart = 100;
       int xy[] = new int[2];
       xy[0] = (mx - xstart)/xinterval + 1;
       xy[1] = (my - ystart)/yinterval + 1;
       return xy;
   }
    @Override
    public void mouseClicked(MouseEvent e) {
	int xy[] = toSquare(e.getX(), e.getY());
        if ((xy[0] >= 1 && xy[0] <=8) && (xy[1] >= 1 && xy[1] <=8)) {
	    if (fromtoSquare[0][0] == -1) {
		fromtoSquare[0][0] = xy[0] - 1;
		fromtoSquare[0][1] = xy[1] - 1;
		repaint();
            } else if (fromtoSquare[1][0] == -1) {
		fromtoSquare[1][0] = xy[0] - 1;
		fromtoSquare[1][1] = xy[1] - 1;
		repaint();
            }
	    

        } else {
		System.out.println("out of bounds...");
        }

	System.out.println("     square  : " + xy[0] + " : " + xy[1]);
       
}

    @Override
    public void mouseEntered(MouseEvent e) {
    }

    @Override
    public void mouseExited(MouseEvent e) {
    }

    @Override
    public void mousePressed(MouseEvent e) {
    }

    @Override
    public void mouseReleased(MouseEvent e) {
    }

}
