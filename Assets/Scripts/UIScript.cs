using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIScript : MonoBehaviour
{
    public Data dataScript;
    public List<string> genres = new();
    public int year = 0;
    public int sortType = 0; // 0 is rating, 1 is runtime
    public int sortMethod = 1;

    public void AddGenre(Toggle toggle)
    {
        if(toggle.isOn)
        {
            genres.Add(toggle.name);
        }
        else
        {
            foreach(string i in genres)
            {
                if(i == toggle.name)
                {
                    genres.Remove(toggle.name);
                }
            }
        }
    }

    public void SelectYear(TMP_Dropdown years)
    {
        if(years.value == 0)
        {   
            year = 1950;
        }
        else if(years.value == 1)
        {
            year = 1960;
        }
        else if(years.value == 2)
        {
            year = 1970;
        }
        else if (years.value == 3)
        {
            year = 1980;
        }
        else if (years.value == 4)
        {
            year = 1990;
        }
        else if (years.value == 5)
        {
            year = 2000;
        }
        else if (years.value == 6)
        {
            year = 2010;
        }
        else if (years.value == 7)
        {
            year = 2020;
        }
    }

    public void ChooseSortType(TMP_Dropdown selection)
    {
        if(selection.value == 0)
        {   
            sortType = 0;
        }
        else if(selection.value == 1)
        {
            sortType = 1;
        }
    }

    public void QuickSort()
    {
        sortMethod = 0;
    }
    public void MaxHeap()
    {
        sortMethod = 1;
    }

    public void SubmitSearch()
    {
        if(genres.Count() != 0 )
        {
            dataScript.SortData(genres, year, sortType, sortMethod);
        }
        else
        {
            Debug.Log("something not selected");
        }
    }

    public void MakeAnotherSearch()
    {
        SceneManager.LoadScene("MovieRecommender");
    }
}
