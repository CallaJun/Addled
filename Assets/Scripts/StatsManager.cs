using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

// Add to start scene
public class StatsManager : MonoBehaviour
{
    public static StatsManager instance = null;
    public Stats state;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        Load();
    }

    public void Load()
    {
        // Check if there is a save state
        if (PlayerPrefs.HasKey("stats"))
        {
            state = Deserialize(PlayerPrefs.GetString("stats"));
        }
        else
        {
            state = new Stats();
            Save();
        }
    }

    // Save the state of this save state to the playerprefs
    public void Save()
    {
        PlayerPrefs.SetString("stats", Serialize(state));
    }

    public void addScore(int score)
    {
        // Update average score
        state.numPlays++;
        state.scoresSum += score;

        // Check high score
        if (state.highScore < score)
        {
            state.highScore = score;
        }
        Save();

    }

    public float averageScore()
    {
        return Mathf.Round(((float) state.scoresSum / state.numPlays) * 100f) / 100f;
    }

    public string Serialize(Stats toSerialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(Stats));
        StringWriter writer = new StringWriter();
        xml.Serialize(writer, toSerialize);
        return writer.ToString();
    }

    public Stats Deserialize(string toDeserialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(Stats));
        StringReader reader = new StringReader(toDeserialize);
        return (Stats)xml.Deserialize(reader);
    }
}
