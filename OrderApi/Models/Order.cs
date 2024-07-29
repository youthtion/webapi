namespace OrderApi.Models;

public interface IName
{
    bool check(string _name);
    string ErrorText { get; set; }
}
public class EnglishName:IName
{
    public EnglishName(){
        ErrorText = "";
    }
    public string ErrorText { get; set; }
    public bool check(string _name)
    {
        for(int i = 0; i < _name.Length; i++){
            if(i == 0 || _name[i - 1] == ' '){
                if(_name[i] < 'A' || _name[i] > 'Z'){
                    ErrorText = "Name is not capitalized\n";
                    return false;
                }
            }
            else{
                if((_name[i] < 'a' || _name[i] > 'z') && _name[i] != ' '){
                    ErrorText = "Name contains non-English characters\n";
                    return false;
                }
            }
        }
        return true;
    }
}
public class NameDetector
{
    public static IName createName(string _language)
    {
        if(_language == "English"){
            return new EnglishName();
        }
        else{
            throw new NotImplementedException();
        }

    }
}
public class OrderManager
{
    public OrderManager(OrderData _order, string _language){
        Order = _order;
        Exchanger = new CurrencyConverter();
        ErrorText = "";
        PriceLimit = 2000;
        IName = NameDetector.createName(_language);
    }

    private OrderData Order;
    private CurrencyConverter Exchanger;
    private String ErrorText;
    private int PriceLimit;
    private IName IName;
    
    public OrderData getOrderData() { return Order; }
    public void exchangeCurrency(string _target)
    {
        var currency = Order.Currency;
        var price = Order.Price;
        if(!Exchanger.exchange(ref currency, ref price, _target)){
            ErrorText = ErrorText + "Currency format is wrong\n";
        }
        Order.Currency = currency;
        Order.Price = price;
    }
    public void checkPriceLimit()
    {
        if(Order.Currency != "TWD"){
            exchangeCurrency("TWD");
        }
        if(Order.Price > PriceLimit){
            ErrorText = ErrorText + "Price is over " + PriceLimit + " TWD\n";
        }
    }
    public void checkName()
    {
        IName.check(Order.Name);
        ErrorText = ErrorText + IName.ErrorText;
    }
    public string getErrorText() { return ErrorText; }
}
public class OrderData
{
    public OrderData(){
        Id = "";
        Name = "";
        Price = 0;
        Currency = "";
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public struct AddressItem{
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }

        public AddressItem(){
            City = "";
            District = "";
            Street = "";
        }
    }
    public AddressItem Address { get; set; }
    public int Price { get; set; }
    public string Currency { get; set; }

    public bool checkInput()
    {
        return (Id.Length > 0 && Name.Length > 0 && Currency.Length > 0 && Address.City.Length > 0 && Address.District.Length > 0 && Address.Street.Length > 0 && Price >= 0);
    }
}
public class CurrencyConverter
{
    public CurrencyConverter(){
        Rate = new Dictionary<string, double>();
        Rate.Add("TWD", 1);
        Rate.Add("USD", 31);
    }
    
    private Dictionary<string, double> Rate;

    public bool exchange(ref string _currency, ref int _price, string _target)
    {
        double target_rate = 0;
        double find_rate = 0;
        foreach(KeyValuePair<string, double> it in Rate){
            if(it.Key == _currency){
                find_rate = it.Value;
            }
            if(it.Key == _target){
                target_rate = it.Value;
            }
        }
        if(target_rate > 0 && find_rate > 0){
            _currency = _target;
            _price = (int)((double)_price * find_rate / target_rate);
            return true;
        }
        return false;
    }
}
