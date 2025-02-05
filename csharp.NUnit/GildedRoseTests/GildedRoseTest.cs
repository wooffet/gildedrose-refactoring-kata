using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Test]
    public void UpdateQuality_Item_NameShouldBeUnmodified()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Name, Is.EqualTo("foo"));
    }

    [Test]
    public void UpdateQuality_NormalItem_ShouldDecreaseSellInAndQualityByOne()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 2, Quality = 2 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(1));
        Assert.That(items[0].Quality, Is.EqualTo(1));
    }

    [Test]
    public void UpdateQuality_NormalItem_ShouldDecreaseSellInByOneAndQualityByTwo()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 5 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(-1));
        Assert.That(items[0].Quality, Is.EqualTo(3));
    }

    [Test]
    public void UpdateQuality_Item_QualityShouldNotBeNegative()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = -2, Quality = 1 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(-3));
        Assert.That(items[0].Quality, Is.EqualTo(0));
    }

    [Test]
    public void UpdateQuality_LegendaryItem_ShouldNotUpdate()
    {
        var items = new List<Item> { new Item { Name = "legedaryfoo", SellIn = 1, Quality = 80 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(1));
        Assert.That(items[0].Quality, Is.EqualTo(80));
    }

    [Test]
    public void UpdateQuality_SpecialItem_AgedBrie_ShouldDecreaseSellInAndIncreaseQualityByOne()
    {
        var items = new List<Item> { new Item { Name = "Aged Brie", SellIn = 5, Quality = 5 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(4));
        Assert.That(items[0].Quality, Is.EqualTo(6));
    }

    [Test]
    public void UpdateQuality_Item_QualityShouldNotExceedFifty()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 2, Quality = 55 }, new Item { Name = "Aged Brie", SellIn = 2, Quality = 50 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(1));
        Assert.That(items[0].Quality, Is.EqualTo(50));
        Assert.That(items[1].SellIn, Is.EqualTo(1));
        Assert.That(items[1].Quality, Is.EqualTo(50));
    }

    [Test]
    public void UpdateQuality_SpecialItem_BackstagePass_GivenSellInExceedsTenShouldDecreaseSellInAndIncreaseQualityByOne()
    {
        var items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 5 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(14));
        Assert.That(items[0].Quality, Is.EqualTo(6));
    }

    [Test]
    public void UpdateQuality_SpecialItem_BackstagePass_GivenSellInBetweenTenAndFiveShouldDecreaseSellInByOneAndIncreaseQualityByTwo()
    {
        var items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 9, Quality = 5 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(8));
        Assert.That(items[0].Quality, Is.EqualTo(7));
    }

    [Test]
    public void UpdateQuality_SpecialItem_BackstagePass_GivenSellLessThanFiveShouldDecreaseSellInByOneAndIncreaseQualityByThree()
    {
        var items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 4, Quality = 5 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(3));
        Assert.That(items[0].Quality, Is.EqualTo(8));
    }

    [Test]
    public void UpdateQuality_SpecialItem_BackstagePass_GivenSellLessThanZeroShouldDecreaseSellInByOneAndSetQualityToZero()
    {
        var items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 5 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(-1));
        Assert.That(items[0].Quality, Is.EqualTo(0));
    }
}