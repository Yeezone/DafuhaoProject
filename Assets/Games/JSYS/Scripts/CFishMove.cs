using UnityEngine;
using System.Collections;

public class CFishMove : MonoBehaviour {


    public Vector3 m_vStarPos;
    public Vector3 m_vEndPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MoveFish()
    {
        this.transform.localPosition = m_vStarPos;
        this.transform.GetComponent<TweenPosition>().from = m_vStarPos;
        this.transform.GetComponent<TweenPosition>().to = m_vEndPos;
        this.transform.GetComponent<TweenPosition>().enabled = true;
        this.transform.GetComponent<TweenPosition>().ResetToBeginning();

    }

    public void WaterLightRotion()
    {
        float z = this.transform.localRotation.z;
        this.transform.GetComponent<TweenRotation>().from = new Vector3(0, 0, z);
        if (z <= 0) z = 24;
        else z = -24;
        this.transform.GetComponent<TweenRotation>().to = new Vector3(0, 0, -z);
        this.transform.GetComponent<TweenRotation>().enabled = true;
   
        this.transform.GetComponent<TweenRotation>().ResetToBeginning();
    }
}
