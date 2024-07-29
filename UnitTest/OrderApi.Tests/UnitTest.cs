using OrderApi.Models;

namespace OrderApi.Tests;

public class Tests
{
    private OrderData TestOrderData;
    private OrderManager TestOrderManager;
    [SetUp]
    public void Setup()
    {
        TestOrderData = new OrderData();
        TestOrderManager = new OrderManager(TestOrderData, "English");
    }

    [TestCase("USD")]
    [TestCase("TWD")]
    public void TestCurrencyFormatCorrect(string _currency)
    {
        TestOrderData.Currency = _currency;
        TestOrderManager.exchangeCurrency("TWD");
        string error_txt = TestOrderManager.getErrorText();
        Assert.IsTrue(error_txt.Length == 0, error_txt);
    }

    [TestCase("JPY")]
    [TestCase("")]
    public void TestCurrencyFormatIncorrect(string _currency)
    {
        TestOrderData.Currency = _currency;
        TestOrderManager.exchangeCurrency("TWD");
        string error_txt = TestOrderManager.getErrorText();
        Assert.IsFalse(error_txt.Length == 0, error_txt);
    }

    [TestCase("USD", 2)]
    [TestCase("USD", -33)]
    [TestCase("TWD", 567)]
    [TestCase("TWD", 0)]
    public void TestCurrencyExchangeCorrect(string _currency, int _price)
    {
        TestOrderData.Currency = _currency;
        TestOrderData.Price = _price;
        TestOrderManager.exchangeCurrency("TWD");
        bool code_correct = TestOrderData.Currency == "TWD";
        bool amount_correct = false;
        if(_currency == "USD" && TestOrderData.Price == _price * 31){
            amount_correct = true;
        }
        else if(_currency == "TWD" && TestOrderData.Price == _price){
            amount_correct = true;
        }
        Assert.IsTrue(code_correct && amount_correct, _currency + $" to TWD, {_price} to {TestOrderData.Price}");
    }

    [TestCase("USD", 0)]
    [TestCase("USD", 789)]
    [TestCase("TWD", -5)]
    [TestCase("TWD", 7)]
    public void TestCurrencyExchangeIncorrect(string _currency, int _price)
    {
        TestOrderData.Currency = _currency;
        TestOrderData.Price = _price;
        TestOrderManager.exchangeCurrency("TWD");
        bool code_correct = TestOrderData.Currency != "TWD";
        bool amount_correct = false;
        if(_currency == "USD" && TestOrderData.Price != _price * 31){
            amount_correct = true;
        }
        else if(_currency == "TWD" && TestOrderData.Price != _price){
            amount_correct = true;
        }
        Assert.IsFalse(code_correct || amount_correct, _currency + $" to TWD, {_price} to {TestOrderData.Price}");
    }

    [TestCase("TWD", 123)]
    [TestCase("TWD", 0)]
    [TestCase("TWD", 2000)]
    [TestCase("USD", 64)]
    [TestCase("USD", -1)]
    public void TestPriceLimit(string _currency, int _price)
    {
        TestOrderData.Currency = _currency;
        TestOrderData.Price = _price;
        TestOrderManager.checkPriceLimit();
        string error_txt = TestOrderManager.getErrorText();
        Assert.IsTrue(error_txt.Length == 0, error_txt);
    }

    [TestCase("TWD", 2001)]
    [TestCase("USD", 70)]
    public void TestPriceLimitOver(string _currency, int _price)
    {
        TestOrderData.Currency = _currency;
        TestOrderData.Price = _price;
        TestOrderManager.checkPriceLimit();
        string error_txt = TestOrderManager.getErrorText();
        Assert.IsFalse(error_txt.Length == 0, error_txt);
    }

    [TestCase("Apple")]
    [TestCase("Apple Pie")]
    [TestCase("Alien Alien Alien")]
    public void TestNameCheck(string _name)
    {
        TestOrderData.Name = _name;
        TestOrderManager.checkName();
        string error_txt = TestOrderManager.getErrorText();
        Assert.IsTrue(error_txt.Length == 0, error_txt);
    }

    [TestCase("apple")]
    [TestCase("ApplePie")]
    [TestCase("Alien AlienAlien")]
    [TestCase("Alie123n")]
    public void TestNameCheckIncorrect(string _name)
    {
        TestOrderData.Name = _name;
        TestOrderManager.checkName();
        string error_txt = TestOrderManager.getErrorText();
        Assert.IsFalse(error_txt.Length == 0, error_txt);
    }
}