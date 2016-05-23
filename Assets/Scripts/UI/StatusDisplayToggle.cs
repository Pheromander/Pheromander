using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent (typeof(Sidebar))]
public class StatusDisplayToggle : MonoBehaviour {
	[SerializeField]
	GameObject _agentStatusPanel;

	[SerializeField]
	GameObject _enemyStatusPanel;

	[SerializeField]
	GameObject _resourceStatusPanel;

	Sidebar _sidebar;

	int _selectableLayer;

	void Awake() {
		_sidebar = GetComponent<Sidebar> ();
		_selectableLayer = LayerMask.GetMask ("Selectable");
	}
	
	// Update is called once per frameysour
	void Update () {
		if (EventSystem.current.IsPointerOverGameObject ())
			return;

		if (Input.GetMouseButtonDown (0)) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit raycastHit;

			if (Physics.Raycast (camRay, out raycastHit, 1000.0f, _selectableLayer)) {
				GameObject gameObject = raycastHit.collider.transform.parent.gameObject;

				if (gameObject.CompareTag ("Ant")) {
					_agentStatusPanel.GetComponent<AntStatusDisplay> ().SetAgent (gameObject);
					_sidebar.ShowPanel (_agentStatusPanel);
				} else if (gameObject.CompareTag ("Resource")) {
					_resourceStatusPanel.GetComponent<ResourceStatusDisplay> ().SetResource (gameObject);
					_sidebar.ShowPanel (_resourceStatusPanel);
				} else if (gameObject.CompareTag ("Enemy")) {
					_enemyStatusPanel.GetComponent<EnemyStatusDisplay> ().SetEnemy (gameObject);
					_sidebar.ShowPanel (_enemyStatusPanel);
				}
			}
		}
	}
}
