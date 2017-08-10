using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SimpleRotation : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{ 
    public ReactionCollection building_reaction;
    public ReactionCollection bousole_reaction;

    public float leng_max;
    public float max_rotate;
    public float speed;
    public float thresholdY;

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;

    private bool touched;
    private int pointerID;

    public Vector2 GetDirection()
    {
        //smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing);
        return direction; //smoothDirection;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!touched)
        {
            origin = eventData.pressPosition;
            pointerID = eventData.pointerId;
            touched = true;
            //Debug.Log("origin of click : " + origin);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.pointerId == pointerID)
        {
            if(eventData.delta.y > thresholdY)
            {
                direction.x = (eventData.delta.x / leng_max * max_rotate) * speed;
                direction.y = 0;
            }
            else
            {
                direction.x = (eventData.delta.x / leng_max * max_rotate) * speed;
                direction.y = 0;
            }

            bousole_reaction.React();
            building_reaction.React();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.pointerId == pointerID)
        {
            touched = false;
            origin = eventData.position;
            pointerID = 0;
            direction = Vector2.zero;
            //Debug.Log("Release position : " + origin);
        }
    }

    /*
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPosition = eventData.position;
        Vector2 directionRaw;
      

        if (currentPosition.x >= 0)
            directionRaw.x = currentPosition.x - origin.x;
        else directionRaw.x = origin.x - currentPosition.x;

        direction.x = directionRaw.x / leng_max * max_rotate;
        direction.y = 0;

        Debug.Log("direction of rotation zone : " + direction);
        bousole_reaction.React();
        building_reaction.React();
    }*/
}
