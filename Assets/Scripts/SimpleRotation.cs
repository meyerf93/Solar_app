using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleRotation : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    public ReactionCollection bousole_reaction;
    public ReactionCollection building_reaction;
    public float smoothing;
    public float leng_max;
    public float max_rotate;

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;

    private bool touched;
    private int pointerID;



    private void Awake()
    {
        direction = Vector2.zero;
        touched = false;
    }

    public void OnPointerDown (PointerEventData data)
    {   //set iour start point
        if (!touched)
        {
            touched = true;
            pointerID = data.pointerId;
            origin = data.position;
        }
    }

    public void OnDrag(PointerEventData data)
    {   //compare the difference betwen our start and current pointer pos
        if(data.pointerId == pointerID)
        {
            Vector2 currentPosition = data.position;
            Vector2 directionRaw;
            Debug.Log("origin value : " + origin.x + " ; current position : " + currentPosition.x);
            if (currentPosition.x >= 0)
                directionRaw.x = currentPosition.x - origin.x;
            else directionRaw.x = origin.x - currentPosition.x;
            direction.x = directionRaw.x / leng_max * max_rotate;
            direction.y = 0;
            Debug.Log("direction of rotation zone : " + direction);
            bousole_reaction.React();
            building_reaction.React();
        }
    }

    public void OnPointerUp(PointerEventData data)
    {   // Reset Everything
        if(data.pointerId == pointerID)
        {
            direction = Vector2.zero;
            touched = false;
        }
    }

    public Vector2 GetDirection()
    {
        smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing);
        return smoothDirection;
    }
}
