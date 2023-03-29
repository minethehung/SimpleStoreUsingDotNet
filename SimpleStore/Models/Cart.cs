namespace SimpleStore.Models
{
    public class Cart 
    {
        public Dictionary<int, int> shoppingCart { get; set; }
        public Cart() { 
            shoppingCart = new Dictionary<int, int>();
        }
        public void AddToCart(int shoppingCartId, int quantity, int max)
        {
            if (shoppingCart.ContainsKey(shoppingCartId))
            {
                shoppingCart[shoppingCartId] += quantity;
            }
            else
            {
                shoppingCart.Add(shoppingCartId, quantity);
            }
            if (shoppingCart[shoppingCartId] < 1)
            {
                shoppingCart[shoppingCartId] = 1;
            }
            if (shoppingCart[shoppingCartId]  > max)
            {
                shoppingCart[shoppingCartId] = max;
            }
        }
        public void RemoveFromCart(int shoppingCartId)
        {
            if (shoppingCart.ContainsKey(shoppingCartId))
            {
                shoppingCart.Remove(shoppingCartId);
            }
        }
    }
}
