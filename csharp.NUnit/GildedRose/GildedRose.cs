using System.Collections.Generic;
using System.Reflection.Metadata;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;
    private readonly int LEGENDARY_QUALITY = 80;
    private readonly int MAX_NON_LEGENDARY_QUALITY = 50;
    private readonly int MIN_NON_LEGENDARY_QUALITY = 0;
    private readonly int DEFAULT_QUALITY_DECAY = 1;
    private readonly int DEFAULT_SELLIN_DECAY = 1;
    private readonly int DEFAULT_SELLIN_DECAY_THRESHOLD = 0;
    private readonly int SPECIALITEM_DEFAULT_QUALITY_GAIN = 1;
    private readonly int CONJURED_ITEM_QUALITY_DECAY = 2;
    private readonly int BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_ONE = 10;
    private readonly int BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_TWO = 5;
    private readonly string SPECIALITEM_AGED_ITEM_PREFIX = "Aged";
    private readonly string SPECIALITEM_BACKSTAGE_PASS_ITEM_PREFIX = "Backstage pass"; // double check backstage items will always have this prefix with PM/BA
    private readonly string SPECIALITEM_CONJURED_ITEM_NAME_PREFIX = "Conjured";

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    private bool CanUpdateItem(int itemQuality) => IsNonLegendaryQualityItem(itemQuality) && itemQuality > MIN_NON_LEGENDARY_QUALITY;
    private bool IsNonLegendaryQualityItem(int itemQuality) => itemQuality < LEGENDARY_QUALITY;
    private bool IsSpecialItem(string itemName) => itemName.StartsWith(SPECIALITEM_AGED_ITEM_PREFIX) || 
        itemName.StartsWith(SPECIALITEM_BACKSTAGE_PASS_ITEM_PREFIX) || itemName.StartsWith(SPECIALITEM_CONJURED_ITEM_NAME_PREFIX);

    public void UpdateQuality()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            var currentItem = Items[i];

            // check if we can process the item (quality > 0 and not a legendary quality item)
            // we can avoid processing items that are already minimum quality (quality cannot be less than 0) or are legendary quality (sellin/quality never changes)
            if (CanUpdateItem(currentItem.Quality))
            {
                // reduce the SellIn days by 1
                currentItem.SellIn -= DEFAULT_SELLIN_DECAY;

                // if special item (aged, backstage pass or conjured)
                if (IsSpecialItem(currentItem.Name))
                {
                    if (currentItem.Name.StartsWith(SPECIALITEM_AGED_ITEM_PREFIX))
                    {
                        currentItem.Quality += SPECIALITEM_DEFAULT_QUALITY_GAIN;
                    }

                    if (currentItem.Name.StartsWith(SPECIALITEM_BACKSTAGE_PASS_ITEM_PREFIX))
                    {
                        if (currentItem.SellIn > BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_ONE)
                        {
                            currentItem.Quality += SPECIALITEM_DEFAULT_QUALITY_GAIN;
                        }

                        if (currentItem.SellIn <= BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_ONE && currentItem.SellIn > BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_TWO) 
                        {
                            currentItem.Quality += SPECIALITEM_DEFAULT_QUALITY_GAIN * 2;
                        }

                        if (currentItem.SellIn <= BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_TWO)
                        {
                            currentItem.Quality += SPECIALITEM_DEFAULT_QUALITY_GAIN * 3;
                        }

                        if (currentItem.SellIn < DEFAULT_SELLIN_DECAY_THRESHOLD)
                        {
                            currentItem.Quality = MIN_NON_LEGENDARY_QUALITY;
                        }
                    }

                    if (currentItem.Name.StartsWith(SPECIALITEM_CONJURED_ITEM_NAME_PREFIX))
                    {
                        currentItem.Quality = currentItem.SellIn > DEFAULT_SELLIN_DECAY_THRESHOLD ? 
                            currentItem.Quality -= CONJURED_ITEM_QUALITY_DECAY : currentItem.Quality -= (CONJURED_ITEM_QUALITY_DECAY * 2);
                    }
                }
                // else normal item
                else
                {
                    currentItem.Quality = currentItem.SellIn >= DEFAULT_SELLIN_DECAY_THRESHOLD ? 
                        currentItem.Quality -= DEFAULT_QUALITY_DECAY : currentItem.Quality -= (DEFAULT_QUALITY_DECAY * 2);
                }

                // ensure quality is never greater than 50
                if (currentItem.Quality > MAX_NON_LEGENDARY_QUALITY)
                {
                    // just set quality to 50
                    currentItem.Quality = MAX_NON_LEGENDARY_QUALITY;
                }

                // ensure quality is never less than 0
                if (currentItem.Quality < MIN_NON_LEGENDARY_QUALITY)
                {
                    // just set quality to 0
                    currentItem.Quality = MIN_NON_LEGENDARY_QUALITY;
                }
            }
        }
    }
}