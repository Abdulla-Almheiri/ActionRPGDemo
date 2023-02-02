using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Chaos.Gameplay.Systems;
using Chaos.Gameplay.Characters;
using Chaos.Gameplay.Skills;
using System.Linq;

namespace Chaos.Gameplay.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField]
        private GameAudioController GameAudioController;
        [SerializeField]
        private GamePlayerController _gamePlayerController;
        private CharacterMovementController _playerMovementController;
        private CharacterSkillController _playerSkillController;
        private CharacterCombatController _playerCombatController;

        private VisualElement _rootVisualElement;
        private VisualElement _pauseMenu;

        private VisualElement _resumeButton;
        private VisualElement _settingsButton;
        private VisualElement _restartGameButton;
        private VisualElement _exitGameButton;

        private VisualElement _skillButton1;
        private VisualElement _skillButton2;
        private VisualElement _skillButton3;
        private VisualElement _skillButton4;


        private VisualElement _tooltip;
        private Label _tooltipSkillTitle;
        private Label _tooltipSkillDescription;
        private Label _tooltipSkillEnergyCost;
        private Label _tooltipSkillRecharge;

        private float _baseWidthValue = 40f;

        private VisualElement _healthBar;
        private VisualElement _energyBar;

        private Slider _masterVolumeSlider;
        private Slider _audioVolumeSlider;
        private Slider _musicVolumeSlider;

        private VisualElement _settingsMenu;
        private Button _confirmButton;


        private Label _messageLabel;
        private VisualElement _messageSection;
        private Color _defaultColor = Color.white;
        private VisualElement _topSection;

        private VisualElement _restartSection;
        private Button _deathRestartButton;
        private PlayerUIMessage _currentPlayerUIMessage;


        private List<VisualElement> _hoverableList;

        void Start()
        {
            Initialize();
        }

        void Update()
        {

            ProcessPauseAndSettingsMenu();
            ProcessAbilitiesRecharge();
            ProcessHealthAndEnergyBars();
        }

        private void ProcessPauseAndSettingsMenu()
        {
            if (Input.GetKeyUp(KeyCode.Escape) == true)
            {
                _playerMovementController.StopMovement();

                if (IsVisualElementActive(_pauseMenu) == false)
                {
                    if (IsVisualElementActive(_settingsMenu) == false)
                    {
                        ShowVisualElement(_pauseMenu);
                        GameAudioController.PlayUIWindowOpenSound();
                    }
                    else
                    {
                        HideVisualElement(_settingsMenu);
                        GameAudioController.PlayUICancelSound();
                    }

                }
                else
                {
                    HideVisualElement(_pauseMenu);
                    GameAudioController.PlayUICancelSound();
                }
            }
        }
        private void ProcessHealthAndEnergyBars()
        {
            var healthValue = _playerCombatController.GetHealthPercentage() / 100f;
            var energyValue = _playerCombatController.GetEnergyPercentage() / 100f;

           _healthBar.style.scale = new StyleScale(new Scale(new Vector3(healthValue, 1f, 1f)));
            _energyBar.style.scale = new StyleScale(new Scale(new Vector3(energyValue, 1f, 1f)));

        }

        private void Initialize()
        {

            _rootVisualElement = GetComponent<UIDocument>().rootVisualElement;


            _pauseMenu = _rootVisualElement.Q("PauseMenu");
            _pauseMenu.style.display = DisplayStyle.None;

            _playerMovementController = _gamePlayerController.PlayerMovementController;
            _playerSkillController = _gamePlayerController.PlayerMovementController.gameObject.GetComponent<CharacterSkillController>();
            _playerCombatController = _gamePlayerController.PlayerMovementController.gameObject.GetComponent<CharacterCombatController>();


            _resumeButton = _rootVisualElement.Q("ResumeButton");


            _settingsButton = _rootVisualElement.Q<Button>("SettingsButton");


            _exitGameButton = _rootVisualElement.Q<Button>("ExitGameButton");
            _restartGameButton = _rootVisualElement.Q<Button>("RestartGameButton");

            _tooltip = _rootVisualElement.Q("Tooltip");
            RegisterCallbackPauseMenuButtons();

            _skillButton1 = _rootVisualElement.Q("SkillButton1");
            _skillButton2 = _rootVisualElement.Q("SkillButton2");
            _skillButton3 = _rootVisualElement.Q("SkillButton3");
            _skillButton4 = _rootVisualElement.Q("SkillButton4");

            _tooltip = _rootVisualElement.Q("Tooltip");
            _tooltipSkillTitle = _rootVisualElement.Q<Label>("TooltipTitle");
            _tooltipSkillDescription = _rootVisualElement.Q<Label>("TooltipDescription");
            _tooltipSkillEnergyCost = _rootVisualElement.Q<Label>("EnergyCost");
            _tooltipSkillRecharge = _rootVisualElement.Q<Label>("RechargeTime");

            _healthBar = _rootVisualElement.Q("HealthBar");
            _energyBar = _rootVisualElement.Q("EnergyBar");

            _masterVolumeSlider = _rootVisualElement.Q<Slider>("MasterSlider");
            _audioVolumeSlider = _rootVisualElement.Q<Slider>("SFXSlider");
            _musicVolumeSlider = _rootVisualElement.Q<Slider>("MusicSlider");

            _masterVolumeSlider.value = GameAudioController.MasterVolume;
            _audioVolumeSlider.value = GameAudioController.SoundEffectsVolume;
            _musicVolumeSlider.value = GameAudioController.MusicVolume;

            _confirmButton = _rootVisualElement.Q<Button>("ExitSettingsButton");
            _settingsMenu = _rootVisualElement.Q("SettingsMenu");

            _settingsMenu.style.display = DisplayStyle.None;

            _messageLabel = _rootVisualElement.Q<Label>("MessageLabel");
            _messageSection = _rootVisualElement.Q("MessageSection");
            _topSection = _rootVisualElement.Q("TopSection");

            _restartSection = _rootVisualElement.Q("RestartSection");
            _deathRestartButton = _rootVisualElement.Q<Button>("DeathRestartButton");


            _hoverableList = _rootVisualElement.Query<VisualElement>(className: "pause-menu-button").ToList();

            ShowVisualElement(_topSection);
            ShowVisualElement(_messageSection);
            HideVisualElement(_messageLabel);

            RegisterCallbackSkillButtons();

            RegisterSettingsSlidersCallbacks();


            RegisterCallbackConfirmButton();
            RegisterCallbackPauseMenuButtons();
            RegisterCallbackSettingsButton();

            RegisterCallbackDeathRestartButton();

            RegisterCallBackExitGameButton();
            RegisterCallBackRestartGameButton();


            RegisterHoverableList();

            UpdateSkillIcons();
        }

        private bool IsVisualElementActive(VisualElement element)
        {
            return element.style.display != DisplayStyle.None;
        }
        public void TriggerPlayerUIMessage(PlayerUIMessage playerMessage, float duration)
        {
            if (_currentPlayerUIMessage != null)
            {
                if (_currentPlayerUIMessage.Priority > playerMessage.Priority)
                {
                    return;
                }
            }
            ShowPlayerUIMessage(playerMessage);
            StopAllCoroutines();
            StartCoroutine(HidePlayerMessageCO(duration));

        }
        private void RegisterHoverableList()
        {
            foreach(VisualElement element in _hoverableList)
            {
                element.RegisterCallback<MouseEnterEvent>(MouseHoverAllButtons);
            }
        }

        private void MouseHoverAllButtons(MouseEnterEvent evt)
        {
            GameAudioController.PlayUIHoverSound();
        }

        private void RegisterCallbackDeathRestartButton()
        {
            _deathRestartButton.RegisterCallback<ClickEvent>(DeathRestartButtonPressed);
        }

        private void DeathRestartButtonPressed(ClickEvent evt)
        {
            GameAudioController.PlayUIConfirmSound();
            _gamePlayerController.RestartLevel();
        }
        private IEnumerator HidePlayerMessageCO(float duration)
        {
            yield return new WaitForSeconds(duration);
            //HideVisualElement(_messageLabel);
            _messageLabel.style.opacity = new StyleFloat(0f);
            RestartPlayerUIMessageToDefault();
            _currentPlayerUIMessage = null;

        }
        private void ShowPlayerUIMessage(PlayerUIMessage playerMessage)
        {
   
            ShowVisualElement(_messageLabel);
            _messageLabel.style.opacity = new StyleFloat(1f);
            _messageLabel.text = playerMessage.Text;
            _messageLabel.style.color = playerMessage.Color;
            _currentPlayerUIMessage = playerMessage;
            

        }

        private void RestartPlayerUIMessageToDefault()
        {
            _messageLabel.style.color = _defaultColor;
        }
        private void HidePlayerUIMessage()
        {
            _messageSection.style.display = DisplayStyle.None;
        }

        private void TogglePauseMenu()
        {
            if (_pauseMenu.style.display == DisplayStyle.Flex )
            {
                _pauseMenu.style.display = DisplayStyle.None;
                GameAudioController.PlayUICancelSound();

            }
            else
            {
                _pauseMenu.style.display = DisplayStyle.Flex;
                _playerMovementController.StopMovement();
                GameAudioController.PlayUIWindowOpenSound();
            }
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

        private void ToggleVisualElement(VisualElement element)
        {
            if (element.style.display == DisplayStyle.Flex)
            {
                element.style.display = DisplayStyle.None;

            }
            else
            {
                element.style.display = DisplayStyle.Flex;
            }
        }

        private void RegisterCallbackSettingsButton()
        {
            _settingsButton.RegisterCallback<ClickEvent>(SettingsMenuButtonPressed);

        }

        private void RegisterCallbackPauseMenuButtons()
        {
            _resumeButton.RegisterCallback<ClickEvent>(ResumeButtonPressed);
        }

        private void RegisterCallbackConfirmButton()
        {
            _confirmButton.RegisterCallback<ClickEvent>(ConfirmButtonPressed);
        }
        private void RegisterSettingsSlidersCallbacks()
        {
            _masterVolumeSlider.RegisterValueChangedCallback(MasterVolumeChanged);
            _audioVolumeSlider.RegisterValueChangedCallback(SoundEffectsVolumeChanged);
            _musicVolumeSlider.RegisterValueChangedCallback(MusicVolumeChanged);
        }

        private void MasterVolumeChanged(ChangeEvent<float> evt)
        {
            GameAudioController.PlayUIHoverSound();
            GameAudioController.SetMasterVolume(evt.newValue);
        }

        private void SettingsMenuButtonPressed(ClickEvent evt)
        {
            TogglePauseMenu();
            ToggleVisualElement(_settingsMenu);
            GameAudioController.PlayUIConfirmSound();
        }

        private void ConfirmButtonPressed(ClickEvent evt)
        {
            GameAudioController.PlayUIConfirmSound();
            ToggleVisualElement(_settingsMenu);
            TogglePauseMenu();
        }
        private void SoundEffectsVolumeChanged(ChangeEvent<float> evt)
        {
            GameAudioController.PlayUIHoverSound();
            GameAudioController.SetSoundEffectsVolume(evt.newValue);
        }

        private void MusicVolumeChanged(ChangeEvent<float> evt)
        {
            GameAudioController.SetMusicVolume(evt.newValue);
        }
        private void RegisterCallbackSkillButtons()
        {
            _skillButton1.RegisterCallback<ClickEvent>(SkillButton1Pressed);
            _skillButton2.RegisterCallback<ClickEvent>(SkillButton2Pressed);
            _skillButton3.RegisterCallback<ClickEvent>(SkillButton3Pressed);
            _skillButton4.RegisterCallback<ClickEvent>(SkillButton4Pressed);


            _skillButton1.RegisterCallback<MouseEnterEvent>(SkillButton1HoverEnter);
            _skillButton2.RegisterCallback<MouseEnterEvent>(SkillButton2HoverEnter);
            _skillButton3.RegisterCallback<MouseEnterEvent>(SkillButton3HoverEnter);
            _skillButton4.RegisterCallback<MouseEnterEvent>(SkillButton4HoverEnter);


            _skillButton1.RegisterCallback<MouseLeaveEvent>(SkillButton1HoverLeave);
            _skillButton2.RegisterCallback<MouseLeaveEvent>(SkillButton2HoverLeave);
            _skillButton3.RegisterCallback<MouseLeaveEvent>(SkillButton3HoverLeave);
            _skillButton4.RegisterCallback<MouseLeaveEvent>(SkillButton4HoverLeave);
        }

        private void RegisterCallBackRestartGameButton()
        {
            _restartGameButton.RegisterCallback<ClickEvent>(RestartGameButtonPressed);
        }

        private void RegisterCallBackExitGameButton()
        {
            _exitGameButton.RegisterCallback<ClickEvent>(ExitGameButtonPressed);
        }
        private void ResumeButtonPressed(ClickEvent evt)
        {
            GameAudioController.PlayUIConfirmSound();
            TogglePauseMenu();
        }
       
        private void RestartGameButtonPressed(ClickEvent evt)
        {
            GameAudioController.PlayUIConfirmSound();
            _gamePlayerController.RestartLevel();
        }

        private void ExitGameButtonPressed(ClickEvent evt)
        {
            GameAudioController.PlayUICancelSound();
            Application.Quit();
        }

        private void UpdateSkillIcons()
        {
            if(_playerSkillController.Skills == null)
            {
                Debug.Log("List of Skills is null");
            }
            _skillButton1.style.backgroundImage = new StyleBackground(_playerSkillController.Skills[0].Icon);
            _skillButton2.style.backgroundImage = new StyleBackground(_playerSkillController.Skills[1].Icon);
            _skillButton3.style.backgroundImage = new StyleBackground(_playerSkillController.Skills[2].Icon);
            _skillButton4.style.backgroundImage = new StyleBackground(_playerSkillController.Skills[3].Icon);

        }

        private void SkillButton1Pressed(ClickEvent evt)
        {
            _playerSkillController.ActivateSkill(0);
        }
        private void SkillButton2Pressed(ClickEvent evt)
        {
            _playerSkillController.ActivateSkill(1);
        }
        private void SkillButton3Pressed(ClickEvent evt)
        {
            _playerSkillController.ActivateSkill(2);
        }
        private void SkillButton4Pressed(ClickEvent evt)
        {
            _playerSkillController.ActivateSkill(3);
        }

        private void ShowTooltip()
        {
            GameAudioController.PlayUIHoverSound();
            _tooltip.style.display = DisplayStyle.Flex;
        }

        private void HideTooltip()
        {
            _tooltip.style.display = DisplayStyle.None;
        }

        private void UpdateTooltipFromSkillTemplate(SkillTemplate skillTemplate)
        {
            _tooltipSkillTitle.text = skillTemplate.Name;
            _tooltipSkillDescription.text = skillTemplate.Description;
            _tooltipSkillEnergyCost.text = "Energy cost: " + skillTemplate.SkillActivationData.EnergyCost.ToString();
            _tooltipSkillRecharge.text = "Recharge: " + skillTemplate.SkillActivationData.RechargeTime.ToString() + " sec";
        }

        private void SkillButton1HoverEnter(MouseEnterEvent evt)
        {
            UpdateTooltipFromSkillTemplate(_playerSkillController.Skills[0]);
            ShowTooltip();
        }
        private void SkillButton2HoverEnter(MouseEnterEvent evt)
        {
            UpdateTooltipFromSkillTemplate(_playerSkillController.Skills[1]);
            ShowTooltip();

        }
        private void SkillButton3HoverEnter(MouseEnterEvent evt)
        {
            UpdateTooltipFromSkillTemplate(_playerSkillController.Skills[2]);
            ShowTooltip();
        }
        private void SkillButton4HoverEnter(MouseEnterEvent evt)
        {
            UpdateTooltipFromSkillTemplate(_playerSkillController.Skills[3]);
            ShowTooltip();
        }

        private void SkillButton1HoverLeave(MouseLeaveEvent evt)
        {
            HideTooltip();
        }
        private void SkillButton2HoverLeave(MouseLeaveEvent evt)
        {
            HideTooltip();
        }
        private void SkillButton3HoverLeave(MouseLeaveEvent evt)
        {
            HideTooltip();
        }
        private void SkillButton4HoverLeave(MouseLeaveEvent evt)
        {
            HideTooltip();
        }

        private void ProcessAbilitiesRecharge()
        {
            ProcessAbilityRecharge(0, _skillButton1);
            ProcessAbilityRecharge(1, _skillButton2);
            ProcessAbilityRecharge(2, _skillButton3);
            ProcessAbilityRecharge(3, _skillButton4);

        }

        private void ProcessAbilityRecharge(int index, VisualElement skillButton)
        {
            if (_playerSkillController.IsSkillRecharging(_playerSkillController.Skills[index]))
            {
                float width = _playerSkillController.GetRemainingSkillRechargeTime(index) / _playerSkillController.Skills[index].SkillActivationData.RechargeTime;
                width *= _baseWidthValue;
                skillButton.style.borderRightWidth = new StyleFloat(width);
                skillButton.style.borderBottomWidth = new StyleFloat(width);
                skillButton.style.borderTopWidth = new StyleFloat(width);
                skillButton.style.borderLeftWidth = new StyleFloat(width);
            }
        }

        public void ShowRestartLevelMenu()
        {
            HideVisualElement(_pauseMenu);
            HideVisualElement(_settingsMenu);
            ShowVisualElement(_restartSection);
            
        }
    }
}