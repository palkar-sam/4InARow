using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Vector2 MouseWorldPosition { get; private set; }

    public event Action<GameObject> OnClick;
    
    private bool _isActive;

    public void ActivateInput(bool flag, float interval = 0.0f)
    {
        if (interval > 0)
        {
            StartCoroutine(DoActivateInput(interval));
        }
        else
        {
            _isActive = flag;
        }
    }

    private IEnumerator DoActivateInput(float interval)
    {
        yield return new WaitForSeconds(interval);

        _isActive = true;
    }

    private void Update()
    {
        MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_isActive && Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(MouseWorldPosition, Vector2.up);

            if (hit2D.collider != null)
            {
                OnClick?.Invoke(hit2D.collider.gameObject);
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Quit the application
                Application.Quit();
            }
        }
    }
}
