using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ResetManager : MonoBehaviour
{
    [SerializeField] public Button resetButton;
    // Start is called before the first frame update
    void Start()
    {
        resetButton.onClick.AddListener(Reset);
    }

    void Reset() {
        SceneManager.LoadScene("Login");
    }

    
}
