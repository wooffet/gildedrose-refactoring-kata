using System.Collections.Generic;
using System.Reflection.Metadata;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;
    private readonly int LEGENDARY_QUALITY = 80;
    private readonly int MAX_NON_LEGENDARY_QUALITY = 50;
    private readonly int MIN_NON_LEGENDARY_QUALITY = 0;
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
                currentItem.SellIn -= 1;

                // if special item (aged, backstage pass or conjured)
                if (IsSpecialItem(currentItem.Name))
                {
                    if (currentItem.Name.StartsWith(SPECIALITEM_AGED_ITEM_PREFIX))
                    {
                        currentItem.Quality += 1;
                    }

                    if (currentItem.Name.StartsWith(SPECIALITEM_BACKSTAGE_PASS_ITEM_PREFIX))
                    {
                        if (currentItem.SellIn > 10)
                        {
                            currentItem.Quality += 1;
                        }

                        if (currentItem.SellIn <= 10 && currentItem.SellIn > 5) 
                        {
                            currentItem.Quality += 2;
                        }

                        if (currentItem.SellIn <= 5)
                        {
                            currentItem.Quality += 3;
                        }

                        if (currentItem.SellIn < 0)
                        {
                            currentItem.Quality = 0;
                        }
                    }

                    if (currentItem.Name.StartsWith(SPECIALITEM_CONJURED_ITEM_NAME_PREFIX))
                    {
                        currentItem.Quality = currentItem.SellIn > 0 ? currentItem.Quality -= 2 : currentItem.Quality -= 4;
                    }
                }
                // else normal item
                else
                {
                    currentItem.Quality = currentItem.SellIn >= 0 ? currentItem.Quality -= 1 : currentItem.Quality -=2;
                }

                // ensure quality is never greater than 50
                if (currentItem.Quality > 50)
                {
                    // just set quality to 50
                    currentItem.Quality = 50;
                }

                // ensure quality is never less than 0
                if (currentItem.Quality < 0)
                {
                    // just set quality to 0
                    currentItem.Quality = 0;
                }
            }
        }
    }
}