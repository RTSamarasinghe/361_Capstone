using DataContracts;

public interface ICartManager
{
    int AddCart();
    Cart GetCart(int id);
    void DeleteCart(int id);
}