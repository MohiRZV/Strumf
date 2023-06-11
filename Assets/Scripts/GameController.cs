using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } else {
            GameObject.Find("house").GetComponent<AudioSource>().Play();
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadAudioClips();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public bool isAudioPlaying = true;

    private int applesCollected = 0;
    private int applesLoseCnd = 2;
    private int ripeAppleCollected = 0;
    private int ripeAppleWinCnd = 3;
    private int boatPartsPlaced = 0;
    private int boatPartsWinCnd = 3;
    private int boatPartsLoseCnd = 2;
    private int boatPartsWrong = 0;
    private int junkCollected = 0;
    private int junkWinCnd = 2;

    private int minigame1Correct = 0;
    private int minigame1WinCnd = 1;
    private int minigame1Wrong = 0;
    private bool reachedEnd = false;

    private int minigame2Mistakes = 0;
    private int minigame3Mistakes = 0;

    private bool minigame2Correct = false;
    private bool minigame3Correct = false;

    private int fallingKiwi = 0;
    private int fallingBanana = 0;
    private int fallingWrong = 0;

    private int hintNumber = 0;

    GameObject minigame1;
    GameObject minigame2;
    GameObject minigame3;
    public CSVWriter csvWriter = new CSVWriter();
    [SerializeField] private GameObject resetButton;

    [SerializeField] Text winText;
    [SerializeField] Text instructionText;
    [SerializeField] GameObject player;

    private GameObject hint;
    private PlayerMovement playerMovement;

    private AudioClip[] audioClips;
    public enum Collectible {
        Cherry,
        Strawberry,
        Apple,
        RipeApple,
        Banana,
        Kiwi,
        Wrong,
        Right,
        BoatPartSquare,
        BoatPartCircle,
        BoatPartTriangle,
        BoatPart,
        BoatPartWrong,
        Boat,
        Junk,
        None
    }

    void Start() {
        resetButton.SetActive(false);
        playerMovement = player.GetComponent<PlayerMovement>();
        hint = GameObject.Find("Hint");
        // You can increase the 1 second delay to as much as the narration for the story lasts
        Invoke("advanceToNextCheckpoint",1);
        setInstruction("Prima data va fi nevoie sa culegi merele coapte de sub copac si sa le pui in cos!");
        if (!minigame3Correct) {
            GameObject.Find("BoatShapes").SetActive(false);
        }
    }

    void Reset() {
        applesCollected = 0;
        applesLoseCnd = 2;
        ripeAppleCollected = 0;
        ripeAppleWinCnd = 3;
        boatPartsPlaced = 0;
        boatPartsWinCnd = 3;
        boatPartsLoseCnd = 2;
        boatPartsWrong = 0;
        junkCollected = 0;
        junkWinCnd = 2;

        minigame1Correct = 0;
        minigame1WinCnd = 1;
        minigame1Wrong = 0;
        reachedEnd = false;

        minigame2Mistakes = 0;
        minigame3Mistakes = 0;

        minigame2Correct = false;
        minigame3Correct = false;

        fallingKiwi = 0;
        fallingBanana = 0;
        fallingWrong = 0;

        hintNumber = 0;
        SceneManager.LoadScene("Login");

    }
    

    // logic for working with the collectibles based on what object was taken to the basket
    public void collected(Collectible collectible, GameObject collidedWith) {
        Debug.Log("Collected "+collectible.ToString());
        switch (collectible) {
            case Collectible.Apple:
                StartCoroutine(displayTextForSeconds("Acel mar nu este copt!", 2));
                applesCollected++;
                break;
            case Collectible.RipeApple:
                ripeAppleCollected++;
                Destroy(collidedWith);
                break;
            case Collectible.Cherry:
                minigame1Correct++;
                Destroy(collidedWith);
                break;
            case Collectible.BoatPart:
                boatPartsPlaced++;
                break;
            case Collectible.BoatPartWrong:
                boatPartsWrong++;
                break;
            case Collectible.Junk:
                junkCollected++;
                Destroy(collidedWith);
                break;
            case Collectible.Kiwi:
                fallingKiwi++;
                Destroy(collidedWith);
                break;
            case Collectible.Banana:
                fallingBanana++;
                Destroy(collidedWith);
                break;
            case Collectible.Wrong:
                fallingWrong++;
                break;
            default:
                break;
        }
        checkWinCnd();
    }

    private void checkWinCnd() {
        if (ripeAppleCollected >= ripeAppleWinCnd) {// ripe apple task over
           
            advanceToNextCheckpoint();
            ripeAppleCollected = -1;
            hintNumber++;
            setInstruction("Oh nu! Barca are niste piese lipsa! Pune-le la locul potrivit!");
        }

        if (fallingBanana >= 2 && fallingKiwi >= 3 && minigame1Correct>-1) {
            Debug.Log("minigame 1 win");
            startMinigame2();
            minigame1Correct = -1;
            hintNumber++;
        }

        if (boatPartsPlaced >= boatPartsWinCnd) {//boat parts task over
           
            boatPartsPlaced = -1;
            hintNumber++;
            setBoatPartsAsChild();
            advanceToNextCheckpoint();
            setInstruction("Apa este poluata si nu putem avansa! Strange deseurile si pune-le in cosul de reciclare!");
        }

        if (junkCollected >= junkWinCnd) {
            
            junkCollected = -1;//junk task over
            hintNumber++;
            setInstruction("L-ai gasit pe prietenul tau! Felicitari!");
            player.transform.parent = null;
            player.transform.position = new Vector3(51f, 2f, 9f);
            reachedEnd = true;
        }
        
        if (ripeAppleCollected== -1 && boatPartsPlaced == -1 && junkCollected == -1 && reachedEnd) {// game finished
            Debug.Log("Reached end!");
            resetButton.SetActive(true);
            GameObject.Find("Hint").SetActive(false);
            onFinish();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check the loaded scene to determine which objects to retrieve
        if (scene.name == "Minigames")
        {
            minigame1 = GameObject.Find("Minigame1");
            minigame2 = GameObject.Find("Minigame2");
            minigame3 = GameObject.Find("Minigame3");
            
            minigame2.SetActive(false);
            minigame3.SetActive(false);
            FruitGenerator fruitGenerator = GameObject.Find("FruitGenerator").GetComponent<FruitGenerator>();
            fruitGenerator.canSpawn = true;
            fruitGenerator.despawnHeight = -5f;
        } 
        if (scene.name == "MainScene") {
            player = GameObject.FindGameObjectWithTag("Player");
            playerMovement = player.GetComponent<PlayerMovement>();
            if (minigame3Correct){
                playerMovement.placeToCheckpoint(2);
                AudioSource audioSource = GameObject.Find("Checkpoint2").GetComponent<AudioSource>();
                audioSource.clip = Resources.Load<AudioClip>("Audio/barca_outro");
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }
                hintNumber = 1;
            } else {
                GameObject.Find("house").GetComponent<AudioSource>().Play();
                playerMovement.placeToCheckpoint(0);
                Invoke("advanceToNextCheckpoint",1);
                GameObject.Find("BoatShapes").SetActive(false);
                hintNumber = 0;
            }
            resetButton = GameObject.Find("Reset");
            resetButton.SetActive(false);
            resetButton.GetComponent<Button>().onClick.AddListener(Reset);
            
            
        }
    }

    private void LoadAudioClips() {
        audioClips = new AudioClip[]{
            Resources.Load<AudioClip>("Audio/mere_instr")
        };
    }

    public void startMinigame() {
        Debug.Log("Try start minigame!");
        SceneManager.LoadScene("Minigames");
    }

    public void minigame2Pressed(bool isCorrect) {
        Debug.Log("Pressed "+ isCorrect);
        if (isCorrect) {
            minigame2Correct = true;
            startMinigame3();
        } else {
            minigame2Mistakes++;
        }
    }

    public void minigame3Pressed(bool isCorrect) {
        if (isCorrect) {
            minigame3Correct = true;
            returnToMainScene();
        } else {
            minigame3Mistakes++;
        }
    }

    private void returnToMainScene() {
        SceneManager.LoadScene("MainScene");
    }

    private void startMinigame2() {
        FruitGenerator fruitGenerator =  GameObject.Find("FruitGenerator").GetComponent<FruitGenerator>();
        fruitGenerator.canSpawn = false;
        MinigameAudioController.Instance.startVoice(Resources.Load<AudioClip>("Audio/fb"));
        minigame1.SetActive(false);
        StartCoroutine(WaitBeforeNext());
        //minigame2.SetActive(true);
    }
     IEnumerator WaitBeforeNext()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //Start next mini game here after waiting 
        minigame2.SetActive(true);
        MinigameAudioController.Instance.startVoice(Resources.Load<AudioClip>("Audio/minigame2"));

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    private void startMinigame3() {
        MinigameAudioController.Instance.startVoice(Resources.Load<AudioClip>("Audio/minigame3"));
        minigame2.SetActive(false);
        minigame3.SetActive(true);
    }

    private void advanceToNextCheckpoint() {
        playerMovement.advanceToNextCheckpoint();
    }

    public void playHint() {
        AudioSource audioSource;
        switch(hintNumber) {
            case 0:
            audioSource = GameObject.Find("Checkpoint1").GetComponent<AudioSource>();
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
            break;
            case 1:
            audioSource = GameObject.Find("Checkpoint2").GetComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>("Audio/barca_instr");
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
            break;
            case 2:
            audioSource = GameObject.Find("Checkpoint2").GetComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>("Audio/gunoi_instr");
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
            break;
            default:
            break;
        }
    }

    private void setBoatPartsAsChild() {
        GameObject part1 = GameObject.FindGameObjectWithTag("BoatPartSquare");
        GameObject part2 = GameObject.FindGameObjectWithTag("BoatPartCircle");
        GameObject part3 = GameObject.FindGameObjectWithTag("BoatPartTriangle");

        GameObject boat = GameObject.FindGameObjectWithTag("Boat");

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        part1.transform.parent = boat.transform;
        part2.transform.parent = boat.transform;
        part3.transform.parent = boat.transform;
        player.transform.parent = boat.transform;
        boat.transform.position = boat.transform.position + new Vector3(0f, -0.3f, 0f);
        Vector3 position = boat.transform.position;
        player.transform.position = position + new Vector3(3f, 2f, 0f);

        camera.GetComponent<CameraController>().offsetY = 1f;
    }

    private IEnumerator displayTextForSeconds(string text, float seconds) {
        // winText.text = text;
        // winText.enabled = true; // Enable the text so it shows
        yield return new WaitForSeconds(seconds);
        // winText.enabled = false; // Disable the text so it is hidden
        // winText.text = "";
    }

    private void setInstruction(string text) {
        //instructionText.text = text;
    }

    private void onFinish() {
        string username = PlayerPrefs.GetString("Username");
        
        List<string[]> data = new List<string[]>
        {
            new string[] { username, computeScore()}
        };

        csvWriter.WriteToCSV("Rezultate.csv", data);
    }

    private string computeScore() {
        /**
        mere 15%

        fructe cazatoare 10%
        numar 20%
        simbol 30%

        piese barca 15%
        **/

        float appleScore = 15 - 5*applesCollected;
        if(appleScore<0){appleScore=0f;}
        float boatScore = 15 - 3*boatPartsWrong;
        if(boatScore<0){boatScore=0;}
        float fallingScore = 10 - 1*fallingWrong;
        if(fallingScore<0){fallingScore=0;}
        float minigame2Score = 20;
        if(minigame2Mistakes==1){minigame2Score-=5;}
        else if(minigame2Mistakes==2){minigame2Score-=15;}
        else if(minigame2Mistakes>2){minigame2Score=0;}
        float minigame3Score = 30;
        if(minigame3Mistakes==1){minigame3Score-=10;}
        else if(minigame3Mistakes>1){minigame3Score=5;}


        float score = 10 + appleScore + fallingScore + boatScore + minigame2Score + minigame3Score;
        Debug.Log("Score: " + score);
        return score.ToString();
    }
}
