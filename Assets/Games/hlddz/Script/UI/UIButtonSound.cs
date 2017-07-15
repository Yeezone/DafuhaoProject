using UnityEngine;
using System.Collections;

public class UIButtonSound : MonoBehaviour {
    public AudioClip enterButtonSound;
    public AudioClip outButtonSound;

    void OnHover(bool isOver) 
    {
         float fvol=NGUITools.soundVolume;
        if (isOver)
        {
           
            NGUITools.PlaySound(enterButtonSound, fvol, 1f);
        }
        else 
        {
            NGUITools.PlaySound(outButtonSound, fvol, 1f);
        }
    }   
}
