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

        [SerializeField] AnimatedPanel loadingScreen;
        [SerializeField] TMP_Text loadingText;

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

            TryAutoAuth();
        }

        private void OnDisable()
        {
            swapButton.onClick.RemoveListener(SwapPanels);
            signinButton.onClick.RemoveListener(SigninOnClick);
            signupButton.onClick.RemoveListener(SignupOnClick);
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

        private async void TryAutoAuth()
        {
            if (dbService.Data.PlayerId < 0)
                return;

            ShowLoadingScreen("Auto auth...");
            signinButton.interactable = false;

            bool success = await dbService.AutoAuthController.TryAutoLogin();
            if (success)
                ShowHiDialog();
            else
                loadingScreen.AnimHide();
        }
        private async void SigninOnClick()
        {
            ShowLoadingScreen("Sign In...");
            signinButton.interactable = false;

            bool success = await dbService.SignInController.SignInAsync(signinLoginInputField.text, signinPasswordInputField.text);

            if (success)
            {
                Debug.Log($"SignIn with playerId: {dbService.Data.PlayerId}");
                ShowHiDialog();
            }
            else
            {
                Debug.Log("SignIn false, error id: {dbService.Data.PlayerId}");
                signinButton.interactable = true;
                loadingScreen.AnimHide();
            }
        }
        private async void SignupOnClick()
        {
            ShowLoadingScreen("Sign Up...");
            signupButton.interactable = false;

            bool success = await dbService.SignUpController.SignUpAsync(signupLoginInputField.text, signupPasswordInputField.text, nicknameInputField.text);

            if (success)
            {
                Debug.Log($"SignUp with playerId: {dbService.Data.PlayerId}");
                ShowHiDialog();
            }
            else
            {
                Debug.Log($"SignUp false, error id: {dbService.Data.PlayerId}");
                signupButton.interactable = true;
                loadingScreen.AnimHide();
            }
        }

        private void ShowLoadingScreen(string loadingText)
        {
            loadingScreen.ForceHide();
            loadingScreen.AnimShow();

            this.loadingText.text = loadingText;
        }
        private async void ShowHiDialog()
        {
            ShowLoadingScreen("Loading userdata...");
            
            await dbService.AnalyticsController.LoadAnalyticsData();
            
            loadingScreen.AnimHide();
            notificationService.ShowDialog((_) => sceneLoader.LoadMainScene(), $"Hi, {dbService.Data.Username}");
        }
    }
}

