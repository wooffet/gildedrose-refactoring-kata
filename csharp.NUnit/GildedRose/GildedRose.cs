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
    private readonly string SPECIALITEM_BACKSTAGE_PASS_PREFIX = "Backstage pass"; // double check backstage items will always have this prefix with PM/BA
    private readonly string SPECIALITEM_CONJURED_ITEM_NAME_PREFIX = "Conjured";

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public void UpdateQuality()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            var currentItem = Items[i];

            // if normal item
            if (!currentItem.Name.StartsWith(SPECIALITEM_AGED_ITEM_PREFIX) && !currentItem.Name.StartsWith(SPECIALITEM_BACKSTAGE_PASS_PREFIX)
                && !currentItem.Name.StartsWith(SPECIALITEM_CONJURED_ITEM_NAME_PREFIX))
            {
                if (currentItem.Quality > MIN_NON_LEGENDARY_QUALITY)
                {
                    if (currentItem.Quality != LEGENDARY_QUALITY)
                    {
                        currentItem.Quality = currentItem.Quality - 1;
                    }
                }
            }
            // else if brie, backstage pass or conjured
            else
            {
                if (currentItem.Quality < MAX_NON_LEGENDARY_QUALITY)
                {
                    if (currentItem.Name.StartsWith(SPECIALITEM_CONJURED_ITEM_NAME_PREFIX))
                    {
                        if (currentItem.SellIn > 0)
                        {
                            currentItem.Quality -= 2;
                        }
                    }
                    else
                    {
                        currentItem.Quality = currentItem.Quality + 1;

                        if (currentItem.Name.StartsWith(SPECIALITEM_BACKSTAGE_PASS_PREFIX))
                        {
                            if (currentItem.SellIn < 11)
                            {
                                if (currentItem.Quality < MAX_NON_LEGENDARY_QUALITY)
                                {
                                    currentItem.Quality = currentItem.Quality + 1;
                                }
                            }

                            if (currentItem.SellIn < 6)
                            {
                                if (currentItem.Quality < MAX_NON_LEGENDARY_QUALITY)
                                {
                                    currentItem.Quality = currentItem.Quality + 1;
                                }
                            }
                        }
                    }
                }
            }

            if (currentItem.Quality != LEGENDARY_QUALITY)
            {
                currentItem.SellIn = currentItem.SellIn - 1;
            }

            if (currentItem.SellIn < 0)
            {
                if (!currentItem.Name.StartsWith(SPECIALITEM_AGED_ITEM_PREFIX))
                {
                    if (!currentItem.Name.StartsWith(SPECIALITEM_BACKSTAGE_PASS_PREFIX))
                    {
                        if (currentItem.Quality > MIN_NON_LEGENDARY_QUALITY)
                        {
                            if (currentItem.Quality != LEGENDARY_QUALITY)
                            {
                                if (currentItem.Name.StartsWith(SPECIALITEM_CONJURED_ITEM_NAME_PREFIX))
                                {
                                    currentItem.Quality -= 4;
                                }
                                else
                                {
                                    currentItem.Quality = currentItem.Quality - 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        currentItem.Quality = currentItem.Quality - currentItem.Quality;
                    }
                }
                else
                {
                    if (currentItem.Quality < MAX_NON_LEGENDARY_QUALITY)
                    {
                        currentItem.Quality = currentItem.Quality + 1;
                    }
                }
            }

            if (currentItem.Quality != LEGENDARY_QUALITY && currentItem.Quality > MAX_NON_LEGENDARY_QUALITY)
            {
                currentItem.Quality = MAX_NON_LEGENDARY_QUALITY;
            }
        }
    }
}