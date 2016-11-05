using HoloToolkit.Unity;
using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;

public class GestureManager : Singleton<GestureManager>
{
    // Manipulation and Tap gesture recognizer.
    public GestureRecognizer ManipulationRecognizer { get; private set; }

    // Currently active gesture recognizer.
    public GestureRecognizer ActiveRecognizer { get; private set; }

    public int IsManipulating { get; private set; } // 1: X; 2: Y; 3: Z; 0: null.

    public int IsRotating { get; private set; } // 1: X; 2: Y; 3: Z; 0: null.

    public Vector3 ManipulationPosition { get; private set; }

    public GameObject focusedObject;

    void Awake()
    {
        focusedObject = new GameObject();


        // Instantiate the ManipulationRecognizer.
        ManipulationRecognizer = new GestureRecognizer();

        // Add the ManipulationTranslate GestureSetting to the ManipulationRecognizer's RecognizableGestures.
        ManipulationRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate | GestureSettings.Tap);

        // Register for the Manipulation events on the ManipulationRecognizer.
        ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;
        ManipulationRecognizer.TappedEvent += ManipulationRecognizer_TappedEvent;

        ResetGestureRecognizers();
    }

    void OnDestroy()
    {
        // Unregister the Manipulation events on the ManipulationRecognizer.
        ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognizer_ManipulationCanceledEvent;
        ManipulationRecognizer.TappedEvent -= ManipulationRecognizer_TappedEvent;
    }

    /// <summary>
    /// Revert back to the default GestureRecognizer.
    /// </summary>
    public void ResetGestureRecognizers()
    {
        // Default to the manipulation gestures.
        //Transition(ManipulationRecognizer);
        ActiveRecognizer = ManipulationRecognizer;
        ActiveRecognizer.StartCapturingGestures();
    }

    /// <summary>
    /// Transition to a new GestureRecognizer.
    /// </summary>
    /// <param name="newRecognizer">The GestureRecognizer to transition to.</param>
    /// 

    public void FreeRotation(GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        if (ActiveRecognizer != null)
        {
            if (ActiveRecognizer == newRecognizer)
            {
                return;
            }

            ActiveRecognizer.CancelGestures();
            ActiveRecognizer.StopCapturingGestures();
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
        IsRotating = 4;
    }

    public void RotationX(GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
        IsManipulating = 0;
        IsRotating = 1;
    }

    public void RotationY(GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
        IsManipulating = 0;
        IsRotating = 2;
    }

    public void RotationZ(GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
        IsManipulating = 0;
        IsRotating = 3;
    }

    //public void Transition(GestureRecognizer newRecognizer)
    //{
    //    if (newRecognizer == null)
    //    {
    //        return;
    //    }

    //    if (ActiveRecognizer != null)
    //    {
    //        if (ActiveRecognizer == newRecognizer)
    //        {
    //            return;
    //        }

    //        ActiveRecognizer.CancelGestures();
    //        ActiveRecognizer.StopCapturingGestures();
    //    }

    //    newRecognizer.StartCapturingGestures();
    //    ActiveRecognizer = newRecognizer;
    //    IsManipulating = true;
    //}

    public void Translation_X(GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
        IsManipulating = 1;
        IsRotating = 0;
    }

    public void Translation_Y(GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
        IsManipulating = 2;
        IsRotating = 0;
    }

    public void Translation_Z(GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
        IsManipulating = 3;
        IsRotating = 0;
    }


    private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        Manipulator.Instance.tappedHandle.SendMessageUpwards("PerformManipulationStart", position);
    }

    private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        if (IsManipulating == 1)
        {
            ManipulationPosition = position;
            Manipulator.Instance.tappedHandle.SendMessageUpwards("PerformTranslation_X", position);
        }
        else if (IsManipulating == 2)
        {
            ManipulationPosition = position;
            Manipulator.Instance.tappedHandle.SendMessageUpwards("PerformTranslation_Y", position);
        }
        else if (IsManipulating == 3)
        {
            ManipulationPosition = position;
            Manipulator.Instance.tappedHandle.SendMessageUpwards("PerformTranslation_Z", position);
        }
        else if (IsRotating == 1)
        {
            Manipulator.Instance.tappedHandle.SendMessageUpwards("PerformRotation_X", position);
        }
        else if (IsRotating == 2)
        {
            Manipulator.Instance.tappedHandle.SendMessageUpwards("PerformRotation_Y", position);
        }
        else if (IsRotating == 3)
        {
            Manipulator.Instance.tappedHandle.SendMessageUpwards("PerformRotation_Z", position);
        }
    }

    private void ManipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = 0;
        IsRotating = 0;
    }

    private void ManipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = 0;
        IsRotating = 0;
    }

    private void ManipulationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
    {
        //GameObject focusedObject = InteractibleManager.Instance.FocusedGameObject;
        focusedObject = InteractibleManager.Instance.FocusedGameObject;

        if (focusedObject != null)
        {
            focusedObject.SendMessageUpwards("OnSelect");
            Manipulator.Instance.TapManager(focusedObject);
        }
    }
}