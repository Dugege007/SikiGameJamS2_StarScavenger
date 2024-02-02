using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    #region Public Variables
    public bool isTimedScreen = false;
    public float screenTime = 3f;
    public GameObject targetTimedScreen;
    #endregion

    #region Private Variables
    private float startTime = 0f;
    #endregion

    #region Main Methods
    // Start is called before the first frame update
    void Start()
    {
        SetupCanvas();
    }

    private void OnEnable()
    {
        SetupCanvas();
    }
    // Update is called once per frame
    void Update()
    {
        if (isTimedScreen)
        {
            while (Time.time < startTime + screenTime)
            {
                return;
            }
            if (targetTimedScreen)
            {
                gameObject.SetActive(false);
                targetTimedScreen.SetActive(true);
            }
        }
    }
    #endregion
    #region Utility Methods
    void SetupCanvas()
    {
        startTime = Time.time;
    }
    #endregion
}