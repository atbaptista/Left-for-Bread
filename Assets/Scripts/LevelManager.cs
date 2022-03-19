using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentScene;
    public float SecretEndingTimer = 5f;
    private float currentTime = 0.0f;
    private bool _secretEndingStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (!_secretEndingStarted) return;
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime >= SecretEndingTimer)
            {
                //Mom comes back scene
                Debug.Log("Secret ending happened! bread has been getted");
                FindObjectOfType<MomMove>().StartMovingBack();
                FindObjectOfType<DialogueManager>()._secretEndingHappened = true;
                _secretEndingStarted = false;
            }
        }
        
    }

    public void LoadNextSceneAfterDelay(int delay)
    {
        Invoke("LoadNextScene", delay);
    }

    public void LoadNextScene()
    {
        currentScene++;
        SceneManager.LoadScene(currentScene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadNextScene();
        }
        
    }

    public void StartSecretEndingTimer()
    {
        currentTime = 0.0f;
        _secretEndingStarted = true;
    }
}
