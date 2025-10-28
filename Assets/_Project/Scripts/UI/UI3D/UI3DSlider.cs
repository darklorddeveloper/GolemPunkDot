using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DarkLordGame
{
    public class UI3DSlider : UI3D, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Transform graphicsRoot;
        [SerializeField] private float transitionPeriod = 0.2f;
        [SerializeField] private float rootHeightWhenInteract;
        [SerializeField] private float minX = -0.2f, maxX = 0.2f;
        [SerializeField] private Vector2 dragWeight = new Vector2(0.01f, 0.0f);

        private const int DefaultTarget = 0;
        private const int HoverTarget = 1;
        private const int SelectTarget = 2;

        [SerializeField] private List<GameObject> targets;

        public float Value
        {
            get
            {
                return currentValue;

            }
            set
            {
                SetValueWithoutNotify(value);
                onValueChanged?.Invoke(currentValue);
            }
        }
        private float currentValue;

        private Coroutine runningCoroutin;
        private Vector2 startPosition;
        private Vector3 localStartPos;
        public Action<float> onValueChanged;

        void Start()
        {
            SetTargetVisible(DefaultTarget);

        }

        public void SetValueWithoutNotify(float value)
        {
            currentValue = value;
            UpdateGraphicsWithWeight();
        }

        private void SetHeight(float height)
        {
            if (runningCoroutin != null)
            {
                StopCoroutine(runningCoroutin);
            }
            runningCoroutin = StartCoroutine(HeightTransitionEnumerator(height));
        }

        private IEnumerator HeightTransitionEnumerator(float height)
        {
            float t = 0;
            while (t < transitionPeriod)
            {
                var pos = graphicsRoot.localPosition;
                pos.y = Mathf.Lerp(pos.y, height, t / transitionPeriod);
                graphicsRoot.localPosition = pos;
                t += Time.deltaTime;
                yield return null;
            }

            var pos2 = graphicsRoot.localPosition;
            pos2.y = height;
            graphicsRoot.localPosition = pos2;
            runningCoroutin = null;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (interactable == false) return;
            base.OnPointerDown(eventData);
            UpdateLocalStartPos(eventData.position);
            SetHeight(rootHeightWhenInteract);
            SetTargetVisible(SelectTarget);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (interactable == false) return;
            base.OnPointerUp(eventData);
            SetHeight(0);
            SetTargetVisible(HoverTarget);
        }

        protected override void OnHovered(PointerEventData pointerEventData)
        {
            base.OnHovered(pointerEventData);
            SetTargetVisible(HoverTarget);
        }

        protected override void OnUnHovered(PointerEventData pointerEventData)
        {
            base.OnUnHovered(pointerEventData);
            SetTargetVisible(DefaultTarget);
        }

        private void UpdateLocalStartPos(Vector2 mousePos)
        {
            startPosition = mousePos;
            localStartPos = transform.localPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            UpdateLocalStartPos(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var pos = eventData.position;
            var delta = pos - startPosition;
            var deltaPos = new Vector3(delta.x * dragWeight.x, 0, delta.y * dragWeight.y);

            var pos2 = transform.localPosition;
            pos2.x = Mathf.Clamp(localStartPos.x + deltaPos.x, minX, maxX);

            transform.localPosition = pos2;

            currentValue = (pos2.x - minX) / (maxX - minX);
            onValueChanged?.Invoke(currentValue);
        }

        private void UpdateGraphicsWithWeight()
        {
            var pos2 = graphicsRoot.localPosition;
            currentValue = Mathf.Clamp01(currentValue);
            pos2.x = currentValue * (maxX - minX) + minX;
            graphicsRoot.localPosition = pos2;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
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
