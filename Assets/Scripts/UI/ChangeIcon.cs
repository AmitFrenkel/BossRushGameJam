using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite iconSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.sprite = iconSprite;
        icon.color = new Color(icon.color.r,icon.color.g,icon.color.b, 100);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        icon.color = new Color(icon.color.r,icon.color.g,icon.color.b, 0);
    }
}
