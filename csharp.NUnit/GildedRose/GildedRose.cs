using System.Collections.Generic;
using System.Reflection.Metadata;

namespace GildedRoseKata;

public class GildedRose
{
    IList<Item> Items;
    private readonly int LEGENDARY_QUALITY = 80;
    private readonly int MAX_NON_LEGENDARY_QUALITY = 50;
    private readonly int MIN_NON_LEGENDARY_QUALITY = 0;
    private readonly string SPECIALITEM_AGED_BRIE_NAME = "Aged Brie";

    public GildedRose(IList<Item> Items)
    {
        this.Items = Items;
    }

    public void UpdateQuality()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            var currentItem = Items[i];

            if (currentItem.Name != SPECIALITEM_AGED_BRIE_NAME && currentItem.Name != "Backstage passes to a TAFKAL80ETC concert")
            {
                if (currentItem.Quality > MIN_NON_LEGENDARY_QUALITY)
                {
                    if (currentItem.Quality != LEGENDARY_QUALITY)
                    {
                        currentItem.Quality = currentItem.Quality - 1;
                    }
                }
            }
            else
            {
                if (currentItem.Quality < MAX_NON_LEGENDARY_QUALITY)
                {
                    currentItem.Quality = currentItem.Quality + 1;

                    if (currentItem.Name == "Backstage passes to a TAFKAL80ETC concert")
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

            if (currentItem.Quality != LEGENDARY_QUALITY)
            {
                currentItem.SellIn = currentItem.SellIn - 1;
            }

            if (currentItem.SellIn < 0)
            {
                if (currentItem.Name != SPECIALITEM_AGED_BRIE_NAME)
                {
                    if (currentItem.Name != "Backstage passes to a TAFKAL80ETC concert")
                    {
                        if (currentItem.Quality > MIN_NON_LEGENDARY_QUALITY)
                        {
                            if (currentItem.Quality != LEGENDARY_QUALITY)
                            {
                                currentItem.Quality = currentItem.Quality - 1;
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