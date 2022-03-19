using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    
    public bool isPaused;
    public bool isClick;

    //[TextArea(3, 10)]
    //public string[] sentences;
    public Sentence[] sentences;
}

[System.Serializable]
public class Sentence
{
    public string whoIsSpeaking;
    public Sprite SpeakerImage;

    [Header("Dialogue text")]
    [TextArea(3, 10)]
    public string sentence;
}
