using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    
    public Text dialogueText;
    public Image currentSpeaker;
    public Text nameText;
    public Button ContinueButton;

    public Animator animator;
    public float typingSpeed = 0.05f;
    public float timeBetweenSentences = 3f;

    private Queue<Sentence> sentences;

    private Dialogue dialogue;

    private int currentScene;
    private bool _momDialogueHappened = false;
    private bool _cashierDialogueHappened = false;
    [HideInInspector] public bool _secretEndingHappened = false;
    public bool _managerDialogueHappened = false;
    private GameObject _thanksScreen;
    private GameObject _rightArrow;

    public AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<Sentence>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == 1 || currentScene == 2)
        {
            _thanksScreen = GameObject.FindGameObjectWithTag("Thanks");
            _thanksScreen.SetActive(false);
        }
        if (currentScene == 1)
        {
            _rightArrow = FindObjectOfType<BlinkingArrow>().gameObject;
            _rightArrow.SetActive(false);
        }
    }

    public void StartDialogue(Dialogue d)
    {

        dialogue = d;
        // pause the game while dialogue is on screen
        if (dialogue.isPaused)
        {
            //Time.timeScale = 0;
            // make the player not move? timescale doesn't work because it doesn't let the dialogue box move onto the screen lol
        }
        

        animator.SetBool("IsOpen", true);

        sentences.Clear();

        foreach (Sentence s in dialogue.sentences)
        {
            Debug.Log("sentence: " + s.sentence);
            sentences.Enqueue(s);
        }

        
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        Sentence s = sentences.Peek();
        string sentence = s.sentence;

        nameText.text = s.whoIsSpeaking;
        currentSpeaker.sprite = s.SpeakerImage;

        
        sentences.Dequeue();
        

        StopAllCoroutines(); //if a user starts a new sentence before the previous one is finished, stop loading the previous sentence
        StartCoroutine(TypeSentence(sentence));

        //dialogueText.text = sentence;
        //Debug.Log(sentence);
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
            audioS.PlayOneShot(audioS.clip);
        }

        



        if (!dialogue.isClick)
        {
            yield return new WaitForSecondsRealtime(timeBetweenSentences);
            DisplayNextSentence();
        }


    }

    public void EndDialogue()
    {
        Debug.Log("End of conversation");
        animator.SetBool("IsOpen", false);
        //Time.timeScale = 1;

        //if the current scene is the opening cutscene, which should be build index 1
        if (currentScene == 1 && !_momDialogueHappened)
        {
            GameObject.FindGameObjectWithTag("Mom").GetComponent<MomMove>().StartMoving();

            Invoke("StartOldLadyRoll", 3);
            _momDialogueHappened = true;
        }
        else if (currentScene == 1 && !_cashierDialogueHappened)
        {
            _cashierDialogueHappened = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().InCutscene = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().InCutScene = false;
            FindObjectOfType<LevelManager>().StartSecretEndingTimer();
            _rightArrow.SetActive(true);
            _rightArrow.GetComponent<BlinkingArrow>().StartBlinking();
        }
        else if (currentScene == 1 && _secretEndingHappened)
        {
            Debug.Log("Load the YOU WIN scene/screen");
            _thanksScreen.SetActive(true);
            Invoke("LoadTitleScreen", 5);
        }

        if (currentScene == 2 && !_managerDialogueHappened) // manager dialogue
        {
            _managerDialogueHappened = true;
        }
        else if (currentScene == 2 && _managerDialogueHappened) // mom dialogue (only after manager)
        {
            Debug.Log("Load the YOU WIN scene/screen");
            _thanksScreen.SetActive(true);
            Invoke("LoadTitleScreen", 4);
        }
    }

    private void StartOldLadyRoll()
    {
        GameObject.FindGameObjectWithTag("OldLady").GetComponent<OldLadyRoll>().StartRolling();
    }

    private void LoadTitleScreen()
    {
        _thanksScreen.SetActive(false);
        SceneManager.LoadScene(0);
    }

    

    
}
