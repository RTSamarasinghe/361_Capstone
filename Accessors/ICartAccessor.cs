using DataContracts;
public interface ICartAccessor
{
    int AddCart();
    Cart GetCart(int id);
    void DeleteCart(int id);
}