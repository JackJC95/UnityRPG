﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour
    {
        
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] Texture2D targetCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        // TODO solve fight between serialized and const
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;

        CameraRaycaster cameraRaycaster;

        void Start()
        {
            cameraRaycaster = GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyLayerChangeObservers += OnLayerChange; // registering
        }

        void OnLayerChange(int newLayer)
        {
            switch (newLayer)
            {
                case enemyLayerNumber:
                    Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                    return;
            }
        }
    }
}
