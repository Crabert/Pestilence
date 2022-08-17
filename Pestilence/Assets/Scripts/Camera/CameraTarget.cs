using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Player player;
    bool _hover;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && player.cameraDestinationTransform != transform && _hover)
        {
            player.cameraDestinationTransform = transform;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        _hover = true;
        player.hovering = true;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _hover = false;
        player.hovering = false;
    }
}
