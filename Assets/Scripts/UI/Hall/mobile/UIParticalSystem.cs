using UnityEngine;
using System.Collections;

public class UIParticalSystem : MonoBehaviour {

	public	GameObject[]		ObjParticals;
	public	int					ParticalCount;
	public	float				WidthArea;
	public	float				HeightArea;
	public	float				LifeTimeDownLimit;
	public	float				LifeTimeUpLimit;
	public	float				ScaleDownLimit;
	public	float				ScaleUpLimit;
	public	float				RotationArea;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.childCount < ParticalCount)
		{
			GameObject childPartical = Instantiate(ObjParticals[Random.Range(0,ObjParticals.Length)]);
			childPartical.transform.parent = transform;
			childPartical.transform.localPosition = new Vector2( Random.Range(-WidthArea,WidthArea),Random.Range(-HeightArea,HeightArea) );
			float tempScale = Random.Range(ScaleDownLimit,ScaleUpLimit);
			childPartical.transform.localScale = new Vector2(tempScale,tempScale);
			childPartical.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y,Random.Range(-RotationArea,RotationArea));
			childPartical.transform.GetComponent<UIPartical>().LifeTime = Random.Range(LifeTimeDownLimit,LifeTimeUpLimit);
		}
	}
}
