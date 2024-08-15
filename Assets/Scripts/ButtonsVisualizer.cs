using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsVisualizer : MonoBehaviour
{
    [SerializeField] private ImagesConfig config;
    [SerializeField] private Image background;
    [SerializeField] private GameObject buttonPrefab;

    public Action<string> SomeDataClick;
    private float w, h;
    private ImageConfig choosenImageData;

    void Start()
    {
        SomeDataClick += OnSomeDataClick;

        SetMainImage();

        SetScaleCoefitients();

        InstantiateButtons();

    }

    private void SetMainImage()
    {
        int randomImageIndex = UnityEngine.Random.Range(0, config.allImages.Count);
        choosenImageData = config.allImages[randomImageIndex];

        background.sprite = choosenImageData.main;
    }

    private void SetScaleCoefitients()
    {
        float curW = Screen.width;
        float curH = Screen.height;
        float targetW = background.sprite.texture.width;
        float targetH = background.sprite.texture.height;
        w = targetW / curW;
        h = targetH / curH;
    }

    private void InstantiateButtons()
    {
        foreach (var item in choosenImageData.buttons)
        {
            var instance = Instantiate(this.buttonPrefab, background.transform);

            // Set sprite
            instance.GetComponent<Image>().sprite = item.image;

            // Set position and size
            var rectTransform = instance.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector2(item.position.x / w, item.position.y / h);
            rectTransform.sizeDelta = item.isScalable ? new Vector2(item.size.x / w, item.size.y / h) :
                new Vector2(item.size.x / h, item.size.y / h);

            // Set button
            var button = instance.GetComponent<Button>();
            button.onClick.AddListener(() => { SomeDataClick?.Invoke(item.SomeUsefulData); });
        }
    }

    private void OnSomeDataClick(string data)
    {
        Debug.Log(data);
    }
}
