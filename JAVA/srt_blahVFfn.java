/*x00204556 cw (double v) */
import java.util.Arrays;

public class srt_blah {
    public static int gArrBurst[][];
    public static int globalPositionArray[] = new int[1000];
    public static int globalCounter = 0;
    public static int tq = 0;
    
    public static void main(String [] args) {
        if (args.length != 0) {
            looperSJR(args);
        }   else {
            String [] arg2 = {"tq:10", "0:80", "10:60", "130:20", "20:20", "50:30","0:60"};
            looperSJR(arg2);
        }
    }
    public static void looperSJR(String [] argz) {
        gArrBurst = new int[argz.length-1][3];
        for (int o = 0; o < argz.length; o++) {
            String tmpString[] = argz[o].split(":");
            if (tmpString[0].equals("tq")) {
                tq = Integer.parseInt(tmpString[1]);
            } else {
                gArrBurst[o-1][0] = Integer.parseInt(tmpString[0]);
                gArrBurst[o-1][1] = Integer.parseInt(tmpString[1]);
                gArrBurst[o-1][2] = o;
            }
                
        }
        printit(gArrBurst);
        Arrays.sort(gArrBurst, Arrays::compare);
        printit(gArrBurst);
        //for (int p = 0; p < argz.length; p++) {
        //int breakVal = 0;
        boolean b = true;
        while (b) {  
            b = processHeadJobFor1Quantum(gArrBurst);
            Arrays.sort(gArrBurst, Arrays::compare);
            printit(gArrBurst);
            if (b == false) {break;}
        }
        
        for (int i = 0; i < globalPositionArray.length && globalPositionArray[i] != -1; i++) {
            System.out.print("P:" + globalPositionArray[i] + " ");
        }
        System.out.println("");
    }
    public static void printit(int [][]gArrBurst) {
        for (int i = 0; i < gArrBurst.length; i++) {
            System.out.print(gArrBurst[i][0] + ":" + gArrBurst[i][1] + " ");
        }
        System.out.println();
    }
    public static boolean processHeadJobFor1Quantum(int [] [] gArrBurst) {
        boolean reVal = false;
        for (int i = 0; i < gArrBurst.length; i++) {
            if (gArrBurst[i][1] != 0) {
                gArrBurst[i][1] -= tq;
                int orig = gArrBurst[i][0];
                gArrBurst[i][0] += tq;
                globalPositionArray[globalCounter] = gArrBurst[i][2];
                globalCounter++;
                globalPositionArray[globalCounter] = -1;
                for (int j = 0; j < gArrBurst.length; j++) {
                    if (gArrBurst[j][1]!= 0) {
                        if (gArrBurst[j][0] == orig) { 
                            gArrBurst[j][0] += tq;
                        }
                    }
                }
                reVal = true;
                return reVal;
            }
        }
        return reVal;
    }
}