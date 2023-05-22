using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public class MouseToWorld2D : MonoBehaviour
    {
        //Declarations
        [SerializeField] private Camera _cameraReferencePerspective;
        [SerializeField] private float _zOverride = 0;
        private Vector3 _mouseWorldPosition;
        public bool _isDebugActive = false;



        //Monobehaviors
        private void Update()
        {
            if (_isDebugActive)
                LogMouseWorldPosition();
        }



        //Utils
        public Vector3 GetWorldPosition()
        {
            _mouseWorldPosition = _cameraReferencePerspective.ScreenToWorldPoint(Input.mousePosition);
            _mouseWorldPosition.z = _zOverride;
            return _mouseWorldPosition;
        }

        public bool IsDebugActive()
        {
            return _isDebugActive;
        }

        public void SetDebug(bool newValue)
        {
            _isDebugActive = newValue;
        }

        private void LogMouseWorldPosition()
        {
            Debug.Log(GetWorldPosition());
        }



    }
}

