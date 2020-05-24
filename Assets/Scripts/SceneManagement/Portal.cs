using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour

        
    {
        
     
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = .5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
                
            }
          
        }
         private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not present");
                yield break;
            }
            Fader fader = FindObjectOfType<Fader>();
            DontDestroyOnLoad(gameObject);

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            yield return fader.FadeOut(fadeOutTime);

            //Save current level
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();
           
            Portal otherPortal = GetOtherPortal();

            //Load current level

            wrapper.Load();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            //player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
           player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
           foreach (Portal portal in FindObjectsOfType<Portal>()){
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }
            return null;

        }
    }

}