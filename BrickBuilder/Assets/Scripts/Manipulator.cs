using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.VR.WSA;
using UnityEngine.UI;

public class Manipulator : Singleton<Manipulator> {

    // Prefabs
    public GameObject brick;
    public GameObject handlers;

    // Voice Command Support
    KeywordRecognizer keywordRecognizer;
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    public GameObject tappedBrick;
    public GameObject tappedHandle;
    private int brickCounter;

    // Goal
    [Tooltip("Target Finish")]
    public GameObject targetFinish;

    // Assembly
    public GameObject Assembly;

    // For debugging in HoloLens
    public GameObject consolePanel;
    private Text consoleText;


    void Start() {
        // Voice Commands
        keywordCollection = new Dictionary<string, KeywordAction>();
        keywordCollection.Add("New", AddBrick);
        keywordCollection.Add("Delete", DeleteBrick);
        keywordCollection.Add("Show Hint", ShowHint);
        keywordCollection.Add("Dismiss", DismissHint);
        keywordCollection.Add("Console", ConsoleSwitch);
        keywordCollection.Add("Reset Assembly", ResetAssembly);

        // Initialize KeywordRecognizer with the previously added keywords.
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        brickCounter = 0;
        tappedBrick = null;
        tappedHandle = null;
        handlers.SetActive(false);

        consoleText = consolePanel.GetComponentInChildren<Text>();
    }

    void OnDestroy()
    {
        keywordRecognizer.Dispose();
    }

    public void Update () {
        // DEBUG ONLY!!!
        string buffer;
        consoleText.text = " GazeManager >> ";
        buffer = (GazeManager.Instance.Hit)? GazeManager.Instance.HitInfo.transform.name.ToString() : "null";
        consoleText.text += buffer;
        consoleText.text += "\n InteractibleManager >> ";
        buffer = (InteractibleManager.Instance.FocusedGameObject != null) ? InteractibleManager.Instance.FocusedGameObject.name : "null";
        consoleText.text += buffer;
        consoleText.text += "\n GestureManager >> ";
        buffer = (GestureManager.Instance.focusedObject != null) ? GestureManager.Instance.focusedObject.name : "null";
        consoleText.text += buffer;
        consoleText.text += "\n Manipulator >> TappedBrick : ";
        buffer = (tappedBrick != null) ? tappedBrick.name : "null";
        consoleText.text += buffer;
        consoleText.text += "\n MagneticCollision >> Count: ";
        buffer = (tappedBrick != null && tappedBrick.GetComponent<MagneticCollision>() !=null) ? tappedBrick.GetComponent<MagneticCollision>().DetectedGameObjects.Count.ToString() : "null";
        consoleText.text += buffer;
    }

    // Actions after voice command is recognized
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    // Add a new brick at the gaze point
    private void AddBrick(PhraseRecognizedEventArgs args)   
    {
        brickCounter = GameObject.FindGameObjectsWithTag("brick").Length;
        GameObject newBrick = (GameObject)Instantiate(brick, GazeManager.Instance.Position, Quaternion.identity);
        newBrick.name = "Brick_" + brickCounter.ToString();
        // Move and SetActive handlers to current brick
        handlers.transform.position = newBrick.transform.position;
        handlers.transform.rotation = newBrick.transform.rotation;
        handlers.SetActive(true);
        tappedBrick = newBrick;
    }

    private void DeleteBrick(PhraseRecognizedEventArgs args)
    {
        if (tappedBrick == null || tappedBrick.tag == "assembly")    // must have a brick selected; assembly (container) can't be deleted
        {
            return;
        }

        tappedBrick.SetActive(false);
        handlers.SetActive(false);
    }

    // Show hint of the target
    private void ShowHint(PhraseRecognizedEventArgs args)
    {
        targetFinish.SetActive(true);
        // Move and SetActive handlers to current brick
        handlers.transform.position = targetFinish.transform.position;
        handlers.transform.rotation = targetFinish.transform.rotation;
        handlers.SetActive(true);
        tappedBrick = targetFinish;
    }
    // Dismiss hint
    private void DismissHint(PhraseRecognizedEventArgs args)
    {
        targetFinish.SetActive(false);
        tappedBrick.SetActive(false);
        handlers.SetActive(false);
    }

    private void ConsoleSwitch(PhraseRecognizedEventArgs args)
    {
        consolePanel.SetActive(!consolePanel.activeSelf);
    }

    // Destroy all clone bricks in Assembly
    private void ResetAssembly(PhraseRecognizedEventArgs args)
    {
        Assembly.SendMessageUpwards("Reset");
    }

    public void TapManager(GameObject tappedGameObject)
    {
        if (tappedGameObject.tag == "brick")
        {
            tappedBrick = tappedGameObject;
            // Move and SetActive handlers to current brick
            handlers.transform.position = tappedBrick.transform.position;
            handlers.transform.rotation = tappedBrick.transform.rotation;
            handlers.SetActive(!handlers.activeSelf);

            // Assembly
            if (handlers.activeSelf && tappedBrick.GetComponent<MagneticCollision>().DetectedGameObjects.Count > 0)
            {
                foreach (GameObject magnified in tappedBrick.GetComponent<MagneticCollision>().DetectedGameObjects)
                {
                    Assembly.SendMessageUpwards("Replicate", magnified);
                }
                Assembly.SendMessageUpwards("Replicate", tappedBrick);
                handlers.SetActive(false);
            }
         
        }
        else if ( tappedGameObject.tag == "assembly")
        {
            tappedBrick = tappedGameObject;
            // Move and SetActive handlers to current brick
            handlers.transform.position = tappedBrick.transform.position;
            handlers.transform.rotation = tappedBrick.transform.rotation;
            handlers.SetActive(!handlers.activeSelf);
        }
        else if (tappedGameObject.tag == "handler" && tappedBrick != null)
        {
            tappedHandle = tappedGameObject;

            switch (tappedHandle.name)
            {
                case "Handler_X":
                    GestureManager.Instance.Translation_X(GestureManager.Instance.ManipulationRecognizer);
                    break;
                case "Handler_Y":
                    GestureManager.Instance.Translation_Y(GestureManager.Instance.ManipulationRecognizer);
                    break;
                case "Handler_Z":
                    GestureManager.Instance.Translation_Z(GestureManager.Instance.ManipulationRecognizer);
                    break;
                case "Handler_Xr":                    
                    GestureManager.Instance.RotationX(GestureManager.Instance.ManipulationRecognizer);
                    break;
                case "Handler_Yr":
                    GestureManager.Instance.RotationY(GestureManager.Instance.ManipulationRecognizer);
                    break;
                case "Handler_Zr":
                    GestureManager.Instance.RotationZ(GestureManager.Instance.ManipulationRecognizer);
                    break;
                default:
                    break;
            }
        }
    }

}
