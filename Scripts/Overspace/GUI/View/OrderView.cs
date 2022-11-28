using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Overspace.Fluid;
using UnityEngine;
using UnityEngine.UI;

namespace Overspace.GUI.View
{
    public class OrderView : MonoBehaviour
    {
        public List<Image> orderIcons = new();
        public List<Image> orderCheckIcons = new();
        
        public void SetOrderIcon(int index, Sprite icon)
        {
            Image target = orderIcons.ElementAtOrDefault(index);

            if (target)
            {
                target.sprite = icon;
                target.gameObject.SetActive(true);
            }
        }

        public bool CheckOrderIconState(int index)
        {
            Image target = orderCheckIcons.ElementAtOrDefault(index);
            return target != null && target.gameObject.activeSelf;
        }

        public void SetCheck(int index, bool state)
        {
            Image target = orderCheckIcons.ElementAtOrDefault(index);
            if (target)
            {
                target.gameObject.SetActive(state);
                if (state) StartCoroutine(PlayCheckAnim(target));
            }
        }

        public IEnumerator PlayCheckAnim(Image target)
        {
            Vector3 defaultPos = target.rectTransform.position; 
            
            target.rectTransform.position = UnityEngine.Camera.main.WorldToScreenPoint(FluidManager.Instance.pourPoint.position);
            target.rectTransform.localScale = Vector3.one * 3f;

            while (target.rectTransform.position != defaultPos || target.rectTransform.localScale != Vector3.one)
            {
                target.rectTransform.localScale = Vector3.MoveTowards(target.rectTransform.localScale, Vector3.one, Time.deltaTime * 7.5f);
                target.rectTransform.position = Vector3.MoveTowards(target.rectTransform.position, defaultPos, Time.deltaTime * 2250f);
                yield return null;
            }
            
            yield return null;
        }

        public void Clear()
        {
            foreach (Image orderIcon in orderIcons)
            {
                orderIcon.sprite = null;
                orderIcon.gameObject.SetActive(false);
            }

            foreach (Image orderCheckIcon in orderCheckIcons)
            {
                orderCheckIcon.gameObject.SetActive(false);
            }
            
            StopAllCoroutines();
        }
    }
}