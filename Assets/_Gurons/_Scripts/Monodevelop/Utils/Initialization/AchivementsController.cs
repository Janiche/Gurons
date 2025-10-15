using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.UI;

public class AchivementsController : MonoBehaviour
{

    public delegate void AchievementButton(int id);
    public static AchievementButton OnReclaimAchievement;
    public static void ReclaimAchievement(int id)
    {
        OnReclaimAchievement?.Invoke(id);
    }


    //[SerializeField] private TextTable _textTable;

    [Space]
    [SerializeField] private GameObject _achivementItem;
    [SerializeField] private ScrollRect _scroll;

    private int _achivementCount = 0;

    [SerializeField] private List<Achivement> _achivements;

    void OnEnable()
    {
        OnReclaimAchievement += Reclaim;

        _achivements = AchivementSystem.Instance.GetAchivements;
        _achivementCount = _achivements.Count;

        for (int i = 0; i < _achivementCount; i++)
        {
            GameObject _go = Instantiate(_achivementItem, _scroll.content);
            AchivementItem _item = _go.GetComponent<AchivementItem>();

            AchievementCondition _ach = AchivementSystem.Instance.GetCondition(_achivements[i].id);

            string nameKey = $"Achievements.{_achivements[i].name}.Name";
            string descKey = $"Achievements.{_achivements[i].name}.Description";


          /*string title = _textTable.GetFieldText(nameKey);
            string description = _textTable.GetFieldText(descKey);
            */

            string title = string.Empty;
            string description = string.Empty;

            _item._Init(title, description, _ach, _achivements[i].unlocked, _achivements[i].claimed);

        }
    }

    private void OnDisable()
    {
        OnReclaimAchievement -= Reclaim;
    }

    private void Reclaim(int id)
    {
        int index = _achivements.FindIndex(a => a.id == id);
        _achivements[index].claimed = true;

        index = GlobalVars.saveData.achivementData.achivements.FindIndex(a => a.id == id);
        GlobalVars.saveData.achivementData.achivements[index].claimed = true;


        SaveSystem.SaveData();
    }

}
