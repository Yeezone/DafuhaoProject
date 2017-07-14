using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class DeskCreator : MonoBehaviour 
{
	public virtual void Init(HallTransfer.RoomDeskInfo deskInfo){}
}

