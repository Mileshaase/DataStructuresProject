using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Data : MonoBehaviour
{
    public Dictionary<string, List<string>> data = new(); //title is the key and then the list of strings stores the year, the runtime, the genres and the avg rating
    KeyValuePair<string, List<string>>[] heap; // max heap

    public void Start()
    {
        // Load the file from the Resources folder
        TextAsset file = Resources.Load<TextAsset>("Data/DataSet");

        // Split the text into lines
        string[] lines = file.text.Split('\n');

        // Process each line
        for (int i = 1; i < lines.Length; i++) // Start from index 1 to skip the first line
        {
            string line = lines[i];
            
            // Skip empty lines
            if (string.IsNullOrEmpty(line))
                continue;

            // Split the line by tabs
            string[] values = line.Split('\t');

            // Extract data
            string primaryTitle = values[2]; // Assuming primaryTitle is at index 2
            List<string> strings = new List<string>()
            {
                values[4], // Year
                values[6], // Runtime
                values[7], // Genres
                values[8]  // Average Rating
            };

            // Check if the key already exists in the dictionary
            if (!data.ContainsKey(primaryTitle))
            {
                // If it doesn't exist, add it to the dictionary
                data.Add(primaryTitle, strings);
            }
        }

        // // Debug and print the values in data with their keys
        // foreach (KeyValuePair<string, List<string>> entry in data)
        // {
        //     string key = entry.Key;
        //     List<string> values = entry.Value;
            
        //     Debug.Log("Key: " + key);
        //     Debug.Log("Values: " + string.Join(", ", values.ToArray()));
        // }
    }

    public void SortData(List<string> Genres, int Year, int SearchType)
    {
        // need to implement timers

        QuickSort();

        float heapStartTime = Time.realtimeSinceStartup;

        MaxHeap(Genres, Year, SearchType);

        float maxHeapTime = Time.realtimeSinceStartup - heapStartTime;

        Debug.Log("Max Heap Time: " + maxHeapTime);
    }

    private void QuickSort()
    {

    }
    private void MaxHeap(List<string> Genres, int Year, int sortMethod)
    {
        Debug.Log("Making Max Heap...");

        // make new list of movies filtered by criteria

        List<KeyValuePair<string, List<string>>> filteredMovies = new List<KeyValuePair<string, List<string>>>();
        
        // iterate through movie dataset to filter

        foreach(var movie in data)
        {
            int movieYear = 0;

            if (int.TryParse(movie.Value[0], out movieYear)) // make sure there are no errors for movies with "/N" as their year
            {
                if(Genres.Contains(movie.Value[2]) && movieYear <= Year && movieYear >= Year - 10)
                    {
                        // add movie to list if it fits criteria

                        filteredMovies.Add(movie);
                    }
            }            
        }

        // convert list into an array

        heap = filteredMovies.ToArray();

        // make the heap

        MakeHeap(sortMethod);

        for(int i = 0; i < 5; i++)
        {
            Debug.Log(heap[i].Key);
        }
    }

    private void MakeHeap(int sortMethod)
    {
        // heapify the nodes to make the max heap

        for (int i = heap.Length / 2 - 1; i >= 0; i--)
        {
            heapify(i, sortMethod);
        }
    }

    private void heapify(int i, int sortMethod)
    {
        // initializing the left and right children of each node

        int left = 2 * i + 1;
        int right = 2 * i + 2;
        int largest = i;

        // determine values to be compared using the sortMethod
        int valueToCompare = 0;

        if(sortMethod == 0)
        {
            valueToCompare = 3;
        }
        else
        {
            valueToCompare = 1;
        }

        // if the right node is in bounds and larger than the current largest node, right node becomes the new largest

        if (right < heap.Length && float.Parse(heap[right].Value[valueToCompare]) > float.Parse(heap[largest].Value[valueToCompare]))
        {
            largest = right;
        }

        // or if the left node is in bounds and larger than the current largest node, left is new largest

        if (left < heap.Length && float.Parse(heap[left].Value[valueToCompare]) > float.Parse(heap[largest].Value[valueToCompare]))
        {
            largest = left;
        }

        if (largest != i)
        {
            // if the root isn't largest anymore, swap them and heapify

            KeyValuePair<string, List<string>> temp = heap[i];
            heap[i] = heap[largest];
            heap[largest] = temp;

            heapify(largest, sortMethod);
        }
    }
}