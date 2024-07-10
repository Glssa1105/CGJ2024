using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JianYing : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public UnityEvent OnPointerEnterEvent;
    public UnityEvent OnPointerExitEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color=Color.black;
        OnPointerEnterEvent?.Invoke();
        ;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.white;
        OnPointerExitEvent?.Invoke();
    }
}
