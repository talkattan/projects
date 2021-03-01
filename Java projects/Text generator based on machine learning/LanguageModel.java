/*
Assignment number : 9
File Name : languagemodel
Name : TAL KATTAN
Student ID : 207541673
Email : tal.kattan@post.idc.ac.il
*/
package languagemodel;

import std.StdIn;

import java.security.InvalidParameterException;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.Random;
import java.util.LinkedList;
import std.StdIn;

public class LanguageModel {

	// The length of the moving window
	private int windowLength; 
	// The map where we manage the (window, LinkedList) mappings 
	private HashMap<String, LinkedList<CharProb>> probabilities;
	
	// Random number generator:
	// Used by the getRandomChar method, initialized by the class constructors. 
	Random randomGenerator;
	
	/**
	 * Creates a new language model, using the given window length
	 * and a given (fixed) number generator seed.
	 * @param windowLength
	 * @param seed
	 */
	public LanguageModel(int windowLength, int seed) {
		this.randomGenerator = new Random(seed);
		this.windowLength = windowLength;
		probabilities = new HashMap<String, LinkedList<CharProb>>();
	}

	/**
	 * Creates a new language model, using the given window length.
	 * @param windowLength
	 */
	public LanguageModel(int windowLength) {
		this.windowLength = windowLength;
		probabilities = new HashMap<String, LinkedList<CharProb>>();
	}

	/**
	 * Builds a language model from the text in standard input (the corpus).
	 */
	public void train() {
	    String window = "";
		char c;
		for(int i = 0; i < windowLength; i++)
		{
			window += StdIn.readChar();			
		}
		while(!StdIn.isEmpty()) {
			c = StdIn.readChar();
			if(probabilities.get(window) == null) //window = the current string
												  //The loop run if there is no linked list to this string
			{
				probabilities.put(window, new LinkedList<CharProb>()); //Associates the specified value with the specified key in this map
 
			}
			calculateCounts(probabilities.get(window), c); //probabilities.get(window) = prob , it is the linked list that comes after the 
													       //window(the string) (like in the word example)
			window = window.substring(1, window.length());
			window += c; 
		}
		for(LinkedList<CharProb> probs : probabilities.values()) //pass everything from the first value until the last value
		{
			calculateProbabilities(probs); //probs is 1st list and in next loop the 2nd..
		}
	}
		
	// If the given character is found in the given list, increments its count;
    // Otherwise, constructs a new CharProb object and adds it to the given list.
	private void calculateCounts(LinkedList<CharProb> probs, char c) {
		for(int i = 0; i < probs.size(); i++)
		{
			if(probs.get(i).chr == c)
			{
				probs.get(i).count++;
				return;
			}
		}
		probs.add(new CharProb(c));
	}
	
	// Calculates and sets the probabilities (p and cp fields) of all the
	// characters in the given list.
	private void calculateProbabilities(LinkedList<CharProb> probs) {				
		int totalCount = 0;
		for(int j = 0; j < probs.size(); j++) {
			totalCount += probs.get(j).count;
		}
		for(int i = 0; i < probs.size(); i++) {
			probs.get(i).CharProb(totalCount); //calculate p for every charprob on the list
			                                   //it calls a function that initialize p
		}
		if(probs.size() != 0) {
			probs.getFirst().cp = probs.getFirst().p; //the cp of the first node
													//equals to the p for the first node
		}
		for(int i = 1; i < probs.size(); i++) {
			probs.get(i).cp = probs.get(i - 1).cp + probs.get(i).p;  //the cp of the current node
			//equals to the cp of the previous node + the p of the current node
		}		
	}	

	/**
	 * Returns a string representing the probabilities map.
	 */
	public String toString() {
		StringBuilder str = new StringBuilder();
		for (String key : probabilities.keySet()) { //keySet() Returns a Set view of the keys contained in this map
			LinkedList<CharProb> keyProbs = probabilities.get(key); // for every key, keyProbs equals to the value the key points to
																	//get the charprobs of the current key
			str.append(key); //because its string builder we cant do +=
			str.append(" : (");
			for(int i = 0; i < keyProbs.size(); i++) // go over the charprobs that the key points to
			{
				str.append(keyProbs.get(i).toString()); // for example in-> hi: ((n 2 0.5 0.5)(m 2 0.5 1.0)) 
														//it the 1st loop get(i) prints (n 2 0.5 0.5) which is the first charprob in the linked list
														// and in the 2nd (m 2 0.5 1.0) which is the 2nd charprob in the linked list 
			}
			str.append(")");
			str.append("\n");
		}
		return str.toString();
	}	
	/**
	 * Generates a random text, based on the probabilities that were learned during training. 
	 * @param initialText - text to start. 
	 * @param textLength - the size of text to generate
	 * @return the generated text
	 */
	public String generate(String initialText, int textLength) {
		String s = initialText;
		for(int i = 0; i < textLength; i++)
		{
			if(probabilities.get(s.substring(s.length() - this.windowLength, s.length())) == null) // tries to get linked list of the current window
					return s; //if the window doesnt appear in the text
			s += getRandomChar(probabilities.get(s.substring(s.length() - this.windowLength, s.length()))); //otherwise
		}
		return s;
	}

	// Returns a random character from the given probabilities list.
	public char getRandomChar(LinkedList<CharProb> probs) {
		double c = randomGenerator.nextDouble();
		for(int i = 0; i < probs.size(); i++) {
			if(probs.get(i).cp >= c)
				return probs.get(i).chr;
		}
		throw new InvalidParameterException(); //if the list is empty		
	}
	
	// Learns the text that comes from standard input,
	// using the window length given in args[0],
	// and prints the resulting map. 
	public static void main(String[] args) {
		int windowLength = Integer.parseInt(args[0]);  // window length
		String initialText = args[1];			      // initial text //THE WORD TO START WITH
		int textLength = Integer.parseInt(args[2]);	  // size of generated text
		boolean random = (args[3].equals("random") ? true : false);  // random / fixed seed
		// if the word is random its without seed , otherwise with seed
		LanguageModel lm;

		// Creates a language model with the given window length and random/fixed seed
		if (random) {
			// the generate method will use a random seed
			lm = new LanguageModel(windowLength);      
		} else {
			// the generate method will use a fixed seed = 20 (for testing purposes)
			lm = new LanguageModel(windowLength, 20); 
		}
		
		// Trains the model, creating the map.
		lm.train();
		
		// Generates text, and prints it.
		System.out.println(lm.generate(initialText,textLength));
	}
}
