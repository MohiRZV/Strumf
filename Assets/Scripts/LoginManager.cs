using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI usernameText;
    [SerializeField] public Button startButton;

    private void Start() {
        startButton.onClick.AddListener(Login);
    }

    public void Login()
    {
        string username = usernameText.text;

        if (!string.IsNullOrEmpty(username))
        {
            // Perform the necessary login actions here
            Debug.Log("Logat ca: " + username);
            PlayerPrefs.SetString("Username", username);
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            // Display an error message if the username is empty
            Debug.LogError("Numele nu poate fi vid!");
        }
    }
}
