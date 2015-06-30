using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {

	public Text mouseOverLabel;
	public GameObject connectionSelector;
	public int selectionIndex = 0;
	CharacterMovement character;
	Drone container;
	CameraFollow cF;
	// Use this for initialization
	void Start () {
		character = this.GetComponent<CharacterMovement> ();
		cF = Camera.main.GetComponent<CameraFollow> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if(container == null)
		{

			RaycastHit info;
			if (Physics.Raycast (ray, out info, 12.0f)) 
			{
				var cM = info.collider.gameObject.GetComponent<Drone>();
				if(cM != null)
				{
					mouseOverLabel.gameObject.SetActive(true);
					mouseOverLabel.rectTransform.anchoredPosition = (Vector2)Input.mousePosition-30.0f*Vector2.up;
					mouseOverLabel.text = info.collider.gameObject.name;
					if(Input.GetMouseButtonDown(0))
					{
						container = cM;
						container.Activate();
						character.enabled = false;
						character.gameObject.GetComponent<Rigidbody>().isKinematic = true;
						character.gameObject.GetComponent<Collider>().enabled = false;
						character.transform.parent = container.transform;
						character.transform.localPosition = Vector3.zero;
						cF.target = container.gameObject;
						mouseOverLabel.gameObject.SetActive(false);
					}

				} else mouseOverLabel.gameObject.SetActive(false);
			} else mouseOverLabel.gameObject.SetActive(false);
		}
		else
		{
			RaycastHit info;
			if (Physics.Raycast (ray, out info, 12.0f)) 
			{
				var cN = info.collider.gameObject.GetComponent<ComponentBase>();
				if(cN != null)
				{
					if(cN.transform.parent == null)
					{
						if(container.CurrentIndex() != -1)
						{
							connectionSelector.SetActive(true);
							connectionSelector.transform.position = container.connections[container.CurrentIndex()].pointPos;
						}

						if( Input.GetAxis("Mouse ScrollWheel") != 0 && connectionSelector.activeSelf)
						{
							if(Input.GetAxis("Mouse ScrollWheel") > 0) container.NextIndex();
							else container.PreviousIndex();
							if(container.CurrentIndex() != -1) connectionSelector.transform.position = container.connections[container.CurrentIndex()].pointPos;
						}
					}

					mouseOverLabel.gameObject.SetActive(true);
					mouseOverLabel.rectTransform.anchoredPosition = (Vector2)Input.mousePosition-30.0f*Vector2.up;
					mouseOverLabel.text = info.collider.gameObject.name;
					if(Input.GetMouseButtonDown(0)) 
					{
						if(cN.transform.parent == null)
						{
							container.Connect(cN);
						} 
						else 
						{
							container.Disconnect (cN);
						}
					}
				}
				else 
				{
					mouseOverLabel.gameObject.SetActive(false);
					connectionSelector.SetActive(false);
				}
			}
			else if(mouseOverLabel.gameObject.activeSelf)
			{
				mouseOverLabel.gameObject.SetActive(false);
				connectionSelector.SetActive(false);
			}

			if(Input.GetKeyDown(KeyCode.X))
			{
				container.Deactivate ();
				character.enabled = true;
				character.transform.parent = null;
				character.transform.position = container.transform.position - container.transform.forward * 2.25f;
				character.transform.rotation = container.transform.rotation;
				character.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				character.gameObject.GetComponent<Collider>().enabled = true;
				cF.target = character.gameObject;
				container = null;
				connectionSelector.SetActive(false);
			}
		}

	
	}
}
