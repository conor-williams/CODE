/*conorx00204556 p williams... conor.williams@gmail.com*/
/*Round Robin functioning with a static arrival q */
/* issue now: when two jobs have the same arrival time...*/
/* currently letting the computer decide...*/
/* maybe modify sort compartor at a later date */
/* -- two schools of thought... c.sw.!! */
import java.util.Comparator;
import java.util.Arrays;
/*note could be flawed (flaw fix i'm nearly sure shur!!)...late 231022 7am up all night*/
class roundRobinSched {
  public static int globalProcessedArray[] = new int[1000];
  public static int globalCounter = 0;
  public static int totalJobsTime = 0;
  public static void main (String args[])
  {
    System.out.println("hello world...");
    if (args.length == 0) {
        String [] builtInArgs = {"tq:3", "0:8", "5:2", "1:7", "6:3", "8:5"};
        looper(builtInArgs);
    } else {
        looper(args);  
    }  
}

public static void looper(String args[]) {
    int tq = 0;
    int len = args.length;
    int arrivalsAndBurst[][] = new int[len-1][3];
    tq = captureJobs(args, arrivalsAndBurst);
    markOriginalPosition(arrivalsAndBurst, 2);
    columnWiseSorting(arrivalsAndBurst, 1);
    printOutIntArray(arrivalsAndBurst);
    //take all the jobs of arrival time between 0 and tq and run them
    //    ensuring that you subtract from the jobs intended length
    // now the jobs between 1*tq and 2*tq go onto the queue
    //    now the jobs between 2*tq and 3*tq go onto the queue
    // now the jobs between 3*tq and 4*tq yada yada...
    //columnWiseSorting(arrivalsAndBurst, 1);
    // **correct** -- markOriginalPosition(arrivalsAndBurst, 2);
    printOutIntOriginalPosFlat(arrivalsAndBurst, 2);
    int shouldwebreak = 0;
    //for (int j=1; j<50 /*until all are [][k] are zero*/; j++) {
    int loopControl = 1;
    int totalJobsTime = 0;
    int tJT = 0;
    int tCounter = 0;
    while (shouldwebreak != -1) {
        tCounter++;
    //for (int i = 0; i < 10; i++) { 
        // the big breakthrough was to process one job then order the jobs... NB**
        //tJT = takeTheThreeGroupAndProcess(arrivalsAndBurst, 0, totalJobsTime + (tq*loopControl), tq);
        //instead do one job at a time the sort...
        takeOneJob(arrivalsAndBurst, tq, tCounter);
        totalJobsTime = tJT;
        shouldwebreak = shouldwebreakFunc(arrivalsAndBurst);
    //first pass...
        System.out.println("-------BEFORE ARRANGE------------");
        printOutIntArrayFlat(arrivalsAndBurst);
        //then re-order
        columnWiseSorting(arrivalsAndBurst, 1);
        printOutIntArrayFlat(arrivalsAndBurst);
        System.out.println("-------AFTER ARRANGE------------");
        //if (shouldwebreak == -1) break;
        if (shouldwebreak == -1) break;
        loopControl++;
    }
    printOutIntOriginalPosFlat(arrivalsAndBurst, 2);
    //takeTheThreeGroupAndProcess(arrivalsAndBurst, 0, 2*tq);
    //printOutIntArrayFlat(arrivalsAndBurst);
    //takeTheThreeGroupAndProcess(arrivalsAndBurst, 0, 3*tq);
    for (int i = 0; i<globalProcessedArray.length; i++) {
        if (globalProcessedArray[i] == 0) {System.out.println("zero: "); break;}
        System.out.print("p" + globalProcessedArray[i] + " ");
    }
    System.out.print("");
}
   public static int shouldwebreakFunc(int [][]arrivalsAndBurst) {
       for (int i=0; i<arrivalsAndBurst.length; i++) {
           if (arrivalsAndBurst[i][1] != 0) {return 0;}
       }
       return -1;
   }
   
  public static void printOutIntOriginalPosFlat(int [][]arrivalsAndBurst, int posLocator1) {
    for (int i = 0; i<arrivalsAndBurst.length; i++) {
        System.out.print(arrivalsAndBurst[i][posLocator1] + " ");
    }
    System.out.println("");
  }
  public static void markOriginalPosition(int [][]arrivalsAndBurst, int pos) {
    for (int i = 0; i<arrivalsAndBurst.length; i++) {
        arrivalsAndBurst[i][pos] = i+1;
    }
}
     public static void takeOneJob(int [][]arrivalsAndBurst, int tq, int looptimes) {
         for (int i = 0; i < arrivalsAndBurst.length; i++) {
             if (arrivalsAndBurst[i][1] != 0) {
                 if (arrivalsAndBurst[i][1] >= tq) {
                    arrivalsAndBurst[i][0] += (tq * looptimes);
                    arrivalsAndBurst[i][1] -= tq;
                 } else {
                     arrivalsAndBurst[i][0] += arrivalsAndBurst[i][1];
                     arrivalsAndBurst[i][1] = 0;
                 }
                 globalProcessedArray[globalCounter] = arrivalsAndBurst[i][2];
                 System.out.println("PPPPP" + arrivalsAndBurst[i][2]);
                 globalCounter++;
                 break;
                 // PPP 1 3 1 2 4 3 5 1 3 5 
             } 
         }
     }
  public static int takeTheThreeGroupAndProcess(int [][]arrivalsAndBurst, int from, int to, int tq) {
      //int shouldwebreak_local = -1;
      for (int i = 0; i < arrivalsAndBurst.length ; i++) {
            //because they are in order
            if (arrivalsAndBurst[i][0] >= from && arrivalsAndBurst[i][0] < to) {
                if (arrivalsAndBurst[i][1] != 0) {
                    //if (shouldwebreak_local == -1) {shouldwebreak_local = 0;}
                    if (arrivalsAndBurst[i][1] < tq) {
                        totalJobsTime += arrivalsAndBurst[i][1];
                        arrivalsAndBurst[i][1] = 0;
                    } else {
                        totalJobsTime += tq;
                        arrivalsAndBurst[i][1] = arrivalsAndBurst[i][1] - tq;
                        //jobLoopTimes = i+1;
                        arrivalsAndBurst[i][0] = arrivalsAndBurst[i][0] + totalJobsTime;
                        //arrivalsAndBurst[i][0] += tq;
                    }
                    globalProcessedArray[globalCounter] = arrivalsAndBurst[i][2];
                    globalCounter++;
                } else {
                    //job completed -- would normally knock this off the run queue...
                }
                   
            }
      }
      //return 0; //shouldwebreak_local;
      return totalJobsTime;

  }
  public static int captureJobs (String args[], int [][]arrivalsAndBurst) {
      int tq = 0;
      for (int i = 0; i < args.length ; i++) {
          String [] parts1 = args[i].split(":");
          System.out.println(parts1[0] + " - " + parts1[1]);
          if (parts1[0].equals("TQ") || parts1[0].equals("tq")) {
                   tq = Integer.parseInt(parts1[1]);
                   System.out.println("time quantum is: " + tq);
          }   else {
                  arrivalsAndBurst[i-1][0] = Integer.parseInt(parts1[0]);
                  arrivalsAndBurst[i-1][1] = Integer.parseInt(parts1[1]);
          }
          if (i == 1) {        
            System.out.println("first job arrival time: " + parts1[0]);    
          }
          //1. first capture in array
          //2. arrange by arrival time...      
        }
    return tq;
  }
   public static void printOutIntArray(int arr[][]) {
          System.out.println("hey...");  
            for (int i = 0; i < arr.length; i++) {
                for (int j = 0; j < arr[i].length; j++ ) {
                   System.out.print(arr[i][j] + " ");  
                } // end of for
                System.out.println();                          
            } // end of for
    }
    public static void printOutIntArrayFlat(int arr[][]) {
          //System.out.println("hey flat...");  
            for (int i = 0; i < arr.length; i++) {
                //for (int j = 0; j < arr[i].length; j++ ) {
                   System.out.print(arr[i][0] + ":" + arr[i][1]);  
                //} // end of for
                System.out.print(" ");                              
            } // end of for
            System.out.println();
    }
   
  public static void columnWiseSorting(int arr[][], int column) {
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

//(2) Round Robin(RR) CPU Scheduling Algorithm in OS with example - YouTube
// is https://www.youtube.com/watch?v=-jFGYDfWkXI&t=837s
