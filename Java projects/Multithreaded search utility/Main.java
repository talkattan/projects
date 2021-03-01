import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;
import java.io.*;


public class Main {

    public static void main(String[] args) {

        List<String> lines = getLinesFromFile();
        System.out.println("Number of lines found: " + lines.size());
        System.out.println("Starting to process");

        long startTimeWithoutThreads = System.currentTimeMillis();
        workWithoutThreads(lines);
        long elapsedTimeWithoutThreads = (System.currentTimeMillis() - startTimeWithoutThreads);
        System.out.println("Execution time: " + elapsedTimeWithoutThreads);


        long startTimeWithThreads = System.currentTimeMillis();
        workWithThreads(lines);
        long elapsedTimeWithThreads = (System.currentTimeMillis() - startTimeWithThreads);
        System.out.println("Execution time: " + elapsedTimeWithThreads);

    }

    private static void workWithThreads(List<String> lines) {
        //Get the number of available cores
        int x = Runtime.getRuntime().availableProcessors();
        int amountOfJob4Each = lines.size() / x;
        Thread[] allTreads = new Thread[x];
        //Assuming X is the number of cores - Partition the data into x data sets
        for (int i = 0; i < x ; i++){
           List<String> lines4currentWorker = lines.subList(i * amountOfJob4Each, i * amountOfJob4Each + amountOfJob4Each);
            //Create X threads that will execute the Worker class
            allTreads[i] = new Thread(new Worker(lines4currentWorker));
            allTreads[i].start();
        }
        //Wait for all threads to finish
        for(int i = 0; i < x; i++){
            try {
                allTreads[i].join(); //wait the thread to finish
            }catch (InterruptedException ex){
            }
        }
    }

    private static void workWithoutThreads(List<String> lines) {
        Worker worker = new Worker(lines);
        worker.run();
    }

    private static List<String> getLinesFromFile()  {
        File file;
        BufferedReader br;
        ArrayList<String> allRows = new ArrayList<String>();//arrayList inherit from List
        //Read the shakespeare file provided from C:\Temp\Shakespeare.txt
        try {
            file = new File("C:\\Temp\\Shakespeare.txt");
            br = new BufferedReader(new FileReader(file));
            //and return an ArrayList<String> that contains each line read from the file.
            String currentRow;
            while ((currentRow = br.readLine()) != null) {
                allRows.add(currentRow);
            }
        }catch (IOException ex) {
            return null;
        }
        return allRows;
    }
}
