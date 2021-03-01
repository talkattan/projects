import java.io.File;
/**This class reads a directory from the directory queue and lists all files in this directory.
 * Then, it checks each file to see if the file name contains the pattern given. Files that contain
 * the pattern are enqueued to the results queue (to be copied).
 */
public class Searcher implements Runnable{//runnable means we can run on thread

    private java.lang.String pattern;//Pattern to look for

    //A queue with directories to search in (as listed by the scouter)
    private SynchronizedQueue<File> directoryQueue;

    //A queue for files found (to be copied by a copier)
    private SynchronizedQueue<File> resultsQueue;

    //Constructor. Initializes the searcher thread.
    public Searcher(java.lang.String pattern,
                    SynchronizedQueue<File> directoryQueue,
                    SynchronizedQueue<File> resultsQueue){
        this.pattern = pattern;
        this.directoryQueue = directoryQueue;
        this.resultsQueue = resultsQueue;
    }

    /**Runs the searcher thread. Thread will fetch a directory to search in from the directory queue,
     * then search all files inside it (but will not recursively search subdirectories!).
     * Files that are found to contain the pattern are enqueued to the results queue.
     * This method begins by registering to the results queue as a producer and when finishes,
     * it unregisters from it.
     */
    public void run(){
        resultsQueue.registerProducer();
        File current = directoryQueue.dequeue();
        while(current != null){
            File[] files = current.listFiles();
            for(File file : files){
                if(file.isFile()){
                    if (file.getName().contains(pattern)) {
                        resultsQueue.enqueue(file);
                    }
                }
            }
            current = directoryQueue.dequeue();
        }
        resultsQueue.unregisterProducer();
    }
}
