namespace StonkBot.Entities
{
    public class Token
    {
        public TokenFinance Finance { get; }
        public TokenInventory Inventory { get;  }

        public Token(TokenFinance finance, TokenInventory inventory)
        {
            Finance = finance;
            Inventory = inventory;
        }
    }
}
