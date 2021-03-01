import java.io.File;
import java.io.IOException;
import java.nio.file.FileAlreadyExistsException;
import java.nio.file.Files;
import java.nio.file.Paths;

/**This class reads a file from the results queue (the queue of files that contains the pattern),
 * and copies it into the specified destination directory.
 */
 public class Copier implements Runnable {
    public static final int COPY_BUFFER_SIZE = 4096; //Size of buffer used for a single file copy process
    private File destination;//Destination directory
    private SynchronizedQueue<File> resultsQueue; //Queue of files found, to be copied

    /** Constructor. Initializes the worker with a destination directory and a queue of files to copy. */
    public Copier(File destination, SynchronizedQueue<File> resultsQueue){
        this.destination = destination;
        this.resultsQueue = resultsQueue;
    }

    /**Runs the copier thread. Thread will fetch files from queue and copy them, one after each other,
    * to the destination directory. When the queue has no more files, the thread finishes.
    */
    public void run(){
        File current = resultsQueue.dequeue();
        while(current != null) {
            try {
                Files.copy(current.toPath(), Paths.get(destination.toPath().toString(), current.getName()));
            } catch (FileAlreadyExistsException ex) {
                // Not an error, ignore
            } catch(IOException ex){
                System.out.println("an Error happened while copying");
            }
            current = resultsQueue.dequeue();
        }


    }
}
