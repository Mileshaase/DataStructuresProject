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
        float quickSortStartTime = Time.realtimeSinceStartup;

        QuickSort(Genres, Year, SearchType);

        float quickSortTime = Time.realtimeSinceStartup - quickSortStartTime;

        Debug.Log("Quick Sort Time: " + quickSortTime);
        


        float heapStartTime = Time.realtimeSinceStartup;

        MaxHeap(Genres, Year, SearchType);

        float maxHeapTime = Time.realtimeSinceStartup - heapStartTime;

        Debug.Log("Max Heap Time: " + maxHeapTime);
    }

    private void QuickSort(List<string> Genres, int Year, int SortType)
    {
        Debug.Log("Starting Quicksort...");

        // make new list of movies filtered by criteria

        List<KeyValuePair<string, List<string>>> quickSortfilteredMovies = new List<KeyValuePair<string, List<string>>>();

        // iterate through movie dataset to filter

        foreach (var movie in data)
        {
            int movieYear = 0;

            if (int.TryParse(movie.Value[0], out movieYear)) // make sure there are no errors for movies with "/N" as their year
            {
                if (Genres.Contains(movie.Value[2]) && movieYear <= Year && movieYear >= Year - 10 && movie.Value[3] != @"\N" && movie.Value[1] != @"\N")
                {
                    // add movie to list if it fits criteria

                    quickSortfilteredMovies.Add(movie);
                }
            }
        }

        // do quick sort stuff with filteredMovies
        if (SortType == 0)
        {
            QuickSortByRuntime(quickSortfilteredMovies, 0, quickSortfilteredMovies.Count - 1);
        }
        else if (SortType == 1)
        {
            QuickSortByRating(quickSortfilteredMovies, 0, quickSortfilteredMovies.Count - 1);
        }
        // Sort the movies in descending order by rating
        List<KeyValuePair<string, List<string>>> sortedMovies = quickSortfilteredMovies.OrderByDescending(movie => double.Parse(movie.Value[3])).ToList();

        // Get the top 5 movies
        List<KeyValuePair<string, List<string>>> top5Movies = sortedMovies.Take(5).ToList();

        // Log the top 5 movies
        foreach (var movie in top5Movies)
        {
            Debug.Log($"Title: {movie.Key}, Runtime: {movie.Value[1]}, Rating: {movie.Value[3]}");
        }
    }

    private void QuickSortByRuntime(List<KeyValuePair<string, List<string>>> movies, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = PartitionByRuntime(movies, low, high);
            QuickSortByRuntime(movies, low, pivotIndex - 1);
            QuickSortByRuntime(movies, pivotIndex + 1, high);
        }
    }

    private int PartitionByRuntime(List<KeyValuePair<string, List<string>>> movies, int low, int high)
    {
        int pivot = int.Parse(movies[high].Value[1]);
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (int.Parse(movies[j].Value[1]) < pivot)
            {
                i++;
                KeyValuePair<string, List<string>> temp = movies[i];
                movies[i] = movies[j];
                movies[j] = temp;
            }
        }

        KeyValuePair<string, List<string>> temp1 = movies[i + 1];
        movies[i + 1] = movies[high];
        movies[high] = temp1;

        return i + 1;
    }

    private void QuickSortByRating(List<KeyValuePair<string, List<string>>> movies, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = PartitionByRating(movies, low, high);
            QuickSortByRating(movies, low, pivotIndex - 1);
            QuickSortByRating(movies, pivotIndex + 1, high);
        }
    }

    private int PartitionByRating(List<KeyValuePair<string, List<string>>> movies, int low, int high)
    {
        float pivot = float.Parse(movies[high].Value[3]);
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (float.Parse(movies[j].Value[3]) < pivot)
            {
                i++;
                KeyValuePair<string, List<string>> temp = movies[i];
                movies[i] = movies[j];
                movies[j] = temp;
            }
        }

        KeyValuePair<string, List<string>> temp1 = movies[i + 1];
        movies[i + 1] = movies[high];
        movies[high] = temp1;

        return i + 1;
    }
    
    private void MaxHeap(List<string> Genres, int Year, int sortMethod)
    {
        Debug.Log("Making Max Heap...");

        // make new list of movies filtered by criteria

        List<KeyValuePair<string, List<string>>> filteredMovies = new List<KeyValuePair<string, List<string>>>();
        
        // iterate through movie dataset to filter

        foreach (var movie in data)
        {
            int movieYear = 0;

            if (int.TryParse(movie.Value[0], out movieYear)) // make sure there are no errors for movies with "/N" as their year
            {
                if (Genres.Contains(movie.Value[2]) && movieYear <= Year && movieYear >= Year - 10  && movie.Value[3] != @"\N" && movie.Value[1] != @"\N")
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

        List<KeyValuePair<string, List<string>>> fiveLargest = kthLargest(sortMethod);
        foreach (var movie in fiveLargest)
        {
            Debug.Log($"Title: {movie.Key}, Runtime: {movie.Value[1]}, Rating: {movie.Value[3]}");
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
            valueToCompare = 1;
        }
        else
        {
            valueToCompare = 3;
        }

        int runtime = 0;

        // if the movie doesn't have a runtime, set it to 0

        /*if (largest < heap.Length && !int.TryParse(heap[largest].Value[valueToCompare], out runtime))
        {
            heap[largest].Value[valueToCompare] = "0";
        }
        if (right < heap.Length && !int.TryParse(heap[right].Value[valueToCompare], out runtime))
        {
            heap[right].Value[valueToCompare] = "0";
        }
        if (left < heap.Length && !int.TryParse(heap[left].Value[valueToCompare], out runtime))
        {
            heap[left].Value[valueToCompare] = "0";
        }*/

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

    private List<KeyValuePair<string, List<string>>> kthLargest(int sortMethod)
    {
        List<KeyValuePair<string, List<string>>> fiveLargest = new List<KeyValuePair<string, List<string>>>();
        for (int i = 0; i < 5; i++)
        {
            fiveLargest.Add(extractMax(sortMethod));
        }
        return fiveLargest;
    }

    private KeyValuePair<string, List<string>> extractMax(int sortMethod)
    {
        if (heap.Count() != 0)
        {
            // Save the maximum element
            KeyValuePair<string, List<string>> maxItem = heap[0];

            // Replace the root with the last element in the heap
            heap[0] = heap[heap.Length - 1];

            // Reduce the size of the heap
            heap[0] = heap[heap.Length - 1];

            // Heapify the root to maintain the heap property
            heapify(0, sortMethod);

            return maxItem;
        }
        else
        {
            throw new System.Exception("heap empty :(");
        }
    }
}