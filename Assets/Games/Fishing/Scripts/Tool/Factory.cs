using UnityEngine;
using System.Collections.Generic;

namespace com.QH.QPGame.Fishing
{
	public class Factory : MonoBehaviour
	{
	    // object buffer, instance id as key
	    static Dictionary<int, List<Transform>> buffer = new Dictionary<int, List<Transform>>();
	    // map from instance id to source id
	    static Dictionary<int, int> originID = new Dictionary<int, int>();

	    // instantiate
	    public static Transform Create(Transform obj, Vector3 pos, Quaternion rot)
	    {
	        if (obj == null)
	            return null;

	        Transform newObj = null;

	        int objID = obj.GetInstanceID();      // get instance id, Returns the instance id of the object.
	        if (buffer.ContainsKey(objID))
	        {
	            // if object buffer available

	            if (buffer[objID].Count > 0)
	            {
	                List<Transform> bufferObj = buffer[objID];
	                newObj = bufferObj[bufferObj.Count - 1];
	                bufferObj.RemoveAt(bufferObj.Count - 1);

	                newObj.position = pos;
	                newObj.rotation = rot;
	                newObj.gameObject.SetActive(true);

	                if (mySelf)
					{
						// newObj.parent = mySelf;
					}
	            }
	            else
	            {
	                newObj = (Transform)Instantiate(obj, pos, rot);

	                int newObjID = newObj.GetInstanceID();
	                originID.Add(newObjID, objID);

	                if (mySelf)
					{
						// newObj.parent = mySelf;
					}
	            }

	        }
	        else
	        {
	            // if object buffer unavailable

	            List<Transform> bufferObj = new List<Transform>();
	            buffer.Add(objID, bufferObj);

	            newObj = (Transform)Instantiate(obj, pos, rot);

	            int newObjID = newObj.GetInstanceID();
	            originID.Add(newObjID, objID);

	            if (mySelf)
	                newObj.parent = mySelf;
	        }

	        return newObj;
	    }
	    
	    // recycle an object
	    public static void Recycle(Transform obj)
	    {
	        int objID = obj.GetInstanceID();
	        if (originID.ContainsKey(objID))
	        {
	            obj.gameObject.SetActive(false);
	           // obj.parent = mySelf;

	            List<Transform> bufferObj = buffer[originID[objID]];
	            if (bufferObj != null)
	            {
	                bufferObj.Add(obj);
	                return;
	            }
	        }
	        Destroy(obj.gameObject);
	    }

	    static Transform mySelf;

	    void Awake()
	    {
	        mySelf = transform;
	    }

		void OnDestroy()
		{
			mySelf = null;
		}

		public static void Clear()
	    {
	        buffer.Clear();
	        originID.Clear();
	    }
	}
}