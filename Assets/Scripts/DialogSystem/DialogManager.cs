using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Dialog currentDialog;

    public bool startOnAwake = false;

    GameObject gameManager;

    private void Start()
    {
        gameManager = GameManager.instance.gameObject;
        NextMessage();
    }


    float lettersAppearSpeedProSec = 2;
    int lastSchownLetterIndex = 0, currentMessageLenght = 0;
    int currentMessageIndex = -1;
    string nextMessage;

    float t;

    public static DialogManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        if (!startOnAwake)
            gameObject.SetActive(false);
    }

    private void Update()
    {
        if(gameManager.activeSelf)
        {
            t += Time.deltaTime;
            int appearedLettersCount = (int) (t * lettersAppearSpeedProSec);
            if(appearedLettersCount > lastSchownLetterIndex && currentMessageLenght > appearedLettersCount)
            {
                lastSchownLetterIndex = appearedLettersCount;
                UpdateMessage();
            }

            if (Input.GetKeyDown(KeyCode.Return))
                NextMessage();
        }
    }

    public void NextMessage()
    {
        if (lastSchownLetterIndex < currentMessageLenght - 1)
        {
            lastSchownLetterIndex = currentMessageLenght - 1;
        }
        else
        {
            if(currentMessageIndex == currentDialog.messages.Length - 1)
            {
                gameObject.SetActive(false);
                return;
            }

            lastSchownLetterIndex = -1;
            nextMessage = currentDialog.messages[++currentMessageIndex];
            currentMessageLenght = nextMessage.Length;
            t = 0;
        }
        UpdateMessage();
    }

    void UpdateMessage()
    {
        text.text = nextMessage.Substring(0, lastSchownLetterIndex + 1);
    }


    //To Do (Ivan): make better initialisation
    public void ShowDialog(Dialog dialog)
    {
        
        currentDialog = dialog;
        lettersAppearSpeedProSec = dialog.speed;
        lastSchownLetterIndex = 0; currentMessageLenght = 0;
        currentMessageIndex = -1;
        t = 0;
        gameObject.SetActive(true);
        NextMessage();
    }
}
