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
        int randomImageIndex = UnityEngine.Random.Range(0, config.allImages.Count);
        choosenImageData = config.allImages[randomImageIndex];

        SetMainImage();

        SetScaleCoefitients();

        InstantiateButtons();

    }

    private void SetMainImage()
    {
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
            button.onClick.AddListener(() => { SomeDataClick?.Invoke(item.title); });

            // Set effects
            var effectsController = instance.GetComponent<ButtonEffectsController>();
            effectsController.SetImage(item.image);
            effectsController.SetText(item.title);
            effectsController.SetEffects(item.effects);
        }
    }

    private void OnSomeDataClick(string data)
    {
        Debug.Log(data);
    }

    public void SetNextImage()
    {
        int currentIndex = config.allImages.IndexOf(choosenImageData);
        int nextIndex = (currentIndex + 1) % config.allImages.Count;
        choosenImageData = config.allImages[nextIndex];

        DestroyOldButtons();
        SetMainImage();
        SetScaleCoefitients();
        InstantiateButtons();
    }

    public void SetPreviousImage()
    {
        int currentIndex = config.allImages.IndexOf(choosenImageData);
        int previousIndex = (currentIndex - 1 + config.allImages.Count) % config.allImages.Count;
        choosenImageData = config.allImages[previousIndex];

        DestroyOldButtons();
        SetMainImage();
        SetScaleCoefitients();
        InstantiateButtons();
    }

    private void DestroyOldButtons()
    {
        for (int i = background.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = background.transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }
}
