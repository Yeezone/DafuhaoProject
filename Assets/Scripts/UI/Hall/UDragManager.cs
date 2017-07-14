using UnityEngine;

public class UDragManager : MonoBehaviour {

	public Transform target;
	
	private Vector3 offset;
	private Bounds bounds;

	void Awake () {
		UIEventListener.Get (this.gameObject).onDrag = DragWindow;
		UIEventListener.Get (this.gameObject).onPress = pressWindow;
	}


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DragWindow(GameObject obj,Vector2 point)
	{
		Vector3 currentPoint = new Vector3 (Input.mousePosition.x - offset.x, Input.mousePosition.y - offset.y, 0f);

		//如果坐标小于0
		if (currentPoint.x + bounds.size.x / 2.0f < 0) 
		{
			currentPoint.x = -bounds.size.x / 2.0f;
		}
		//如果坐标大于屏幕宽
		if (currentPoint.x + bounds.size.x > Screen.width) 
		{
			currentPoint.x = Screen.width - bounds.size.x;
		}

//		//如果坐标小于0
//		if (currentPoint.y - bounds.size.y / 2.0f< 0) 
//		{
//			currentPoint.y = bounds.size.y / 2.0f;
//		}

		//如果坐标小于0
		if (currentPoint.y < 0) 
		{
			currentPoint.y = 0;
		}

		//如果坐标大于屏幕高
		if (currentPoint.y + bounds.size.y / 2.0f > Screen.height) 
		{
			currentPoint.y = Screen.height - bounds.size.y / 2.0f;
		}

		currentPoint.x += bounds.size.x / 2;
		currentPoint.y += bounds.size.y / 2;
		
		target.position = UICamera.currentCamera.ScreenToWorldPoint (currentPoint);
	}

	void pressWindow(GameObject win, bool isHover)
	{	
		if (target == null) return;
		if (isHover) {
			bounds = NGUIMath.CalculateRelativeWidgetBounds(target.transform);
			Vector3 position = UICamera.currentCamera.WorldToScreenPoint(target.position);
			offset = new Vector3(Input.mousePosition.x - (position.x - bounds.size.x / 2), Input.mousePosition.y - (position.y - bounds.size.y / 2),0f);

		}
	}
}
