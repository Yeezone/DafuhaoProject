using UnityEngine;

namespace com.QH.QPGame.WXHH
{    
	[RequireComponent(typeof (MeshFilter))]
	public class MeshMorpher : MonoBehaviour
	{
	  public bool m_AnimateAutomatically = true;
	  public float m_OneLoopLength = 1f;
	  public WrapMode m_WrapMode = WrapMode.Loop;
	  private int m_SrcMesh = -1;
	  private int m_DstMesh = -1;
	  private float m_Weight = -1f;
	  public Mesh[] m_Meshes;
	  public float m_AutomaticTime;
	  private Mesh m_Mesh;

	  public void SetComplexMorph(int srcIndex, int dstIndex, float t)
	  {
	    if (this.m_SrcMesh == srcIndex && this.m_DstMesh == dstIndex && Mathf.Approximately(this.m_Weight, t))
	      return;
	    Vector3[] vertices1 = this.m_Meshes[srcIndex].vertices;
	    Vector3[] vertices2 = this.m_Meshes[dstIndex].vertices;
	    Vector3[] vector3Array = new Vector3[this.m_Mesh.vertexCount];
	    for (int index = 0; index < vector3Array.Length; ++index)
	      vector3Array[index] = Vector3.Lerp(vertices1[index], vertices2[index], t);
	    this.m_Mesh.vertices = vector3Array;
	    this.m_Mesh.RecalculateBounds();
	  }

	  public void SetMorph(float t)
	  {
	    int srcIndex = Mathf.Clamp((int) t, 0, this.m_Meshes.Length - 2);
	    float num = t - (float) srcIndex;
	    float t1 = Mathf.Clamp(t - (float) srcIndex, 0.0f, 1f);
	    this.SetComplexMorph(srcIndex, srcIndex + 1, t1);
	  }

	  private void Awake()
	  {
	    //this.enabled = this.m_AnimateAutomatically;
	    MeshFilter meshFilter = this.GetComponent(typeof (MeshFilter)) as MeshFilter;
	    for (int index = 0; index < this.m_Meshes.Length; ++index)
	    {
	      if ((Object) this.m_Meshes[index] == (Object) null)
	      {
	        Debug.Log((object) ("MeshMorpher mesh  " + (object) index + " has not been setup correctly"));
	        this.m_AnimateAutomatically = false;
	        return;
	      }
	    }
	    if (this.m_Meshes.Length < 2)
	    {
	      Debug.Log((object) "The mesh morpher needs at least 2 source meshes");
	      this.m_AnimateAutomatically = false;
	    }
	    else
	    {
	      meshFilter.sharedMesh = this.m_Meshes[0];
	      this.m_Mesh = meshFilter.mesh;
	      int vertexCount = this.m_Mesh.vertexCount;
	      for (int index = 0; index < this.m_Meshes.Length; ++index)
	      {
	        if (this.m_Meshes[index].vertexCount != vertexCount)
	        {
	          Debug.Log((object) ("Mesh " + (object) index + " doesn't have the same number of vertices as the first mesh"));
	          this.m_AnimateAutomatically = false;
	          break;
	        }
	      }
	    }
	  }

	  private void Update()
	  {
	    if (!this.m_AnimateAutomatically)
	      return;
	    this.m_AutomaticTime += Time.deltaTime * (float) (this.m_Meshes.Length - 1) / this.m_OneLoopLength;
	    this.SetMorph(this.m_WrapMode != WrapMode.Loop ? (this.m_WrapMode != WrapMode.PingPong ? Mathf.Clamp(this.m_AutomaticTime, 0.0f, (float) (this.m_Meshes.Length - 1)) : Mathf.PingPong(this.m_AutomaticTime, (float) (this.m_Meshes.Length - 1))) : Mathf.Repeat(this.m_AutomaticTime, (float) (this.m_Meshes.Length - 1)));
	  }
	}
}