using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour
{
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    CameraRaycaster cameraRaycaster;

    void Start ()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChange; // registering
	}
	
	void OnLayerChange (int newLayer)
    {
        print("Cursor over new layer");
        switch (newLayer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                return;
        }        
	}

    // TODO consider deregistering OnLayerChanged on leaving all game scenes
}
