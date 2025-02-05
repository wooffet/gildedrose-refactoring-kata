using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Test]
    [TestCase("foo")]
    [TestCase("bar")]
    [TestCase("baz")]
    public void UpdateQuality_Item_NameShouldBeUnmodified(string name)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = 0, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Name, Is.EqualTo(name));
    }

    [Test]
    [TestCase("foo", 2, 2, 1, 1)]
    [TestCase("bar", 10, 9, 9, 8)]
    [TestCase("baz", 1, 0, 0, 0)]
    public void UpdateQuality_NormalItem_ShouldDecreaseSellInAndQualityByOne(string name, int sellIn, int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
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
    public void UpdateQuality_SpecialItem_BackstagePass_GivenSellInLessThanFiveShouldDecreaseSellInByOneAndIncreaseQualityByThree()
    {
        var items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 4, Quality = 5 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(3));
        Assert.That(items[0].Quality, Is.EqualTo(8));
    }

    [Test]
    public void UpdateQuality_SpecialItem_BackstagePass_GivenSellInLessThanZeroShouldDecreaseSellInByOneAndSetQualityToZero()
    {
        var items = new List<Item> { new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 5 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(-1));
        Assert.That(items[0].Quality, Is.EqualTo(0));
    }

    [Test]
    public void UpdateQuality_SpecialItem_ConjuredItem_GivenSellInExceedsZeroShouldDecreaseSellInByOneAndQualityByTwo()
    {
        var items = new List<Item> { new Item { Name = "Conjured Mana Cake", SellIn = 4, Quality = 6 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(3));
        Assert.That(items[0].Quality, Is.EqualTo(4));
    }

    [Test]
    public void UpdateQuality_SpecialItem_ConjuredItem_GivenSellInLessThanZeroShouldDecreaseSellInByOneAndQualityByFour()
    {
        var items = new List<Item> { new Item { Name = "Conjured Mana Cake", SellIn = -1, Quality = 6 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(-2));
        Assert.That(items[0].Quality, Is.EqualTo(2));
    }
}