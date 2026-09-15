using System.Collections.Generic;
using UnityEngine;

public class GoalContainerUI : BaseUI
{
    [SerializeField] private GoalItemUI goalItemPrefab;

    private readonly List<GoalItemUI> goalItems = new();


    protected override void Start()
    {
        base.Start();

        this.SpawnGoalItems();

        if (this.goalItemPrefab != null)
            this.goalItemPrefab.gameObject.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoalAmountChanged += this.RefreshGoalItem;
            GameManager.Instance.LevelRestarted += this.RefreshGoals;
        }
    }



    protected override void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoalAmountChanged -= this.RefreshGoalItem;
            GameManager.Instance.LevelRestarted -= this.RefreshGoals;
        }

        base.OnDisable();
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadGoalItemPrefab();
    }
    protected void LoadGoalItemPrefab()
    {
        if (this.goalItemPrefab != null) return;

        this.goalItemPrefab = GetComponentInChildren<GoalItemUI>();
        Debug.Log(transform.name + ": LoadGoalItemPrefab", gameObject);
    }

    private void SpawnGoalItems()
    {
        this.goalItems.Clear();

        List<JellyGoalData> progresses = GameManager.Instance.Goals;


        foreach (JellyGoalData progress in progresses)
        {
            GoalItemUI item = Instantiate(
                goalItemPrefab,
                transform
            );

            item.SetProgress(progress);
            item.gameObject.SetActive(true);
            this.goalItems.Add(item);
        }
    }


    private void RefreshGoalItem(JellyGoalData progress)
    {
        if (progress == null)
            return;

        GoalItemUI item = this.goalItems.Find(item => item.Progress == progress);
        if (item == null)
            return;

        item.Refresh();
    }

    private void RefreshGoals()
    {
        foreach (GoalItemUI item in this.goalItems)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        this.goalItems.Clear();

        this.SpawnGoalItems();
    }
}