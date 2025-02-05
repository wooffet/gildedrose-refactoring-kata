using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Test]
    public void UpdateQuality_Item_NameIsUnmodified()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Name, Is.EqualTo("foo"));
    }

    [Test]
    public void UpdateQuality_NormalItem_DecreaseSellInAndQualityByOne()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 2, Quality = 2 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(1));
        Assert.That(items[0].Quality, Is.EqualTo(1));
    }

    [Test]
    public void UpdateQuality_NormalItem_DecreaseSellInByOneAndQualityByTwo()
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
}