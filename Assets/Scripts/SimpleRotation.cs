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

    private Vector2 direction;

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
            pointerID = eventData.pointerId;
            touched = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == pointerID)
        {
            if (eventData.delta.y > thresholdY)
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
        if (eventData.pointerId == pointerID)
        {
            touched = false;
            pointerID = 0;
            direction = Vector2.zero;
        }
    }
}
