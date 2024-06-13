// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using System;
using System.Collections.Generic;

namespace PixelCrushers.QuestMachine
{

    [AddComponentMenu("Pixel Crushers/Quest Machine/Third Party/Invector/Invector Item Reward System")]
    public class InvectorItemRewardSystem : RewardSystem
    {
        [Serializable]
        public class RewardItem
        {
            [Tooltip("ID of item that can be offered as a reward.")]
            public int itemID;

            [Tooltip("Name or short description of item.")]
            public StringField description;

            [Tooltip("Optional icon to show in reward section.")]
            public Sprite icon;

            [Tooltip("Reward system point value of item.")]
            public int pointValue;

            [Tooltip("Amount of item available to give as reward.")]
            public int amountAvailable = 1;

            [Tooltip("Max number of item to give in a single quest.")]
            public int maxAmountPerReward = 1;
        }

        [Tooltip("Items to offer.")]
        [SerializeField]
        private List<RewardItem> m_items = new List<RewardItem>();
        public List<RewardItem> items
        {
            get { return m_items; }
            set { m_items = value; }
        }

        [Tooltip("Shuffle list before choosing rewards.")]
        [SerializeField]
        private bool m_shuffleItems = true;
        public bool shuffleItems
        {
            get { return m_shuffleItems; }
            set { m_shuffleItems = value; }
        }

        [Tooltip("Remove items from list as they're rewarded.")]
        [SerializeField]
        private bool m_removeItemsWhenUsed = true;
        public bool removeItemsWhenUsed
        {
            get { return m_removeItemsWhenUsed; }
            set { m_removeItemsWhenUsed = value; }
        }


        // The quest generator will call this method to try to use up points
        // by choosing rewards to offer.
        public override int DetermineReward(int points, Quest quest)
        {
            var successInfo = quest.GetStateInfo(QuestState.Successful);

            // Offer items:
            if (shuffleItems)
            {
                for (int i = 0; i < items.Count - 2; i++)
                {
                    var j = UnityEngine.Random.Range(i, items.Count);
                    var tmp = items[i];
                    items[i] = items[j];
                    items[j] = tmp;
                }
            }
            for (int i = items.Count - 1; i >= 0; i--)
            {
                var rewardItem = items[i];
                var itemValue = Mathf.Max(1, rewardItem.pointValue);
                if (itemValue <= points)
                {
                    var max = UnityEngine.Random.Range(1, Mathf.Min(rewardItem.amountAvailable, rewardItem.maxAmountPerReward));
                    while (max > 1 && max * itemValue > points)
                    {
                        max--;
                    }
                    int amount = UnityEngine.Random.Range(1, max);
                    if (removeItemsWhenUsed)
                    {
                        rewardItem.amountAvailable -= amount;
                        if (rewardItem.amountAvailable <= 0) items.Remove(rewardItem);
                    }

                    // Add some UI content to the quest's offerContentList:
                    if (rewardItem.icon == null)
                    {
                        var itemText = BodyTextQuestContent.CreateInstance<BodyTextQuestContent>();
                        itemText.bodyText = new StringField((amount == 1) ? StringField.GetStringValue(rewardItem.description) : $"{amount} {rewardItem.description}");
                        quest.offerContentList.Add(itemText);
                    }
                    else
                    {
                        var itemIcon = IconQuestContent.CreateInstance<IconQuestContent>();
                        itemIcon.image = rewardItem.icon;
                        itemIcon.count = amount;
                        itemIcon.caption = new StringField(rewardItem.description);
                        quest.offerContentList.Add(itemIcon);
                    }

                    // Add an InvectorAddItemQuestAction action to the quest's Successful state:
                    var itemAction = InvectorAddItemQuestAction.CreateInstance<InvectorAddItemQuestAction>();
                    itemAction.itemID = rewardItem.itemID;
                    itemAction.amount = new QuestNumber(amount);
                    successInfo.actionList.Add(itemAction);

                    // Reduce points left:
                    points -= itemValue;
                    if (points <= 0) return 0;
                }
            }

            return points;
        }

    }
}
