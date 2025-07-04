using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreService : MonoBehaviour
{
    public int TotalMatches { get; private set; }
    public TextMeshProUGUI scoreTxt;
    public void RegisterMatch()
    {
        TotalMatches++;
        scoreTxt.text = $"Total Matches: {TotalMatches}";
        
    }
}
