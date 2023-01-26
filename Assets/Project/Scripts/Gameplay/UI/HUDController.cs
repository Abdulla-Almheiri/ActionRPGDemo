using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Chaos.Gameplay.Systems;
using Chaos.Gameplay.Characters;
using Chaos.Gameplay.Skills;

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

        private Slider _AudioVolumeSlider;
        private Slider _MusicVolumeSlider;

        private VisualElement _settingsMenu;
        private Button _confirmButton;

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape) == true)
            {
                TogglePauseMenu();
            }

            ProcessAbilitiesRecharge();
            ProcessHealthAndEnergyBars();
        }
        private void ProcessHealthAndEnergyBars()
        {
            var healthValue = _playerCombatController.GetHealthPercentage() / 100f;
            var energyValue = _playerCombatController.GetEnergyPercentage() / 100f;
            Debug.Log("Health =   :" + healthValue);

           _healthBar.style.scale = new StyleScale(new Scale(new Vector3(healthValue, 1f, 1f)));
            _energyBar.style.scale = new StyleScale(new Scale(new Vector3(energyValue, 1f, 1f)));

        }

        private void Initialize()
        {
            _rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
            _pauseMenu = _rootVisualElement.Q("PauseMenu");
            _playerMovementController = _gamePlayerController.PlayerMovementController;
            _playerSkillController = _gamePlayerController.PlayerMovementController.gameObject.GetComponent<CharacterSkillController>();
            _playerCombatController = _gamePlayerController.PlayerMovementController.gameObject.GetComponent<CharacterCombatController>();


            _resumeButton = _rootVisualElement.Q("ResumeButton");
            _settingsButton = _rootVisualElement.Q<Button>("SettingsButton");

            _tooltip = _rootVisualElement.Q("Tooltip");
            RegisterCallbackPauseMenuButtons();

            _skillButton1 = _rootVisualElement.Q("SkillButton1");
            _skillButton2 = _rootVisualElement.Q("SkillButton2");
            _skillButton3 = _rootVisualElement.Q("SkillButton3");
            _skillButton4 = _rootVisualElement.Q("SkillButton4");

            _tooltip = _rootVisualElement.Q("TooltipSection");
            _tooltipSkillTitle = _rootVisualElement.Q<Label>("TooltipTitle");
            _tooltipSkillDescription = _rootVisualElement.Q<Label>("TooltipDescription");
            _tooltipSkillEnergyCost = _rootVisualElement.Q<Label>("EnergyCost");
            _tooltipSkillRecharge = _rootVisualElement.Q<Label>("RechargeTime");

            _healthBar = _rootVisualElement.Q("HealthBar");
            _energyBar = _rootVisualElement.Q("EnergyBar");

            _AudioVolumeSlider = _rootVisualElement.Q<Slider>("SFXSlider");
            _MusicVolumeSlider = _rootVisualElement.Q<Slider>("MusicSlider");

            _AudioVolumeSlider.value = GameAudioController.SFXVolume;
            _MusicVolumeSlider.value = GameAudioController.MusicVolume;

            _confirmButton = _rootVisualElement.Q<Button>("ExitSettingsButton");
            _settingsMenu = _rootVisualElement.Q("SettingsMenu");

            RegisterCallbackSkillButtons();
            RegisterSettingsSlidersCallbacks();
            RegisterCallbackConfirmButton();
            RegisterCallbackPauseMenuButtons();
            RegisterCallbackSettingsButton();

            UpdateSkillIcons();
        }



        private void TogglePauseMenu()
        {
            if (_pauseMenu.style.display == DisplayStyle.Flex )
            {
                _pauseMenu.style.display = DisplayStyle.None;

            }
            else
            {
                _pauseMenu.style.display = DisplayStyle.Flex;
                _playerMovementController.StopMovement();
            }
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
            _AudioVolumeSlider.RegisterValueChangedCallback(AudioVolumeChanged);
            _MusicVolumeSlider.RegisterValueChangedCallback(MusicVolumeChanged);
        }

        private void SettingsMenuButtonPressed(ClickEvent evt)
        {
            TogglePauseMenu();
            ToggleVisualElement(_settingsMenu);
        }

        private void ConfirmButtonPressed(ClickEvent evt)
        {
            ToggleVisualElement(_settingsMenu);
            TogglePauseMenu();
        }
        private void AudioVolumeChanged(ChangeEvent<float> evt)
        {
            GameAudioController.SetSFXVolume(evt.newValue / 100f);
        }

        private void MusicVolumeChanged(ChangeEvent<float> evt)
        {
            GameAudioController.SetMusicVolume(evt.newValue / 100f);
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


        private void ResumeButtonPressed(ClickEvent evt)
        {
            TogglePauseMenu();
            Debug.Log("Resume pressed");
        }
       
        private void RestartGameButtonPressed(ClickEvent evt)
        {

        }

        private void ExitGameButtonPressed(ClickEvent evt)
        {

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
            //Debug.Log("Skill icons updated :  " + _playerSkillController.Skills[0].Icon.name);

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
    }
}