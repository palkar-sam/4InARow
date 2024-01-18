using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Vector2 MouseWorldPosition { get; private set; }

    public event Action<GameObject> OnClick;

    private IScreenPointProvider screenPointProvider;
    private IRayCastHitDetector rayCastHitDetector;
    private IApplicationDetector applicationDetector;

    private bool _isActive;

    private void Awake() 
    {
        screenPointProvider = GetComponent<IScreenPointProvider>();
        rayCastHitDetector = GetComponent<IRayCastHitDetector>();
        applicationDetector = GetComponent<IApplicationDetector>();
    }

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
        MouseWorldPosition = screenPointProvider.ScreenToWorldPoint(Input.mousePosition);

        if (_isActive && Input.GetMouseButtonUp(0))
            OnClick?.Invoke(rayCastHitDetector.GetHit(MouseWorldPosition, Vector2.up));

        applicationDetector.Quit(Application.platform);
    }
}



