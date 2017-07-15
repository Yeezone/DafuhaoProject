using UnityEngine;

public class testAudioSwitch : MonoBehaviour {

	public	Transform	RootCamera;

	// Use this for initialization
	void Start () {
	
	}
	void OnClick(){
		RootCamera.GetComponent<AudioListener>().enabled = !RootCamera.GetComponent<AudioListener>().enabled;
	}
}
