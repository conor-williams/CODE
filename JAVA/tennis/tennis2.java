/* cw 27/07/2020*/
import java.awt.*;
import java.awt.event.*;
import java.awt.geom.*;
import java.awt.AlphaComposite;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JOptionPane;
import javax.swing.JDialog;

public class tennis2 extends JPanel implements MouseListener {
    Graphics2D g2;
    static JFrame frame;
    static int racketPosition = 0;
    static int xC = 100;
    static int yC = 100;
    static int xDirection = 1;
    static int yDirection = 1;
    boolean gameOver = false;

    public static void main(String[] args) {
        frame = new JFrame();
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setPreferredSize(new Dimension(500, 520));
        frame.getContentPane().add(new tennis2());
	frame.setTitle("Mini Tennis - cw 2020");
        frame.pack();
        frame.setVisible(true);
    }
    public tennis2(){
        setBackground(Color.WHITE);
        addMouseListener(this);
    }

    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g);
      g2 = (Graphics2D)g;
      g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING,
         RenderingHints.VALUE_ANTIALIAS_ON);

      g2.draw3DRect(racketPosition, 450, 100, 20, true);
      g2.setColor(Color.black);
      g2.fillRect(racketPosition, 450, 100, 20);   

      if (xDirection == 1) {
	    xC+=25; xDirection = 1;
	    if (xC+40 < 480) {xDirection = 1;} else {xDirection = 0;} 
      } else if (xDirection == 0) {
	    xC-=15; xDirection = 0;
	    if (xC > 0) {xDirection = 0;} else {xDirection = 1;}
      }
      if (yDirection == 1) {      
	    if (yC+20 < 435) { yC+=20; yDirection = 1;} else {yDirection = 0; isGameOverP();}    
      } else if (yDirection == 0) {
             yC-=18; yDirection = 0;
            if (yC > 0) {yDirection = 0;} else {yDirection = 1;}
      }
	//yC+=20;
      g2.fillArc(xC, yC, 40, 40, 0, 360); 
      //Graphics2D g2d = (Graphics2D) g2;
      //g2d.setComposite(makeTransparent(0.5f));
      //g2.fillArc(xC - 10, yC - 10, 40, 40, 0, 360); 
	
      try {Thread.sleep(200);} catch (Exception e) {}
       
END:
	if (gameOver == true) {
             g2.drawString("Game Over - cw 2020", 100, 200);
             g2.drawString("- Press Mouse to restart...", 80, 240); 

	}    else {repaint(); }
    }
    private AlphaComposite makeTransparent(float alpha) {
	int type = AlphaComposite.SRC_OVER;
	return (AlphaComposite.getInstance(type, alpha));
    }
    public void warn(String cwMessage) {
	JOptionPane optionPane = new JOptionPane(cwMessage,JOptionPane.WARNING_MESSAGE);

	JDialog dialog = optionPane.createDialog("cw Mini Tennis 2020");
	dialog.setAlwaysOnTop(true); // to show top of all other application
	//dialog.setVisible(true);
	//repaint();
    }

    public boolean isGameOverP() {
        if (xC < racketPosition -20 || (xC > (racketPosition + 120)) ) {xC = 100; yC = 100; gameOver = true; return true;}
	else {gameOver = false;	return false;}
    }
    @Override
    public void mouseClicked(MouseEvent e) {
	int mx = e.getX();
	int my = e.getY();
	//System.out.println("in mouseClicked: " + mx + " : " + my);
	if (mx < racketPosition) {racketPosition -= 80;}
	else if (mx > racketPosition + 99) {racketPosition += 80;}
	
	gameOver = false; repaint();
    }
    @Override
    public void mouseEntered(MouseEvent e) {}
    @Override
    public void mouseExited(MouseEvent e) {}
    @Override
    public void mousePressed(MouseEvent e) {}
    @Override
    public void mouseReleased(MouseEvent e) {}
}