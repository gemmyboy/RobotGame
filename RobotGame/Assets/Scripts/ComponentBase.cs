using UnityEngine;
using System.Collections;
using System;

public class ComponentBase : MonoBehaviour {

	public Connection[] connections;

	void Start()
	{
		foreach (Connection c in connections) 
		{
			c.Obj = transform;
			c.IsAvailable = true;
		}
	}
}

[Serializable]
public class Connection {

	public Transform Obj { get { return obj; } set { obj = value; } }
	public bool IsAvailable { get { return isAvailable; } set { isAvailable = value; } }

	protected Transform obj;
	public Vector3 point;
	bool isAvailable;
	public Vector3 pointPos 
	{get{return obj.position + obj.right * point.x + obj.up * point.y + obj.forward * point.z;}}

}
