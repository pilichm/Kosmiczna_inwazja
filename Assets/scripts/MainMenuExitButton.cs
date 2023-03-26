using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuExitButton : MonoBehaviour
{
    private Button button;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        button.onClick.AddListener(EndGame);
    }

    // Function for ending game on click.
    private void EndGame()
    {
        Debug.Log(button.gameObject.name);
        gameManager.EndGame();
    }
}
