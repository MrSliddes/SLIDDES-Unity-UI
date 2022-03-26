using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SLIDDES.UI
{
    /// <summary>
    /// For creating a image slide show
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/Image Slide Show")]
    public class ImageSlideShow : MonoBehaviour
    {
        [SerializeField] private float imageStayDuration = 5f;
        [SerializeField] private float fadeSpeed = 1f;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Material materialBlurShader;
        [SerializeField] private ImageSettings imageSettings;

        private int currentIndex;
        private int nextIndex = 1;
        private Image image;
        private Image imageOverlay;
        private Image imageBlur;
        private Image imageBlurShader;

        private void Awake()
        {
            CreateImages();
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Fade());
        }

        private void CreateImages()
        {
            RectTransform a;

            // Image blur
            if(materialBlurShader != null)
            {
                a = new GameObject("[ImageSlideShow] Image Blur", typeof(Image)).GetComponent<RectTransform>();
                a.transform.SetParent(transform);
                a.anchorMin = Vector2.zero;
                a.anchorMax = Vector2.one;
                a.SetRect(0, 0, 0, 0);
                imageBlur = a.GetComponent<Image>();
                imageBlur.sprite = sprites[currentIndex];

                // Image blur shader
                a = new GameObject("[ImageSlideShow] Image Blur Shader", typeof(Image)).GetComponent<RectTransform>();
                a.transform.SetParent(transform);
                a.anchorMin = Vector2.zero;
                a.anchorMax = Vector2.one;
                a.SetRect(0, 0, 0, 0);
                imageBlurShader = a.GetComponent<Image>();
                imageBlurShader.material = materialBlurShader;
            }

            // Overlay
            a = new GameObject("[ImageSlideShow] Overlay Image", typeof(Image)).GetComponent<RectTransform>();
            a.transform.SetParent(transform);
            a.anchorMin = Vector2.zero;
            a.anchorMax = Vector2.one;
            a.SetRect(0, 0, 0, 0);
            imageOverlay = a.GetComponent<Image>();
            imageOverlay.sprite = sprites[currentIndex];
            imageOverlay.preserveAspect = imageSettings.preserveAspect;

            // Image
            a = new GameObject("[ImageSlideShow] Image", typeof(Image)).GetComponent<RectTransform>();
            a.transform.SetParent(transform);
            a.anchorMin = Vector2.zero;
            a.anchorMax = Vector2.one;
            a.SetRect(0, 0, 0, 0);
            image = a.GetComponent<Image>();
            image.sprite = sprites[currentIndex];
            image.preserveAspect = imageSettings.preserveAspect;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

            if(sprites.Length == 1) nextIndex = 0;
        }

        private IEnumerator Fade()
        {
            while(true)
            {
                yield return new WaitForSeconds(imageStayDuration);
                yield return FadeToNextSprite();
            }
        }

        private IEnumerator FadeToNextSprite()
        {
            // Set
            imageOverlay.sprite = sprites[currentIndex];
            image.sprite = sprites[nextIndex];
            imageOverlay.color = new Color(imageOverlay.color.r, imageOverlay.color.g, imageOverlay.color.b, 1);
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

            // Fade
            float alpha = 1;
            while(true)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                if(alpha < 0) alpha = 0;

                imageOverlay.color = new Color(imageOverlay.color.r, imageOverlay.color.g, imageOverlay.color.b, alpha);
                image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Abs(alpha - 1));

                if(alpha <= 0) break;
                yield return null;
            }

            // Next index
            imageOverlay.sprite = sprites[nextIndex];
            imageOverlay.color = new Color(imageOverlay.color.r, imageOverlay.color.g, imageOverlay.color.b, 1);

            currentIndex = nextIndex;
            nextIndex = nextIndex + 1 >= sprites.Length ? nextIndex = 0 : nextIndex += 1;

            image.sprite = sprites[nextIndex];
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

            imageBlur.sprite = imageOverlay.sprite;
            yield break;
        }

        [System.Serializable]
        public class ImageSettings
        {
            public bool preserveAspect;
        }
    }
}