import java.util.concurrent.Semaphore;

/**
 * A synchronized bounded-size queue for multithreaded producer-consumer applications.
 *
 * @param <T> Type of data items
 */
public class SynchronizedQueue<T> {//A synchronized bounded-size queue for multithreaded producer-consumer applications.

    private T[] buffer;
    private int producers;//how many enter at this time to queue
    // TODO: Add more private members here as necessary
    private int size;
    private Semaphore producersSemaphore;
    private Object lockEnqueue;
    private Object lockDequeue;


    /**
     * Constructor. Allocates a buffer (an array) with the given capacity and
     * resets pointers and counters.
     * @param capacity Buffer capacity
     */
    @SuppressWarnings("unchecked")
    public SynchronizedQueue(int capacity) {
        this.buffer = (T[])(new Object[capacity]);
        this.producers = 0;
        // TODO: Add more logic here as necessary
        size = 0;
        this.producersSemaphore = new Semaphore(1, true);//Creates a Semaphore with 1 number of permits and given fairness.
        lockEnqueue = new Object();
        lockDequeue = new Object();
    }

    /**
     * Dequeues the first item from the queue and returns it.
     * If the queue is empty but producers are still registered to this queue,
     * this method blocks until some item is available.
     * If the queue is empty and no more items are planned to be added to this
     * queue (because no producers are registered), this method returns null.
     *
     * @return The first item, or null if there are no more items
     * @see #registerProducer()
     * @see #unregisterProducer()
     */
    public T dequeue() {//the function can be read only from 1 thread cus it synchronized
        synchronized (lockDequeue){
            while(size == 0) {
                producersSemaphore.acquireUninterruptibly();
                if(producers == 0){
                    producersSemaphore.release();
                    return null;
                }
                producersSemaphore.release();
                try {
                    Thread.sleep(1);
                } catch(InterruptedException ex) {}
            }
            // Don't let other threads enqueue
            synchronized (buffer){
                T first = buffer[0];
                for (int i = 0; i < size - 1; i++){
                    buffer[i] = buffer[i + 1];
                }
                buffer[size - 1] = null;
                size--;
                return first;
            }
        }
    }

        /**
         * Enqueues an item to the end of this queue. If the queue is full, this
         * method blocks until some space becomes available.
         *
         * @param item Item to enqueue
         */
        public void enqueue(T item) {
            synchronized (lockEnqueue){
                while (size == buffer.length){
                    try {
                        Thread.sleep(1);
                    } catch(InterruptedException ex) {}
                }

                // enqueue logic
                synchronized (buffer){
                    buffer[size] = item;
                    size++;
                }
            }
        }

        /**
         * Returns the capacity of this queue
         * @return queue capacity
         */
        public int getCapacity() {
            return this.buffer.length;
        }

        /**
         * Returns the current size of the queue (number of elements in it)
         * @return queue size
         */
        public int getSize() {
            return this.size;
        }

        /**
         * Registers a producer to this queue. This method actually increases the
         * internal producers counter of this queue by 1. This counter is used to
         * determine whether the queue is still active and to avoid blocking of
         * consumer threads that try to dequeue elements from an empty queue, when
         * no producer is expected to add any more items.
         * Every producer of this queue must call this method before starting to
         * enqueue items, and must also call <see>{@link #unregisterProducer()}</see> when
         * finishes to enqueue all items.
         *
         * @see #dequeue()
         * @see #unregisterProducer()
         */
        public void registerProducer() {
            this.producersSemaphore.acquireUninterruptibly();
            this.producers++;
            this.producersSemaphore.release();
        }

        /**
         * Unregisters a producer from this queue. See <see>{@link #registerProducer()}</see>.
         *
         * @see #dequeue()
         * @see #registerProducer()
         */
        public void unregisterProducer() {
            this.producersSemaphore.acquireUninterruptibly();//wait 4 a permit and lock after
            this.producers--;
            this.producersSemaphore.release();//release
        }
    }
