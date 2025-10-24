using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace DarkLordGame
{
    public class UI3DSwapItem : UI3D
    {
        private const int DefaultTarget = 0;
        private const int HoverTarget = 1;
        private const int SelectTarget = 2;
        private const int DisableTarget = 3;

        [SerializeField] private List<GameObject> targets;
        void Start()
        {
            SetTargetVisible(interactable ? DefaultTarget : DisableTarget);
        }

        public override void SetInteractable(bool _interactable)
        {
            base.SetInteractable(_interactable);
            SetTargetVisible(_interactable ? DefaultTarget : DisableTarget);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (interactable == false) return;
            base.OnPointerDown(eventData);
            SetTargetVisible(SelectTarget);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (interactable == false) return;
            base.OnPointerUp(eventData);
            if (eventData.pointerCurrentRaycast.gameObject == gameObject)
            {
                SetTargetVisible(HoverTarget);
            }
            else
                SetTargetVisible(DefaultTarget);

        }

        protected override void OnHovered(PointerEventData pointerEventData)
        {
            SetTargetVisible(HoverTarget);
            base.OnHovered(pointerEventData);
        }

        protected override void OnUnHovered(PointerEventData pointerEventData)
        {
            SetTargetVisible(DefaultTarget);
            base.OnUnHovered(pointerEventData);
        }

        private void SetTargetVisible(int layer)
        {
            for (int i = 0, length = targets.Count; i < length; i++)
            {
                targets[i].SetActive(i == layer);
            }
        }
    }
}
