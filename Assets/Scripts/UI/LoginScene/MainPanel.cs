using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.LoginScene
{
    public class MainPanel : MonoBehaviour
    {
        [Inject] DBService dbService;
        [Inject] SceneLoader sceneLoader;
        [Inject] NotificationService notificationService;

        [SerializeField] Button swapButton;

        [Header("SingIn")]
        [SerializeField] AnimatedPanel signinPanel;

        [SerializeField] TMP_InputField signinLoginInputField;
        [SerializeField] TMP_InputField signinPasswordInputField;

        [SerializeField] Button signinButton;

        [Header("SingUp")]
        [SerializeField] AnimatedPanel signupPanel;
       
        [SerializeField] TMP_InputField nicknameInputField;
        [SerializeField] TMP_InputField signupLoginInputField;
        [SerializeField] TMP_InputField signupPasswordInputField;

        [SerializeField] Button signupButton;

        private bool signinPanelActive = true;


        private void Start()
        {
            swapButton.onClick.AddListener(SwapPanels);
            signinButton.onClick.AddListener(SigninOnClick);
            signupButton.onClick.AddListener(SignupOnClick);

            if (dbService.Data.PlayerId > 0)
                notificationService.ShowDialog((_) => sceneLoader.LoadMainScene(), $"Hi, {dbService.Data.Username}");
        }

        private void SwapPanels()
        {
            signinPanelActive = !signinPanelActive;
            if (signinPanelActive)
            {
                signinPanel.AnimShow();
                signupPanel.AnimHide();
            }
            else
            {
                signinPanel.AnimHide();
                signupPanel.AnimShow();
            }
        }

        private async void SigninOnClick()
        {
            try
            {
                signinButton.interactable = false;
                (bool success, string nickname, int playerId) = await dbService.SignInController.SignInAsync(signinLoginInputField.text, signinPasswordInputField.text);

                if (success)
                {
                    dbService.SaveData(nickname, playerId);
                    Debug.Log($"SignIn with playerId: {dbService.Data.PlayerId}");

                    notificationService.ShowDialog((_) => sceneLoader.LoadMainScene(), $"Hi, {dbService.Data.Username}");
                }
                else
                {
                    Debug.Log("SignIn false");
                }

                signinButton.interactable = true;
            }
            catch(Exception ex)
            {
                notificationService.ShowPopup(ex.Message, popupType: PopupType.Error);
            }
        }
        private async void SignupOnClick()
        {
            signupButton.interactable= false;
            (bool success, string nickname, int playerId) = await dbService.SignUpController.SignUpAsync(signupLoginInputField.text, signupPasswordInputField.text, nicknameInputField.text);

            if (success)
            {
                dbService.SaveData(nickname, playerId);
                Debug.Log($"SignUp with playerId: {dbService.Data.PlayerId}");

                notificationService.ShowDialog((_) => sceneLoader.LoadMainScene(), $"Hi, {dbService.Data.Username}");
            }
            else
            {
                Debug.Log($"SignUp false, error id: {dbService.Data.PlayerId}");
            }
            signupButton.interactable = true;
        }
    }
}

