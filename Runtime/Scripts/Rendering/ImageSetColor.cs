using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    [RequireComponent(typeof(Image))]
    public class ImageSetColor : MonoBehaviour
    {
        [SerializeField] private Color[] colors;

        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void SetColorIndex(int index)
        {
            image.color = colors[index];
        }
    }
}
