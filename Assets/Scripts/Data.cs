using UnityEngine;
using System.Collections.Generic;

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
        if(SearchType == 0)
        {
            QuickSort();
        }
        else
        {
            MaxHeap(Genres, Year);
        }
    }

    private void QuickSort()
    {

    }
    private void MaxHeap(List<string> Genres, int Year)
    {
        foreach (var movie in data)
        {

        }
    }
}