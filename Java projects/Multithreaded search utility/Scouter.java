import java.io.File;

/**
* lists all sub-directories from a given root path. Each sub-directory
* is enqueued to be searched for files by Searcher threads.
**/
public class Scouter implements java.lang.Runnable{

    private SynchronizedQueue<File> directoryQueue; // A queue for directories to be searched
    private File root; //Root directory to start from

    /**
     * Constructor. Initializes the counter with a queue for the directories to be
     * searched and a root directory to start from
     **/
    Scouter(SynchronizedQueue<File> directoryQueue, File root){
        this.directoryQueue = directoryQueue;
        this.root = root;
    }

   /** Starts the scouter thread. Lists directories under root directory and adds
    * them to queue, then lists directories in the next level and enqueues them and so on.
    * This method begins by registering to the directory queue as a producer and when finishes,
    * it unregisters from it.
    **/
    public synchronized void run(){//only one scouter thread in the system as required
        directoryQueue.registerProducer();
        directoryQueue.enqueue(root);
        AddDirectoryToQueue(root);
        directoryQueue.unregisterProducer();
    }

    private void AddDirectoryToQueue(File directory){
        File[] files = directory.listFiles();

        for(File file : files)
        {
            if (file.isDirectory())
            {
                AddDirectoryToQueue(file);
                directoryQueue.enqueue(file);
            }
        }
    }

}
