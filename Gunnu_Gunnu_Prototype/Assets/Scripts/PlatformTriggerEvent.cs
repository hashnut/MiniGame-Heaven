using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformTriggerEvent : MonoBehaviour
{
    public UnityEvent<Collider2D> onTriggerEnter;

    private void Start() 
    {
        if (onTriggerEnter == null)
        {
            onTriggerEnter = new UnityEvent<Collider2D>();
        }    
    }

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (onTriggerEnter != null)
        {
            onTriggerEnter.Invoke(other);
        }
    }

}
