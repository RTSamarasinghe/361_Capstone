using DataContracts;
public interface ICategoryAccessor
{
    int AddCategory(string name);
    Category GetCategory(int id);
    List<Category> GetAllCategories();
    void UpdateCategory(int id, string name);
    void DeleteCategory(int id);
}