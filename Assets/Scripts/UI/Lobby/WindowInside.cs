using UnityEngine;
using System.Collections;

public class WindowInside : MonoBehaviour {

	public Transform MovedTrans;

	private Vector2 widgetSize = Vector2.zero;
	private float maxX,minX,maxY,minY;

	// Use this for initialization
	void Start () {
		if(GetComponent<UIWidget>()!=null)
		{
			widgetSize = GetComponent<UIWidget>().localSize;
			maxX = ((float)Screen.width-widgetSize.x)/2;
			minX = (widgetSize.x-(float)Screen.width)/2;
			maxY = ((float)Screen.height-widgetSize.y)/2;
			minY = (widgetSize.y-(float)Screen.height)/2;
			Debug.LogWarning("maxX: "+maxX +"   minX: "+minX + "  maxY: "+maxY + "  minY: " +minY);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(widgetSize!=Vector2.zero && MovedTrans!=null)
		{
			if(MovedTrans.localPosition.x>maxX)
			{
				MovedTrans.localPosition = new Vector2(maxX,MovedTrans.localPosition.y);
			}else if(transform.localPosition.x<minX)
			{
				MovedTrans.localPosition = new Vector2(minX,MovedTrans.localPosition.y);
			}
			if(MovedTrans.localPosition.y>maxY)
			{
				MovedTrans.localPosition = new Vector2(MovedTrans.localPosition.x,maxY);
			}else if(MovedTrans.localPosition.y<minY)
			{
				MovedTrans.localPosition = new Vector2(MovedTrans.localPosition.x,minY);
			}
		}
	}
}
