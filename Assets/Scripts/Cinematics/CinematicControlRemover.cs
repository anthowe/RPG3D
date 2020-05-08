﻿
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        void DisableControl(PlayableDirector pd)
        {
        
            player.GetComponent<ActionScheduler>().cancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
            
        }
        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}