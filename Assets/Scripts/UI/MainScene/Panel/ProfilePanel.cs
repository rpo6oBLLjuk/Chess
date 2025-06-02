using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ProfilePanel : AnimatedPanel
{
    [Inject] SceneLoader sceneLoader;
    [Inject] DBService dbService;
    [Inject] NotificationService notificationService;
    [Inject] GameAnalyticsData gameAnalyticsData;

    [Space]
    [SerializeField] TMP_Text nicknameText;

    [Space]
    [SerializeField] TMP_Text registrationDate;
    [SerializeField] TMP_Text lastLoginDate;

    [SerializeField] TMP_Text winsCount;
    [SerializeField] TMP_Text defeatsCount;

    [Space]
    [SerializeField] Button logOutButton;


    private void OnEnable()
    {
        nicknameText.text = dbService.Data.Username;

        registrationDate.text = ((System.DateTime)gameAnalyticsData.registrationDate).ToString();
        lastLoginDate.text = ((System.DateTime)gameAnalyticsData.lastLoginDate).ToString();

        winsCount.text = gameAnalyticsData.winsCount.ToString();
        defeatsCount.text = gameAnalyticsData.defeatsCount.ToString();

        logOutButton.onClick.AddListener(LogOutButtonClick);
    }

    private void LogOutButtonClick()
    {
        notificationService.ShowDialog((bool ok) =>
        {
            if (ok)
                LogOut();
        },"Log out of your account?", "Profile", DialogType.OkCancel);
    }

    private void LogOut()
    {
        dbService.LogOut();
        sceneLoader.LoadAuthScene(inverseLoadScreen: true);
    }
}
