/* shortest job first */
/* x00204556 cw       */

/* look at the q arrivals and take the shortest one to completion */
import java.util.Comparator;
import java.util.Arrays;
public class sJF {
    public static int gArrBurst[][];
    public static int []globalJobsProcessed;
    public static int globalCounter = 0;
    public static int gJobLength = 0;
    
    public static void main(String []args) {
        System.out.println("main..");
        
        if (args.length == 0) {
            String [] builtin = {"0:80", "10:60", "20:20", "50:30","130:20", "0:60"};
            looperSJF(builtin);
        } else {
            looperSJF(args);
        }
    }
    
    public static void looperSJF(String []argu) {
        gArrBurst = new int [argu.length][3];
        globalJobsProcessed = new int [argu.length];
        for (int i=0; i < argu.length; i++) {
            String tmpString[] = argu[i].split(":");
            gArrBurst[i][0] = Integer.parseInt(tmpString[0]);
            gArrBurst[i][1] = Integer.parseInt(tmpString[1]);
            //System.out.println(gArrBurst[i][0] + " -- " + gArrBurst[i][1]);
        }
        markJobsOriginalPosition(gArrBurst);
        int noJobToBeProcessed = 0;
        while (noJobToBeProcessed != -1) {
            columnWiseSorting2ndLow(gArrBurst, 1);
            //printit(gArrBurst);
            //specialShortestJobToHeadOfArray();
            noJobToBeProcessed = processJobAtHeadOfTheQSimpleAs(gArrBurst);
            printit(gArrBurst);//public static int gTime = 0;
        }
        
        for (int i=0; i< gArrBurst.length; i++) {
            System.out.print("P:" + globalJobsProcessed[i] + " ");
        }
        System.out.println("");
    }
    public static void markJobsOriginalPosition(int [][]gArrBurst) {
        for (int i=0; i < gArrBurst.length; i++) {
            gArrBurst[i][2] = i;
        }   
    }
    //at zero check for the shortest job(2nd param) in gArrBurst...
    //then complete the job... then at 80 check all the wait times are less
    // then 80 then select 20:20 then at 100 check all the wait times and
    // select 50:30 and run to ...
    //--- maybe at 80 move all the wait times to 80 if < 80
    //-- keep a process counter global
    //--need a global time on run counter...
    //sortBasedOnWaitingTimeLowAndBurstTimeLow();
    public static void specialShortestJobToHeadOfArray() {
        for (int i = 0; i< gArrBurst.length; i++) {
            
        }
        
    }
    public static int processJobAtHeadOfTheQSimpleAs(int [][]gArrBurst) {
        int retVal = -1;
        for (int i = 0; i< gArrBurst.length; i++) {
            if (gArrBurst[i][1] != 0) {
                if(retVal == -1) {retVal = 0;}
                //head of queue standing on its own
                gJobLength += gArrBurst[i][1];
                gArrBurst[i][1] = 0;
                globalJobsProcessed[globalCounter] = gArrBurst[i][2];
                globalCounter++;
                for (int j = i+1; j< gArrBurst.length; j++) {
                    if (gArrBurst[j][0] < gJobLength) {
                        gArrBurst[j][0] = gJobLength;
                    }
                }
                break;
            }         
        }
        return retVal;
    }
        
    public static void printit(int gArrBurst[][]) {
        for (int i=0; i<gArrBurst.length; i++) {
            System.out.print(gArrBurst[i][0] + ":" + gArrBurst[i][1] + "p" + gArrBurst[i][2] + " ");
        }
        System.out.println("");
    }
    
    public static void columnWiseSorting2ndLow(int arr[][], int column) {
       Arrays.sort(arr, Arrays::compare);

/*       
       Arrays.sort(arr, new Comparator<int[]>() {
           public int compare(int [] frst, int [] scnd) {
                         
            if (frst[column-1] > scnd[column -1]) {                    
                return 1;
            } else {
                return -1;
            }
            }});    
*/
  }
        
    
}