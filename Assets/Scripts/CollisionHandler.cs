using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
     [SerializeField] float respawnTime = 1f;
     [SerializeField] AudioClip crashSound;
     [SerializeField] AudioClip completeSound;

     [SerializeField] ParticleSystem crashParticles;
     [SerializeField] ParticleSystem completeParticles;

     AudioSource audioSource;

     bool isTransitioning = false;

    void OnCollisionEnter(Collision other) 
    {
        if(isTransitioning){
        return;
        }
        switch(other.gameObject.tag)
        {
        case "Friendly":
            Debug.Log("Collided with a friendly object");
            break;
        case "Finish":
            StartSuccessSequence();
            break;
        default:
            StartCrashSequence();
        break;
        }
    }

    void StartSuccessSequence()
    {
        //activates the code
        audioSource.Stop();
        audioSource.PlayOneShot(completeSound);
        isTransitioning = true;
        completeParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", respawnTime);
    }

    void StartCrashSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        isTransitioning = true;
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", respawnTime);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = (SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(currentSceneIndex);
    }

     void LoadNextLevel()
    {
        int currentSceneIndex = (SceneManager.GetActiveScene().buildIndex);
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
