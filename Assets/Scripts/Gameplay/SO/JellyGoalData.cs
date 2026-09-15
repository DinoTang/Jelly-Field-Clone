using System;
using UnityEngine;

[Serializable]
public class JellyGoalData
{
    [SerializeField] private JellyGoalSO goal;
    [SerializeField] private int targetAmount = 1;

    public JellyGoalSO Goal => goal;
    public int TargetAmount => targetAmount;
    public bool IsCompleted => this.targetAmount <= 0;

    public void DecreaseTargetAmount(int amount = 1)
    {
        this.targetAmount -= amount;
    }

    public JellyGoalData Clone()
    {
        return new JellyGoalData
        {
            goal = this.goal,
            targetAmount = this.targetAmount
        };
    }
}