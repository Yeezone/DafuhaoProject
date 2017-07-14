using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace com.QH.QPGame.Utility
{
        public class SpawnManager :MonoBehaviour
        {
                public CacheObject[] caches = null;

                public void OnSpwan()
                {
                        for (int i = 0; i < caches.Length; i++)
                        {
                                caches[i].Clear();
                                caches[i].Initialize(this);
                        }
                }

                public void OnRecycle(GameObject g)
                {
                        if (g == null)
                        {
                                Debug.LogError("The Param Is Null");
                                return;
                        }
                        string tGameObjectName = g.name.Split('(')[0];

                        for (int i = 0; i < caches.Length; i++)
                        {

                                string tCacheName = caches[i].prefab.name;

                                if (tGameObjectName.ToLower() == tCacheName.ToLower())
                                {
                                        caches[i].Return(g);
                                        //Log.OnLog ("Recycle The Prefab of " + g.name);
                                        return;
                                }
                        }
                        Debug.LogError("Can't Recycle The Prefab Of " + g.name);
                }

                public GameObject Fetch(string name)
                {
                        for (int i = 0; i < caches.Length; i++)
                        {
								
                                string tCacheName = caches[i].prefab.name;
                                //Log.OnLog ("Fetch The Resouce Of " + tCacheName);
                                if (tCacheName.ToLower() == name.ToLower())
                                {
									
                                        return caches[i].Fetch();
                                }
                        }
                        Debug.LogError("Can't Find The Resouce Of " + name);
                        return null;
                }
                #if UNITY_EDITOR
                [MenuItem("Tools/Spawn/Populate All SpawnManager")]
                static void Populate()
                {
                        SpawnManager[] sm = GameObject.FindObjectsOfType(typeof(SpawnManager)) as SpawnManager[];
                        if (sm == null || sm.Length == 0)
                        {
                                Debug.LogError("Can't Find SpawnManager In The Scene");
                                return;
                        }
                        for (int i = 0; i < sm.Length; i++)
                        {
                                sm[i].OnSpwan();
                        }
                }
                #endif
        }
        #region CacheObject
        [System.Serializable]

        public class CacheObject
        {
                [HideInInspector]
                public SpawnManager
                        manager = null;
                public GameObject prefab = null;
                public int cacheSize = 0;
                [HideInInspector]
                public int
                        availabeCount = 0;
                [HideInInspector]
                public List<GameObject>
                        unAllotObjs = new List<GameObject>();
                [HideInInspector]
                public List<GameObject>
                        allotObjs = new List<GameObject>();

                public void Initialize(SpawnManager sm)
                {
                        this.manager = sm;
                        for (int i = 0; i < cacheSize; i++)
                        {
                                GameObject go = MonoBehaviour.Instantiate(prefab) as GameObject;
                                go.transform.parent = sm.transform;
                                go.transform.localScale = Vector3.one;
                                go.SetActive(false);
                                go.name = prefab.name + "(" + (i + 1).FormatToIndex() + ")";
                                unAllotObjs.Add(go);
                                availabeCount = cacheSize;
                        }
                }

                public void Clear()
                {
                        manager = null;
                        foreach (GameObject g in unAllotObjs)
                        {
                                GameObject.DestroyImmediate(g);		
                        }


                        foreach (GameObject g in allotObjs)
                        {
                                GameObject.DestroyImmediate(g);		
                        }

                        unAllotObjs.Clear();
                        allotObjs.Clear();
                }

                public GameObject Fetch()
                {
                        GameObject resoult = null;
                        if (availabeCount > 0)
                        {
                                resoult = unAllotObjs[0];
                                allotObjs.Add(resoult);
                                unAllotObjs.RemoveAt(0);
                                availabeCount--;
                        }
                        else
                        {
                                resoult = GameObject.Instantiate(prefab) as GameObject;
                                resoult.name = prefab.name + "(" + (allotObjs.Count + 1).FormatToIndex() + ")";
                                resoult.transform.parent = manager.transform;
                                resoult.transform.localScale = Vector3.one;
                                allotObjs.Add(resoult);
				
                        }
                        resoult.SetActive(true);
                        return resoult;
                }

                public void Return(GameObject gameobj)
                {
                        if (!unAllotObjs.Contains(gameobj))
                        {
                                allotObjs.Remove(gameobj);
                                unAllotObjs.Add(gameobj);
                                availabeCount++;
                                gameobj.transform.parent = manager.transform;
                                gameobj.transform.localPosition = Vector3.zero;
                                gameobj.transform.localScale = Vector3.one;
                                gameobj.SetActive(false);
                        }
                        else
                        {
                                Debug.LogError("Can't Recycle The GameObject Twice!!!!!");
                        }
					
                }
        }
        #endregion
}