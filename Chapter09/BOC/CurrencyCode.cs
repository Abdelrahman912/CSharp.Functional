namespace Chapter09.BOC
{
    public struct CurrencyCode
    {
        public string Value { get; }
        public CurrencyCode(string value) => Value = value;

        public static implicit operator CurrencyCode(string value) => new CurrencyCode(value);
        public static implicit operator string(CurrencyCode currencyCode) => currencyCode.Value;

        override public string ToString() => Value;
        
    }
}
