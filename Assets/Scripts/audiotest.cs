using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiotest : MonoBehaviour
{
    public AudioSource AudioS;
    public AudioClip clickSound;


    public void PlaySoundOnClick()
    {
        AudioS.PlayOneShot(clickSound);
    }
}
