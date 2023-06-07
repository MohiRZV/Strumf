using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
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
    private bool reachedEnd = false;

    public CSVWriter csvWriter = new CSVWriter();
    [SerializeField] private GameObject resetButton;

    [SerializeField] Text winText;
    [SerializeField] Text instructionText;
    [SerializeField] GameObject player;
    private PlayerMovement playerMovement;
    public enum Collectible {
        Cherry,
        Strawberry,
        Apple,
        RipeApple,
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
        // hide reset button
        // TODO insert narration for storyline + apple task
        resetButton.SetActive(false);
        playerMovement = player.GetComponent<PlayerMovement>();
        // You can increase the 1 second delay to as much as the narration for the story lasts
        Invoke("advanceToNextCheckpoint",1);
        setInstruction("Prima data va fi nevoie sa culegi merele coapte de sub copac si sa le pui in cos!");
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
            default:
                break;
        }
        checkWinCnd();
    }

    private void checkWinCnd() {
        if (ripeAppleCollected >= ripeAppleWinCnd) {// ripe apple task over
            //TODO insert narration for the boat task
            advanceToNextCheckpoint();
            ripeAppleCollected = -1;
            setInstruction("Oh nu! Barca are niste piese lipsa! Pune-le la locul potrivit!");
        }

        if (boatPartsPlaced >= boatPartsWinCnd) {//boat parts task over
            //TODO insert narration for the junk task
            boatPartsPlaced = -1;
            setBoatPartsAsChild();
            advanceToNextCheckpoint();
            setInstruction("Apa este poluata si nu putem avansa! Strange deseurile si pune-le in cosul de reciclare!");
        }

        if (junkCollected >= junkWinCnd) {
             //TODO insert narration for game finished
            junkCollected = -1;//junk task over
            setInstruction("L-ai gasit pe prietenul tau! Felicitari!");
            player.transform.parent = null;
            player.transform.position = new Vector3(51f, 2f, 9f);
            reachedEnd = true;
        }
        
        if (ripeAppleCollected== -1 && boatPartsPlaced == -1 && junkCollected == -1 && reachedEnd) {// game finished
            resetButton.SetActive(true);
            winText.text = "GG, esti un stroomph!";
            winText.enabled = true;
            onFinish();
        }
    }

    private void advanceToNextCheckpoint() {
        playerMovement.advanceToNextCheckpoint();
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
        winText.text = text;
        winText.enabled = true; // Enable the text so it shows
        yield return new WaitForSeconds(seconds);
        winText.enabled = false; // Disable the text so it is hidden
        winText.text = "";
    }

    private void setInstruction(string text) {
        instructionText.text = text;
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
        float score = 100 - applesCollected * 10 - boatPartsWrong * 5;
        Debug.Log("Score: " + score);
        return score.ToString();
    }
}
