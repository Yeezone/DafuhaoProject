using UnityEngine;
using System.Collections;

public class ExitManager : MonoBehaviour {
    public GameObject ExitWindow;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            if (!ExitWindow.activeSelf)
            {
                ExitWindow.SetActive(true);
            }
            else
            {
                ExitWindow.SetActive(false);
            }
          
        }
    }
}
