using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DarkLordGame
{
    public class UI3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public enum InteractorDirection
        {
            Up,
            Down,
            Left,
            Right,
        }

        [System.Serializable]
        public class InteractorNavigationData
        {
            public InteractorDirection Direction;
            public UI3D target;
        }
        public bool interactable = true;
        public static UI3D HoveringInstance { get; protected set; }
        public static UI3D FocusInstance { get; protected set; }
        public List<InteractorNavigationData> navigationDatas = new();

        public Action<UI3D> onHovered;
        public Action<UI3D> onUnHovered;
        public Action<UI3D> onClicked;

        public virtual void SetInteractable(bool _interactable)
        {
            interactable = _interactable;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (interactable == false) return;
            if (HoveringInstance == this)
            {
                return;
            }

            HoveringInstance = this;
            OnHovered(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (interactable == false) return;
            if (HoveringInstance == this)
            {
                HoveringInstance = null;
                OnUnHovered(eventData);
            }
        }

        protected virtual void OnHovered(PointerEventData pointerEventData)
        {
            //do something
            onHovered?.Invoke(this);
        }

        protected virtual void OnUnHovered(PointerEventData pointerEventData)
        {
            onUnHovered?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactable == false) return;
            OnClicked(eventData);
        }

        protected virtual void OnClicked(PointerEventData eventData)
        {
            onClicked?.Invoke(this);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}
