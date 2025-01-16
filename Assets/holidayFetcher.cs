using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class HolidayFetcher : MonoBehaviour
{
    public TMP_Text holidayText;

   
    private string apiUrl = "https://calendarific.com/api/v2/holidays";
    private string apiKey = "LABAAIDsHRMxa0zvoMdP96XcXt5rghNF"; 
    private string countryCode = "CA"; 
    private string provinceCode = "QC"; 
    private int year = 2025;


    public void GetHolidays()
    {
        StartCoroutine(FetchHolidays()); // Si vous voulez aussi que la méthode lance la Coroutine
    }


    private IEnumerator FetchHolidays()
    {
        string url = "https://calendarific.com/api/v2/holidays?api_key=LABAAIDsHRMxa0zvoMdP96XcXt5rghNF&country=CA&year=2025";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            
            string jsonResponse = www.downloadHandler.text;
            ProcessHolidays(jsonResponse);
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des jours fériés : " + www.error);
        }
    }

    
    private void ProcessHolidays(string jsonResponse)
    {
        
        var holidays = JsonUtility.FromJson<HolidayList>(jsonResponse);

        string holidayInfo = "Jours fériés au Québec:\n";
        foreach (var holiday in holidays.response.holidays)
        {
            holidayInfo += holiday.name + " : " + holiday.date.iso + "\n";
        }

        
        holidayText.text = holidayInfo;
    }
}

[System.Serializable]
public class HolidayList
{
    public Response response;
}

[System.Serializable]
public class Response
{
    public Holiday[] holidays;
}

[System.Serializable]
public class Holiday
{
    public string name;
    public DateInfo date;
}

[System.Serializable]
public class DateInfo
{
    public string iso;
}