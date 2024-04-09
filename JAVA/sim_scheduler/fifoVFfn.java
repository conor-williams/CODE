/* x00204556 conor.williams@mytudublin.ie */
/* sort q - then process head of the q */
import java.util.Comparator;
import java.util.Arrays;

public class fIFO {
    public static int [][]gArrBurst;
    public static int []globalPosArray;
    public static int globalCounter = 0;
    public static void main(String[] args) {
        System.out.println("hello");
        if (args.length != 0) {
            looperFIFOorFCFS(args);
        } else {
            String builtinArgs[] = {"0:80" , "10:60", "20:20", "50:30", "130:20", "0:60"};
            looperFIFOorFCFS(builtinArgs);
        }
    }
    public static void looperFIFOorFCFS(String[] argz) {
        gArrBurst = new int[argz.length][3];
        globalPosArray = new int[argz.length];
        
        for (int i = 0; i< argz.length; i++) {
            String str[] = argz[i].split(":");
            gArrBurst[i][0] = Integer.parseInt(str[0]);
            gArrBurst[i][1] = Integer.parseInt(str[1]);
            gArrBurst[i][2] = i; //original Pos
        }
        Arrays.sort(gArrBurst, Arrays::compare); //sort it...
        //sortIt(gArrBurst, 2);
        for (int i=0; i< gArrBurst.length; i++) {System.out.print(gArrBurst[i][0] + ":" + gArrBurst[i][1] + " ");}
        System.out.println();
        for (int i = 0; i < argz.length; i++) {
            if (gArrBurst[i][1] != 0) {
                gArrBurst[i][1] = 0;
                globalPosArray[globalCounter] = gArrBurst[i][2];
                globalCounter++;
            }
        }
        for (int i = 0; i < argz.length; i++) {
            System.out.print("P" + globalPosArray[i] + " ");
        }
        System.out.println("");
    }
    
    public static void sortIt(int [][]arr, int column) {
      Arrays.sort(arr, new Comparator<int[]>() {
           public int compare(int [] frst, int [] scnd) {
                         
            if (frst[column-1] > scnd[column -1]) {                    
                return 1;
            } else {
                return -1;
            }
            }});    
   
    }
}