using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Drone : MonoBehaviour {

	public float speed = 3.0f;
	public float rotSpeed = 180.0f;
	private int selectionIndex = 0;
	public List<Connection> connections = new List<Connection> ();
	bool isActive = false;

	// Use this for initialization
	void Start () {
		foreach (Connection c in connections) 
		{
			c.Obj = transform;
			c.IsAvailable = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (Input.GetKey (KeyCode.W)) 
				transform.position += transform.forward * speed * Time.deltaTime;
			else if (Input.GetKey (KeyCode.S)) 
				transform.position -= transform.forward * speed * Time.deltaTime;
		
			if (Input.GetKey (KeyCode.A)) 
				transform.RotateAround (transform.position, Vector3.up, -rotSpeed * Time.deltaTime);
			else if (Input.GetKey (KeyCode.D)) 
				transform.RotateAround (transform.position, Vector3.up, rotSpeed * Time.deltaTime);
		}
	}

	public void Activate()
	{
		isActive = true;
	}

	public void Deactivate()
	{
		isActive = false;
	}

	public void Connect(ComponentBase cmp)
	{
		Debug.Log ("Connecting...");
		int i = CurrentIndex();
		if (i == -1)
			return;

		connections[i].IsAvailable = false;
		cmp.GetComponent<Rigidbody>().isKinematic = true;
		cmp.transform.position = connections[i].pointPos;
		cmp.transform.LookAt(2*cmp.transform.position - connections[i].Obj.position);
		cmp.transform.parent = connections[i].Obj.transform;
		foreach(Connection c in cmp.connections)
			connections.Add (c);

	}

	public void Disconnect(ComponentBase cmp, bool checkConn = true)
	{
		if (checkConn) {
			Transform prnt = cmp.transform.parent;
			while (prnt != null && prnt != transform)
				prnt = prnt.parent;

			if (prnt == null)
				return;
		}

		var children = cmp.transform.GetComponentsInChildren<ComponentBase> ();

		if(children != null && children.Length > 0)
			for (int i = 0; i < children.Length; ++i)
				if(cmp != children[i] && children[i] != null) Disconnect (children[i],false);

		foreach (Connection c in cmp.connections) 
		{
			connections.Remove (c);
		}

		cmp.GetComponent<Rigidbody>().isKinematic = false;
		cmp.transform.parent = null;

		var con = connections.Where ((c) => c.pointPos == cmp.transform.position);

		if (con != null && con.Count() > 0)
			con.First ().IsAvailable = true;
	}

	public int CurrentIndex()
	{
		if (connections.Count == 0)
			return -1;

		if (selectionIndex == -1 || connections.Count <= selectionIndex)
			selectionIndex = 0;

		if (!connections [selectionIndex].IsAvailable)
		{
			for(int i = 1; i < connections.Count+1; ++i)
			{
				if(connections.Count == i) selectionIndex = -1;
				else if(connections[selectionIndex+i-(selectionIndex+i < connections.Count ? 0 : connections.Count)].IsAvailable) 
				{
					selectionIndex = selectionIndex+i-(selectionIndex+i < connections.Count ? 0 : connections.Count);
					i = connections.Count + 1;
				}
			}

		}

		return selectionIndex;
	}

	public int NextIndex()
	{
		for (int i = 1; i < connections.Count+1; ++i) {
			if (connections.Count == i)
				selectionIndex = -1;
			else if (connections [selectionIndex + i - (selectionIndex + i < connections.Count ? 0 : connections.Count)].IsAvailable) {
				selectionIndex = selectionIndex + i - (selectionIndex + i < connections.Count ? 0 : connections.Count);
				i = connections.Count + 1;
			}
		}
		return selectionIndex;
	}

	public int PreviousIndex()
	{
		for (int i = 1; i < connections.Count+1; ++i) {
			if (connections.Count == i)
				selectionIndex = -1;
			else if (connections [selectionIndex - i + (selectionIndex - i >= 0 ? 0 : connections.Count)].IsAvailable) {
				selectionIndex = selectionIndex - i + (selectionIndex - i >= 0 ? 0 : connections.Count);
				i = connections.Count + 1;
			}
		}
		return selectionIndex;
	}

}
