using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager instance;

    [SerializeField] public List<QueuePoint> queuePoints;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        queuePoints.Sort((a, b) => a.index.CompareTo(b.index));
    }

    public void UpdateQueuePositions()
    {
        Vector3 basePos = PointManager.instance.registrationPoint.transform.position;
        Vector3 offset = new Vector3(0, 0, 2);

        for (int i = 0; i < queuePoints.Count; i++)
        {
            if (queuePoints[i].isOccupied && queuePoints[i].occupant != null)
            {
                var occupant = queuePoints[i].occupant;
                occupant.Agent.isStopped = false;
                occupant.Agent.SetDestination(basePos + offset * i);
            }
        }
    }

    public bool TryJoinQueue(BotController bot)
    {
        int currentCount = 0;
        foreach (var qp in queuePoints)
        {
            if (qp.isOccupied)
                currentCount++;
        }

        if (currentCount < queuePoints.Count)
        {
            int newIndex = currentCount;
            queuePoints[newIndex].isOccupied = true;
            queuePoints[newIndex].occupant = bot;
            bot.AssignedQueuePoint = queuePoints[newIndex];

            Vector3 basePos = PointManager.instance.registrationPoint.transform.position;
            Vector3 offset = new Vector3(0, 0, 2);
            Vector3 targetPos = basePos + offset * newIndex;

            if (bot.Agent != null)
            {
                bot.Agent.isStopped = false;
                bot.Agent.updateRotation = true;
                bot.Agent.SetDestination(targetPos);
            }
            else
            {
                bot.transform.position = targetPos;
            }

            UpdateQueuePositions();
            return true;
        }
        return false;
    }

    public bool IsBotAtFront(BotController bot)
    {
        if (queuePoints.Count > 0 && queuePoints[0].isOccupied && queuePoints[0].occupant == bot)
            return true;
        return false;
    }

    public void RemoveBotFromQueue(BotController bot)
    {
        int index = -1;
        for (int i = 0; i < queuePoints.Count; i++)
        {
            if (queuePoints[i].occupant == bot)
            {
                index = i;
                break;
            }
        }
        if (index == -1) return;

        queuePoints[index].isOccupied = false;
        queuePoints[index].occupant = null;
        bot.AssignedQueuePoint = null;

        for (int i = index + 1; i < queuePoints.Count; i++)
        {
            if (queuePoints[i].isOccupied)
            {
                BotController shiftingBot = queuePoints[i].occupant;
                queuePoints[i - 1].occupant = shiftingBot;
                queuePoints[i - 1].isOccupied = true;
                shiftingBot.AssignedQueuePoint = queuePoints[i - 1];

                queuePoints[i].occupant = null;
                queuePoints[i].isOccupied = false;
            }
        }

        UpdateQueuePositions();
    }
}
