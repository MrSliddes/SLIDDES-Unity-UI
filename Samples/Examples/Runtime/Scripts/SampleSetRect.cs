using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLIDDES.UI;

namespace SLIDDES.UI.Samples
{
    public class SampleSetRect : MonoBehaviour
    {
        public Vector2 size = new Vector2(26, 26);
        public RectTransform rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            rectTransform.SetSize(size);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
