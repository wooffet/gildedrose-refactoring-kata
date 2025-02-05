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
    [TestCase("Elixir of the Mongoose", 1, 3, 0, 2)]
    public void UpdateQuality_NormalItem_ShouldDecreaseSellInAndQualityByOne(string name, int sellIn, int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    [TestCase("foo", 0, 5, -1, 3)]
    [TestCase("bar", -3, 9, -4, 7)]
    [TestCase("baz", -1, 2, -2, 0)]
    public void UpdateQuality_NormalItem_ShouldDecreaseSellInByOneAndQualityByTwo(string name, int sellIn, int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
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
    [TestCase("Aged foo", 5, 5, 4, 6)]
    [TestCase("Aged bar", 10, 9, 9, 10)]
    [TestCase("Aged baz", 1, 0, 0, 1)]
    public void UpdateQuality_SpecialItem_Aged_ShouldDecreaseSellInAndIncreaseQualityByOne(string name, int sellIn, int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    [TestCase("foo", 2, 55, 1, 50)]
    [TestCase("Aged bar", 2, 50, 1, 50)]
    public void UpdateQuality_Item_QualityShouldNotExceedFifty(string name, int sellIn, int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    [TestCase("Backstage passes to a TAFKAL80ETC concert", 11, 24, 10, 25)]
    [TestCase("Backstage pass bar", 12, 9, 11, 10)]
    [TestCase("Backstage pass baz", 14, 0, 13, 1)]
    public void UpdateQuality_SpecialItem_BackstagePass_GivenSellInExceedsTenShouldDecreaseSellInAndIncreaseQualityByOne(string name, int sellIn, 
        int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    [TestCase("Backstage passes to a TAFKAL80ETC concert", 6, 33, 5, 35)]
    [TestCase("Backstage pass bar", 8, 9, 7, 11)]
    [TestCase("Backstage pass baz", 10, 0, 9, 2)]
    public void UpdateQuality_SpecialItem_BackstagePass_GivenSellInBetweenTenAndFiveShouldDecreaseSellInByOneAndIncreaseQualityByTwo(string name, int sellIn, 
        int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    [TestCase("Backstage passes to a TAFKAL80ETC concert", 4, 5, 3, 8)]
    [TestCase("Backstage pass bar", 3, 9, 2, 12)]
    [TestCase("Backstage pass baz", 5, 0, 4, 3)]
    public void UpdateQuality_SpecialItem_BackstagePass_GivenSellInLessThanFiveShouldDecreaseSellInByOneAndIncreaseQualityByThree(string name, int sellIn,
        int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
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
    [TestCase("Conjured Mana Cake", 4, 6, 3, 4)]
    [TestCase("Conjured bar", 10, 9, 9, 7)]
    [TestCase("Conjured baz", 2, 4, 1, 2)]
    public void UpdateQuality_SpecialItem_ConjuredItem_GivenSellInExceedsZeroShouldDecreaseSellInByOneAndQualityByTwo(string name, int sellIn,
        int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }

    [Test]
    [TestCase("Conjured Mana Cake", -1, 6, -2, 2)]
    [TestCase("Conjured bar", -10, 9, -11, 5)]
    [TestCase("Conjured baz", 0, 4, -1, 0)]
    public void UpdateQuality_SpecialItem_ConjuredItem_GivenSellInLessThanZeroShouldDecreaseSellInByOneAndQualityByFour(string name, int sellIn,
        int quality, int expectedSellIn, int expectedQuality)
    {
        var items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].SellIn, Is.EqualTo(expectedSellIn));
        Assert.That(items[0].Quality, Is.EqualTo(expectedQuality));
    }
}