using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace board
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D myBody;
        [SerializeField] private Animator anim;

        public Vector3 TargetPosition { get; set; }

        public event Action<Ball> OnBallDropFinished;

        public void PlayCollectAnim()
        {
            anim.Play("Collect");
        }

        public void Clear()
        {
            TargetPosition = Vector3.zero;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            myBody.bodyType = RigidbodyType2D.Static;
            transform.localPosition = TargetPosition;
            OnBallDropFinished?.Invoke(this);
        }
    }
}

