using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private InputController inputController;

    private void Update() {
        transform.localPosition = new Vector3(inputController.MouseWorldPosition.x, inputController.MouseWorldPosition.y, 0);
    }
    
}
