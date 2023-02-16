using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Team11.Interactions;

public class DoorAudio : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private string openSoundPath;
    [SerializeField] private string closeSoundPath;
    private FMOD.Studio.EventInstance doorSound;

    private void Update()
    {
        
        
        if (_door.openState == false)
        {
            //RuntimeManager.PlayOneShot(openSoundPath);
            doorSound = RuntimeManager.CreateInstance(openSoundPath);
            doorSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            doorSound.start();
            doorSound.release();
        }

        else if (_door.openState)
        {
            //RuntimeManager.PlayOneShot(closeSoundPath);
            doorSound = RuntimeManager.CreateInstance(closeSoundPath);
            doorSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            doorSound.start();
            doorSound.release();
        }

        
    }
}

