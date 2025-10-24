using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DarkLordGame
{
    public class UI3DFlagToggle : UI3DToggle
    {

        private const int DefaultTarget = 0;
        private const int HoverTarget = 1;
        private const int SelectTarget = 2;

        [SerializeField] private List<GameObject> targets;
        [SerializeField] private List<GameObject> targetsBase;
        [SerializeField] private Transform moveRoot;
        [SerializeField] private AnimationCurve onCurve;
        [SerializeField] private AnimationCurve offCurve;
        [SerializeField] private float period = 0.5f;

        private float currentTimeCount = -100;

        void Start()
        {
            ResetartAnimation();
            SetTargetVisible(DefaultTarget);
        }

        public override void SetToggleWithoutNotify(bool on)
        {
            if (on != isOn || currentTimeCount < 0)
                ResetartAnimation();
            base.SetToggleWithoutNotify(on);
            SetTargetVisible(DefaultTarget);
        }

        public void ResetartAnimation()
        {
            currentTimeCount = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentTimeCount >= period)
            {
                return;
            }
            currentTimeCount += Time.deltaTime;
            AnimationCurve targetCurve = isOn ? onCurve : offCurve;
            float height = targetCurve.Evaluate(currentTimeCount);
            moveRoot.localPosition = new Vector3(0, height, 0);
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
        protected override void OnClicked(PointerEventData eventData)
        {
            Toggle();
            base.OnClicked(eventData);
        }

        private void SetTargetVisible(int layer)
        {
            layer = isOn ? SelectTarget : layer;
            
            for (int i = 0, length = targets.Count; i < length; i++)
            {
                targets[i].SetActive(i == layer);
            }

            for (int i = 0, length = targetsBase.Count; i < length; i++)
            {
                targetsBase[i].SetActive(i == layer);
            }
        }
    }
}
