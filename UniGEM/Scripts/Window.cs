using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Window: MonoBehaviour {
    public delegate void OnHideDelegate();
    public OnHideDelegate OnHide;
    public Transform dragComponent;
    
    private bool isDragging = false;
    private RectTransform rectTransform;
    private RectTransform dragRectTransform;
    private RectTransform canvasRectTransform;
    private Canvas canvas;
    private Camera eventCamera;
    private Vector2 lastLocalMousePosition;

    private void Start() {
        rectTransform = gameObject.GetComponent<RectTransform>();
        if (!dragComponent.IsUnityNull())
            dragRectTransform = dragComponent.GetComponent<RectTransform>();
        canvas = rectTransform.GetComponentInParent<Canvas>();
        if (canvas != null) {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
            eventCamera = canvas.worldCamera;
        }
    }

    private void Update() {
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (IsClickingOnDragComponent()) {
                Show();
                isDragging = true;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Mouse.current.position.ReadValue(), eventCamera, out lastLocalMousePosition);
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame) {
            isDragging = false;
        }

        if (isDragging && Mouse.current.leftButton.isPressed) {
            UpdateDragPosition();
        }
    }

    private bool IsClickingOnDragComponent() {
        if (dragRectTransform == null) return false;
        if (!RectTransformUtility.RectangleContainsScreenPoint(dragRectTransform, Mouse.current.position.ReadValue(), eventCamera)) {
            return false;
        }
        
        // Check if this window is actually on top
        return IsWindowOnTop();
    }
    
    private bool IsWindowOnTop() {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current) {
            position = Mouse.current.position.ReadValue()
        };
        
        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (raycaster != null) {
            raycaster.Raycast(pointerEventData, results);
            
            if (results.Count > 0) {
                // Check if the topmost result belongs to this window
                GameObject topmost = results[0].gameObject;
                return IsChildOfWindow(topmost);
            }
        }
        
        return true; // If no raycaster, assume we're on top
    }
    
    private bool IsChildOfWindow(GameObject obj) {
        Transform current = obj.transform;
        while (current != null) {
            if (current == rectTransform) {
                return true;
            }
            current = current.parent;
        }
        return false;
    }
    
    private void UpdateDragPosition() {
        Vector2 currentLocalMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Mouse.current.position.ReadValue(), eventCamera, out currentLocalMousePosition);
        
        Vector2 delta = currentLocalMousePosition - lastLocalMousePosition;
        rectTransform.anchoredPosition += delta;
        lastLocalMousePosition = currentLocalMousePosition;
    }

    public void Hide(){
        gameObject.SetActive(false);
        if  (OnHide != null) {
            OnHide();
        }
    }

    public void Show(){
        gameObject.SetActive(true);
        // Bring window to the front by moving it to the end of the parent's children
        transform.SetAsLastSibling();
    }
}