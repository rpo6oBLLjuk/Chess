using UnityEngine;
using Zenject;

public class AchievementService : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] NotificationService notificationService;


}
