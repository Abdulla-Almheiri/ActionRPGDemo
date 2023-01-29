using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Chaos.Gameplay.Systems;
using UnityEngine.SceneManagement;

namespace Chaos.Gameplay.UI
{
    public class MainMenuController : MonoBehaviour
    {
        public string SceneToLoad;
        [field:SerializeField]
        public GameAudioController GameAudioController { private set; get; }
        private VisualElement _rootVisualElement;

        private Label _startGameLabel;
        private Label _settingsLabel;
        private Label _quitLabel;

        private VisualElement _quitSection;
        private Label _quitYesLabel;
        private Label _quitNoLabel;

        private VisualElement _settingsSection;
        private Slider _masterVolumeSlider;
        private Slider _soundEffectsVolumeSlider;
        private Slider _musicVolumeSlider;
        private Label _confirmButton;
        private List<VisualElement> _hoverableList;

        private Label _gameBuildLabel;


        void Start()
        {
            Initialize();
        }

        void Update()
        {

        }

        private void Initialize()
        {
            _rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

            _startGameLabel = _rootVisualElement.Q<Label>("StartGameLabel");
            _settingsLabel = _rootVisualElement.Q<Label>("SettingsLabel");
            _quitLabel = _rootVisualElement.Q<Label>("QuitLabel");

            _quitSection = _rootVisualElement.Q("QuitSection");
            _quitYesLabel = _rootVisualElement.Q<Label>("YesLabel");
            _quitNoLabel = _rootVisualElement.Q<Label>("NoLabel");

            _settingsSection = _rootVisualElement.Q("SettingsSection");
            _masterVolumeSlider = _rootVisualElement.Q<Slider>("MasterVolumeSlider");
            _soundEffectsVolumeSlider = _rootVisualElement.Q<Slider>("SoundEffectsVolumeSlider");
            _musicVolumeSlider = _rootVisualElement.Q<Slider>("MusicVolumeSlider");
            _confirmButton = _rootVisualElement.Q<Label>("ConfirmLabel");

            _masterVolumeSlider.value = GameAudioController.MasterVolume;
            _soundEffectsVolumeSlider.value = GameAudioController.SoundEffectsVolume;
            _musicVolumeSlider.value = GameAudioController.MusicVolume;


            _gameBuildLabel = _rootVisualElement.Q<Label>("GameBuildLabel");

            _hoverableList = _rootVisualElement.Query<VisualElement>(className: "main-menu-label").ToList();

            RegisterCallbacks();

            RegisterHoverableList();

        }

        private void RegisterHoverableList()
        {
            foreach (VisualElement element in _hoverableList)
            {
                element.RegisterCallback<MouseEnterEvent>(MouseHoverAllButtons);
            }
        }

        private void MouseHoverAllButtons(MouseEnterEvent evt)
        {
            GameAudioController.PlayUIHoverSound();
        }

        private void RegisterCallbacks()
        {
            _startGameLabel.RegisterCallback<ClickEvent>(StartGameClicked);
            _settingsLabel.RegisterCallback<ClickEvent>(SettingsClicked);
            _quitLabel.RegisterCallback<ClickEvent>(QuitClicked);

            _quitYesLabel.RegisterCallback<ClickEvent>(QuitYesPressed);
            _quitNoLabel.RegisterCallback<ClickEvent>(QuitNoPressed);

            _masterVolumeSlider.RegisterValueChangedCallback(MasterVolumeSliderChanged);
            _soundEffectsVolumeSlider.RegisterValueChangedCallback(SoundEffectsVolumeSliderChanged);
            _musicVolumeSlider.RegisterValueChangedCallback(MusicVolumeSliderChanged);

            _confirmButton.RegisterCallback<ClickEvent>(ConfirmButtonPressed);

        }

        private void ConfirmButtonPressed(ClickEvent evt)
        {
            GameAudioController.PlayUIConfirmSound();
            HideVisualElement(_settingsSection);
            ShowVisualElementOpacity(_settingsLabel);

        }

        private void MusicVolumeSliderChanged(ChangeEvent<float> evt)
        {
            //GameAudioController.PlayUIHoverSound();
            GameAudioController.SetMusicVolume(evt.newValue);
        }

        private void SoundEffectsVolumeSliderChanged(ChangeEvent<float> evt)
        {
            GameAudioController.PlayUIHoverSound();
            GameAudioController.SetSoundEffectsVolume(evt.newValue);
        }

        private void MasterVolumeSliderChanged(ChangeEvent<float> evt)
        {
            GameAudioController.PlayUIHoverSound();
            GameAudioController.SetMasterVolume(evt.newValue);
        }

        private void QuitNoPressed(ClickEvent evt)
        {
            HideVisualElement(_quitSection);
            ShowVisualElementOpacity(_quitLabel);
        }

        private void QuitYesPressed(ClickEvent evt)
        {
            GameAudioController.PlayUIConfirmSound();
            Application.Quit();
        }

        private void StartGameClicked(ClickEvent evt)
        {
            if(SceneToLoad != null)
            {
                GameAudioController.PlayUIConfirmSound();
                SceneManager.LoadScene(SceneToLoad);
            }
        }

        private void SettingsClicked(ClickEvent evt)
        {
            GameAudioController.PlayUIConfirmSound();
            HideVisualElementOpacity(_settingsLabel);
            ShowVisualElement(_settingsSection);
        }

        private void QuitClicked(ClickEvent evt)
        {
            GameAudioController.PlayUIConfirmSound();
            ShowVisualElement(_quitSection);
            HideVisualElementOpacity(_quitLabel);
            HideVisualElement(_settingsSection);
            ShowVisualElementOpacity(_settingsLabel);
        }
        private void ShowVisualElementOpacity(VisualElement element)
        {
            if (element == null)
            {
                return;
            }

            element.style.opacity = new StyleFloat(1f);
        }

        private void HideVisualElementOpacity(VisualElement element)
        {
            if (element == null)
            {
                return;
            }

            element.style.opacity = new StyleFloat(0f);
        }

        private void ShowVisualElement(VisualElement element)
        {
            if(element == null)
            {
                return;
            }

            element.style.display = DisplayStyle.Flex;
        }

        private void HideVisualElement(VisualElement element)
        {
            if (element == null)
            {
                return;
            }

            element.style.display = DisplayStyle.None;
        }
    }
}