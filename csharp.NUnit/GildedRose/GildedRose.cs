using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;
    // TODO: Can probably move all these values to a config file/db table
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
    private readonly string SPECIALITEM_BACKSTAGE_PASS_ITEM_PREFIX = "Backstage pass"; // TODO: double check backstage items will always have this prefix with PM/BA
    private readonly string SPECIALITEM_CONJURED_ITEM_NAME_PREFIX = "Conjured";

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    private bool CanUpdateItem(int itemQuality) => IsNonLegendaryQualityItem(itemQuality);
    private bool IsNonLegendaryQualityItem(int itemQuality) => itemQuality < LEGENDARY_QUALITY;
    private bool IsSpecialItem(string itemName) => itemName.StartsWith(SPECIALITEM_AGED_ITEM_PREFIX) ||
        itemName.StartsWith(SPECIALITEM_BACKSTAGE_PASS_ITEM_PREFIX) || itemName.StartsWith(SPECIALITEM_CONJURED_ITEM_NAME_PREFIX);

    public void UpdateQuality()
    {
        // we can avoid processing items that are already minimum quality (quality cannot be less than 0) or are legendary quality (sellin/quality never changes)
        // so lets remove them
        var itemsToUpdate = Items.Where(item => CanUpdateItem(item.Quality));

        foreach (var item in itemsToUpdate)
        {
            // reduce the SellIn days by 1
            item.SellIn -= DEFAULT_SELLIN_DECAY;

            // if special item (aged, backstage pass or conjured)
            if (IsSpecialItem(item.Name))
            {
                if (item.Name.StartsWith(SPECIALITEM_AGED_ITEM_PREFIX))
                {
                    item.Quality += SPECIALITEM_DEFAULT_QUALITY_GAIN;
                }

                if (item.Name.StartsWith(SPECIALITEM_BACKSTAGE_PASS_ITEM_PREFIX))
                {
                    if (item.SellIn > BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_ONE)
                    {
                        item.Quality += SPECIALITEM_DEFAULT_QUALITY_GAIN;
                    }

                    if (item.SellIn <= BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_ONE && item.SellIn > BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_TWO)
                    {
                        item.Quality += SPECIALITEM_DEFAULT_QUALITY_GAIN * 2;
                    }

                    if (item.SellIn <= BACKSTAGE_PASS_SELLIN_THRESHOLD_STAGE_TWO)
                    {
                        item.Quality += SPECIALITEM_DEFAULT_QUALITY_GAIN * 3;
                    }

                    if (item.SellIn < DEFAULT_SELLIN_DECAY_THRESHOLD)
                    {
                        item.Quality = MIN_NON_LEGENDARY_QUALITY;
                    }
                }

                if (item.Name.StartsWith(SPECIALITEM_CONJURED_ITEM_NAME_PREFIX))
                {
                    item.Quality = item.SellIn > DEFAULT_SELLIN_DECAY_THRESHOLD ?
                        item.Quality -= CONJURED_ITEM_QUALITY_DECAY : item.Quality -= (CONJURED_ITEM_QUALITY_DECAY * 2);
                }
            }
            // else normal item
            else
            {
                item.Quality = item.SellIn >= DEFAULT_SELLIN_DECAY_THRESHOLD ?
                    item.Quality -= DEFAULT_QUALITY_DECAY : item.Quality -= (DEFAULT_QUALITY_DECAY * 2);
            }

            // ensure quality is never greater than 50
            if (item.Quality > MAX_NON_LEGENDARY_QUALITY)
            {
                // just set quality to 50
                item.Quality = MAX_NON_LEGENDARY_QUALITY;
            }

            // ensure quality is never less than 0
            if (item.Quality < MIN_NON_LEGENDARY_QUALITY)
            {
                // just set quality to 0
                item.Quality = MIN_NON_LEGENDARY_QUALITY;
            }

        }
    }
}