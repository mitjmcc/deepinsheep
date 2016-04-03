using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu_Avery : MonoBehaviour {
    //clip that plays when selection changes
    public AudioClip switchButtonSound;
    public AudioClip SelectSound;

    //buttons that make up the two menus
    public Button[] mainMenuButtons;
    public Button[] sceneSelectButtons;

    //the button in which is currently selected of the current list
    private int selectedIndex;

    //canInteract - variable that helps keep a delay in between selections
    //menuActive - if true, input selects options of the main menu
    //sceneActive - if true, input selects options of the scene selection menu
    private bool canInteract;
    private bool menuActive;
    private bool sceneActive;
    private bool optionsActive;
    //canvas in worldspace in which buttons apear
    public GameObject mainMenuCanvas;
    public GameObject sceneSelectCanvas;
    public GameObject optionsmenuCanvas;

    //objects references for animation purposes
    public GameObject barn;
    public GameObject cameraController;

    //runs on startup
    void Start()
    {
        optionsmenuCanvas.SetActive(false);
        menuActive = true;
        sceneActive = false;
        optionsActive = false;
        canInteract = true;
        selectedIndex = 0;
    }

    //this method is responsible for changing the index of the current menu.
    //parameter buttons (which set of buttons to be indexed) 
    //parameter selectedInput (the direction in which buttons will be changed)
    IEnumerator menuSelect(Button[] buttons,  float selectedInput)
    {
        AudioSource.PlayClipAtPoint(switchButtonSound, Camera.main.transform.position);
        buttons[selectedIndex].GetComponent<Animation>().Stop();

        if (selectedInput > 0)
        {
            if (selectedIndex == 0) {
                selectedIndex = buttons.Length - 1;
            } else {
                selectedIndex--;
            }
        }

        if (selectedInput < 0)
        {
            if (selectedIndex == buttons.Length - 1)
            {
                selectedIndex = 0;
            }
            else
            {
                selectedIndex++;
            }

        }
        yield return new WaitForSeconds(0.5f);
        canInteract = true;
    }

    //called every frame
    //takes in input and decides which menu will take that input
    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Move Vertical 0");
        float horizontalInput = Input.GetAxis("Move Horizontal 0");

        if (menuActive)
        {
            mainMenuButtons[selectedIndex].GetComponent<Animation>().Play();
            if (verticalInput != 0 && canInteract)
            {
                canInteract = false;
                StartCoroutine(menuSelect(mainMenuButtons, verticalInput));
            }

            if (Input.GetAxis("Hit 0") > 0 && canInteract)
            {
                canInteract = false;
                StartCoroutine(handleSelectionMain());
            }
        }

        if (optionsActive)
        {
            if (verticalInput != 0 && canInteract)
            {
                canInteract = false;
                StartCoroutine(menuSelect(mainMenuButtons, verticalInput));
            }

            if (Input.GetAxis("Hit 0") > 0 && canInteract)
            {
                canInteract = false;
                StartCoroutine(handleSelectionMain());
            }

            mainMenuButtons[selectedIndex].GetComponent<Animator>().SetTrigger("menuSwitch");
        }

        if (sceneActive)
        {
            if (horizontalInput != 0 && canInteract)
            {
                canInteract = false;
                StartCoroutine(menuSelect(sceneSelectButtons, -horizontalInput));
            }

            if (Input.GetAxis("Hit 0" ) > 0 && canInteract)
            {
                canInteract = false;
                StartCoroutine(handleSelectionScene());
            }

            sceneSelectButtons[selectedIndex].GetComponent<Animation>().Play();
        }
    }

    //handles which method is called from selected button
    IEnumerator handleSelectionMain()
    {
        AudioSource.PlayClipAtPoint(SelectSound, Camera.main.transform.position);
        yield return new WaitForSeconds(0.5f);
        canInteract = true;
        switch (selectedIndex)
        {
            case 0:
                PlayGame();
                break;
            case 1:
                StartTutorial();
                break;
            case 2:
                StartOptions();
                break;
            case 3:
                Quit();
                break;
            default:
                break;
        }

    }

    //method envolked when play button is selected. triggers animation to the
    // barn and camera. sets input to scene selection menu. 
    void PlayGame()
    {
        mainMenuButtons[selectedIndex].GetComponent<Animation>().Stop();
        menuActive = false;
        sceneActive = true;
        selectedIndex = 0;
        cameraController.GetComponent<Animator>().Play("SceneSelect");
        barn.GetComponent<Animator>().Play("BarnDoors");
    }

    //handles input in the scene selection menu depending on button
    IEnumerator handleSelectionScene()
    {
        AudioSource.PlayClipAtPoint(SelectSound, Camera.main.transform.position);
        yield return new WaitForSeconds(0.5f);
        canInteract = true;

        switch (selectedIndex)
        {
            case 0:
                SceneManager.LoadScene("RollingHills");
                break;
            case 1:
                SceneManager.LoadScene(2);
                break;
            case 2:
                SceneManager.LoadScene(3);
                break;
            case 3:
                BackToMenu();
                break;
            default:
                break;
        }
    }

    //the method in the scene selection menu that returns to the main menu
    void BackToMenu()
    {
        sceneSelectButtons[selectedIndex].GetComponent<Animation>().Stop();
        menuActive = true;
        sceneActive = false;
        selectedIndex = 0;
        cameraController.GetComponent<Animator>().SetTrigger("CameraBack");
        barn.GetComponent<Animator>().SetTrigger("CloseTrigger");
    }

    void StartTutorial()
    {

    }

    //not finished, but gives an idea of what to come
    void StartOptions()
    {
        menuActive = false;
        optionsActive = true;
        mainMenuCanvas.SetActive(false);
        cameraController.GetComponent<Animator>().Play("menuSwitch");
        optionsmenuCanvas.SetActive(true);
    }

    //selected when quit is selected and exits the application
    void Quit()
    {
        Application.Quit();
    }

}
