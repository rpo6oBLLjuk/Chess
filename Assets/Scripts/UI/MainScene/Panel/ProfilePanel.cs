using TMPro;
using UnityEngine;
using Zenject;

public class ProfilePanel : AnimatedPanel
{
    [Inject] DBService dbService;

    [SerializeField] private TextMeshProUGUI nicknameText;


    private void OnEnable()
    {
        nicknameText.text = dbService.Data.Username;
    }
}
