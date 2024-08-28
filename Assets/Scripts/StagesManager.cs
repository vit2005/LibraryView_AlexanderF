using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StagesManager : MonoBehaviour
{
    [SerializeField] private StagesConfig config;
    [SerializeField] private Image background;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private StageEffectsController stageEffectsController;
    [SerializeField] private List<ToolbarElementType> toolbarButtons;

    private float w, h;
    private StageConfig choosenImageData;
    private List<(ActorData, ActorInteractionsHandler)> buttonInstances = new List<(ActorData, ActorInteractionsHandler)> ();

    public void Init(StagesConfig config)
    {
        this.config = config;
        InitToolbar();
        int randomImageIndex = UnityEngine.Random.Range(0, config.allStages.Count);
        choosenImageData = config.allStages[randomImageIndex];

        SetMainImageAndSound();

        SetScaleCoefitients();

        InstantiateButtons();

    }

    private void InitToolbar()
    {
        var buttons = ToolbarController.Instance.Show(toolbarButtons);
        ((SimpleToolbarButton)buttons[ToolbarElementType.PrevBtn]).SubscribeClick((string _) => SetPreviousImage());
        //((SimpleToolbarButton)buttons[ToolbarElementType.NextBtn]).SubscribeClick((string _) => SetNextImage());
        ((SimpleToolbarButton)buttons[ToolbarElementType.AutoNextBtn]).SubscribeClick((string _) => SetNextImage());
        ((ToolbarSoundPicker)buttons[ToolbarElementType.SoundPicker]).SubscribeSoundTypeChange((SoundType type) => { Debug.Log(type); })  ;
    }

    private void SetMainImageAndSound()
    {
        background.sprite = choosenImageData.main;
        backgroundAudio.clip = choosenImageData.music;
        backgroundAudio.Play();
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
        buttonInstances.Clear();
        stageEffectsController.Clear();

        foreach (var item in choosenImageData.actors)
        {
            var instance = Instantiate(this.buttonPrefab, background.transform);

            // Set sprite
            instance.GetComponent<Image>().sprite = item.image;

            // Set position and size
            var rectTransform = instance.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector2(item.position.x / w, item.position.y / h);
            rectTransform.sizeDelta = item.isScalable ? new Vector2(item.size.x / w, item.size.y / h) :
                new Vector2(item.size.x / h, item.size.y / h);

            // Set button interactables
            var button = instance.GetComponent<ActorInteractionsHandler>();
            button.Init(item.title, item.clickSound != null, item.longPressSound != null, item.isDragable);
            buttonInstances.Add((item, button));

            // Set button sounds
            var sounds = instance.GetComponent<ActorSoundsHandler>();
            sounds.Init(item.clickSound, item.longPressSound);

            // Set effects
            var effectsController = instance.GetComponent<ActorEffectsController>();
            effectsController.SetImage(item.image);
            effectsController.SetText(item.title);
            effectsController.SetEffects(item.effects);
        }

        stageEffectsController.CheckOnStageEffects(buttonInstances);
    }

    private void LogClick(string data)
    {
        Debug.Log(data);
    }

    public void SetNextImage()
    {
        int currentIndex = config.allStages.IndexOf(choosenImageData);
        int nextIndex = (currentIndex + 1) % config.allStages.Count;
        choosenImageData = config.allStages[nextIndex];

        DestroyOldButtons();
        SetMainImageAndSound();
        SetScaleCoefitients();
        InstantiateButtons();
    }

    public void SetPreviousImage()
    {
        int currentIndex = config.allStages.IndexOf(choosenImageData);
        int previousIndex = (currentIndex - 1 + config.allStages.Count) % config.allStages.Count;
        choosenImageData = config.allStages[previousIndex];

        DestroyOldButtons();
        SetMainImageAndSound();
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
