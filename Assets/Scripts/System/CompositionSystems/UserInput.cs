using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[System.Serializable]
public class PointerData {
    public Vector3 pointerFirstPosition;
    public Vector3 pointerNextPosition;
    public Vector3 pointerPreviousPosition;
    public Vector3 pointerDelta;
    public Vector3 pointerFirstDelta;
    public Vector3 pointerScrollDelta;
    public Vector3 pointerScale;
}

public class UserInput : MonoBehaviour
{
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static UserInput _instance = null;
    private static bool m_ShuttingDown = false;
 
    // A static property that finds or creates an instance of the manager object and returns it.
    public static UserInput Instance
    {

        get
        {
            if (m_ShuttingDown)
            {
                // Debug.LogWarning("EventSystem Instance " +
                //     "already destroyed. Returning null.");
                return null;
            }
            if (_instance == null)
            {
                // FindObjectOfType() returns the first AManager object in the scene.
                _instance = FindObjectOfType(typeof(UserInput)) as UserInput;
            }
            // If it is still null, create a new instance
            if (_instance == null)
            {
                Debug.LogWarning("UserInput INSTANTIATED!");

                var obj = Resources.Load("UserInput/UserInput", typeof(GameObject));
                GameObject inst = Instantiate(obj) as GameObject;
                inst.name = obj.name;
                UserInput.Instance.Init();
            }
            return _instance;
        }
    }
 
    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnDestroy()
    {
        m_ShuttingDown = true;
        _instance = null;
        Debug.LogWarning("UserInput Singleton DESTROYED!");
    }
    //----------------------------------------------------------------------------------------------------------
    [Header("Pointer parameters")]
    // public GameObject debugObject;
    public baseEvent eventAppQuit;

    private bool clickWasDown = false;
    private bool wasDrag = false;

    public PointerData pointerData = new PointerData();
	private List<RaycastResult> uiClickStartResult = new List<RaycastResult>();
    [SerializeField]
    private GraphicRaycaster _raycaster;
    private GraphicRaycaster raycaster {
        get {
            if(_raycaster == null) {
                _raycaster = FindObjectOfType<GraphicRaycaster>();
            }
            return _raycaster;
        }
    }

    public CanvasScaler _canvasScaler;
    public CanvasScaler canvasScaler {
        get {
            if(_canvasScaler == null) {
                _canvasScaler = FindObjectOfType<CanvasScaler>();
                
            }
            return _canvasScaler;
        }
    }
    private PointerEventData pointerEventData;
    private EventSystem _eventSystem;
    private EventSystem eventSystem {
        get {
            if(_eventSystem == null) {
                _eventSystem = FindObjectOfType<EventSystem>();
            }
            return _eventSystem;
        }
    }

    private RaycastHit startHit, endHit;
    [SerializeField]
    // private Camera gameCam;
    public bool ui = false;
    public bool game = false;

    public void Ping() {
        
    }

    public void Init() {
        pointerData.pointerScale = new Vector3(canvasScaler.referenceResolution.x/Screen.width, canvasScaler.referenceResolution.x/Screen.width, 0f);
    }

    // public void SetActive(bool state) {
	// 	if(state) {
	// 		active = true;
	// 	}
	// 	if(!state) {
	// 		active = false;
	// 	}
	// }

    private void CalculatePositionStart() {
        pointerData.pointerFirstPosition = Input.mousePosition;
    }

    private void ResetPointer() {
        pointerData.pointerFirstPosition = Vector3.zero;
        pointerData.pointerNextPosition = Vector3.zero;
        pointerData.pointerPreviousPosition = Vector3.zero;
        pointerData.pointerDelta = Vector3.zero;
    }

    private void GetFirstDrag() {
        if(wasDrag)
            return;
        if(pointerData.pointerFirstPosition != Vector3.zero) {
            pointerData.pointerFirstDelta = pointerData.pointerNextPosition - pointerData.pointerFirstPosition;
            if(pointerData.pointerFirstDelta.sqrMagnitude >= eventSystem.pixelDragThreshold)
                wasDrag = true;
        }
    }

    private void CalculatePositionNext() {
        pointerData.pointerNextPosition = Input.mousePosition;
    }

    private void CalculatePreviousPos() {
        pointerData.pointerPreviousPosition = pointerData.pointerNextPosition;
    }

    private void CalculatePositionDelta() { 
        CalculatePositionNext();
        if(pointerData.pointerPreviousPosition != Vector3.zero) {
            pointerData.pointerDelta = pointerData.pointerNextPosition - pointerData.pointerPreviousPosition;
        }
        else {
            pointerData.pointerDelta = Vector3.zero;
        }
        CalculatePreviousPos();
    }

	bool areEqualTargets(RaycastResult res, RaycastResult prevres) {
        // Debug.Log(res.isValid + " and " + prevres.isValid + " and " + res.gameObject.transform.parent.name + " and " + prevres.gameObject.transform.parent.name);
		return (
			(res.isValid && prevres.isValid) && 
			(res.gameObject.transform.parent.name == prevres.gameObject.transform.parent.name)			
			);
	}

    List<RaycastResult> newRaycast() {
		//Set up the new Pointer Event
		pointerEventData = new PointerEventData(eventSystem);
		//Set the Pointer Event Position to that of the mouse position
		pointerEventData.position = Input.mousePosition;
		//Create a list of Raycast Results
		List<RaycastResult> results = new List<RaycastResult>();

		//Raycast using the Graphics Raycaster and mouse click position
		raycaster.Raycast(pointerEventData, results);
		return results;
	}

    // Update is called once per frame
    void Update()
    {
        if(ui || game) {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape)) {
                // Quit the application
                eventAppQuit.Raise();
            }
        }

        if(Input.GetMouseButtonUp(0) && clickWasDown) {
            ResetPointer();
            clickWasDown = false;
            wasDrag = false;
            if(ui) {
                List<RaycastResult> raycastList = newRaycast();
                // Debug.Log(" ---------------------Pointer UP: " + uiClickStartResult.Count);
                if (raycastList.Count > 0)
                {
                    for(int i = 0; i < raycastList.Count; i++) {
                        for(int j = 0; j < uiClickStartResult.Count; j++) {
                            if(areEqualTargets(raycastList[i], uiClickStartResult[j])) {
                                UserInputComponent res = raycastList[i].gameObject.GetComponent<UserInputComponent>();
                                if(res == null)
                                    res = raycastList[i].gameObject.transform.parent.GetComponent<UserInputComponent>();
                                res.ClickUp();
                                Debug.Log("Hit done!: " + res.name);
                            }
                        }
                    }
                }
                for(int j = 0; j < uiClickStartResult.Count; j++) {
                    UserInputComponent res = uiClickStartResult[j].gameObject.GetComponent<UserInputComponent>();
                    if(res == null)
                        res = uiClickStartResult[j].gameObject.transform.parent.GetComponent<UserInputComponent>();
                    res.DragOff();
                }
                uiClickStartResult.Clear();
            }
            if(game) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out endHit, 100)) {
                    if(startHit.transform == endHit.transform) {
                        UserInputComponent res = endHit.transform.gameObject.GetComponent<UserInputComponent>();
                        res.ClickUp();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) {
            clickWasDown = true;
            CalculatePositionStart();
            if(ui) {
                List<RaycastResult> raycastList = newRaycast();
                // Debug.Log("--------------------- Pointer Down : " + uiClickStartResult.Count);
                if (raycastList.Count > 0)
                {
                    for(int i = 0; i < raycastList.Count; i++) {
                        UserInputComponent res = raycastList[i].gameObject.GetComponent<UserInputComponent>();
                        if(res == null) {
                            res = raycastList[i].gameObject.transform.parent.GetComponent<UserInputComponent>();
                        }
                            uiClickStartResult.Add(raycastList[i]);
                            res.ClickDown();
                            Debug.Log("Hit start!: " + res.name);
                    }
                }
            }
            if(game) {
                startHit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out startHit, 100)) {
                    UserInputComponent res = startHit.transform.gameObject.GetComponent<UserInputComponent>();
                    Debug.Log(startHit.transform.gameObject);
                    res.ClickDown();
                }
            }
        }
        else if(clickWasDown && wasDrag) {
                if(ui) {
                    if (uiClickStartResult.Count > 0)
                    {
                        for(int i = 0; i < uiClickStartResult.Count; i++) {
                            UserInputComponent res = uiClickStartResult[i].gameObject.GetComponent<UserInputComponent>();
                            if(res == null)
                                res = uiClickStartResult[i].gameObject.transform.parent.GetComponent<UserInputComponent>();
                            // res.SetPointerData(pointerData);
                            if(res.draggable) {
                                res.Drag();
                                Debug.Log("Drag of: " + uiClickStartResult[i].gameObject.name);
                            }
                            // uiClickStartResult.Clear();
                        }
                    }
                }
                if(game) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    UserInputComponent res;
                    RaycastHit nextHit = new RaycastHit();

                    if (Physics.Raycast(ray, out nextHit, 100) && (nextHit.transform == startHit.transform)) {
                        res = startHit.transform.gameObject.GetComponent<UserInputComponent>();
                        res.Drag();
                    }
                }
        }
        else {
            GetFirstDrag();
        }   
        CalculatePositionDelta();
                   
    }
}
