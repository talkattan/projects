import java.io.File;

/**Main application class. This application searches for all files under some given path
 * that contain a given textual pattern. All files found are copied to some specific directory.
 */
public class DiskSearcher {
    //Capacity of the queue that holds the directories to be searched
    public static final int DIRECTORY_QUEUE_CAPACITY = 50;

    //Capacity of the queue that holds the files found
    public static final int RESULTS_QUEUE_CAPACITY = 50;

    public static void main(java.lang.String[] args){
        // java DiskSearcher <filename-pattern> <root directory> <destination directory>
        // <# of searchers> <# of copiers>
        String fileNamePattern = args[0];
        String rootDirectory = args[1];
        String destinationDirectory = args[2];
        int numOfSearchers = Integer.parseInt(args[3]);
        int numOfCopiers = Integer.parseInt(args[4]);

        SynchronizedQueue<File> directoryQueue = new SynchronizedQueue<File>(DIRECTORY_QUEUE_CAPACITY);
        SynchronizedQueue<File> resultsQueue = new SynchronizedQueue<File>(RESULTS_QUEUE_CAPACITY);

        Thread scouterThread = new Thread(new Scouter(directoryQueue, new File (rootDirectory)));
        scouterThread.start();

        Thread[] allThreads = new Thread[numOfCopiers + numOfSearchers + 1];//+1 cus the scouterThread
        int currentIndex = 0;
        for (int i =0 ; i< numOfSearchers; i++){
            Thread searcherThread = new Thread(new Searcher(fileNamePattern, directoryQueue, resultsQueue));
            allThreads[currentIndex++] = searcherThread;//save it so we could wait 4 it to finish
            searcherThread.start();
        }

        for (int i = 0; i< numOfCopiers; i++){
            Thread copierThread = new Thread(new Copier(new File (destinationDirectory), resultsQueue));
            allThreads[currentIndex++] = copierThread;
            copierThread.start();
        }
        allThreads[currentIndex] = scouterThread;

        for (int i = 0; i < allThreads.length; i++){
            try {
                allThreads[i].join();
            }catch (InterruptedException ex){
                System.out.print("there was error to wait the thread to finish");
            }
        }
    }

}
