using DataContracts;

public class CategoryManager : ICategoryManager
{
    private readonly ICategoryEngine _categoryEngine;

    public CategoryManager(ICategoryEngine categoryEngine)
    {
        _categoryEngine = categoryEngine;
    }

    public int AddCategory(string name)
    {
        return _categoryEngine.AddCategory(name);
    }

    public Category GetCategory(int id)
    {
        return _categoryEngine.GetCategory(id);
    }

    public List<Category> GetAllCategories()
    {
        return _categoryEngine.GetAllCategories();
    }

    public void UpdateCategory(int id, string name)
    {
        _categoryEngine.UpdateCategory(id, name);
    }

    public void DeleteCategory(int id)
    {
        _categoryEngine.DeleteCategory(id);
    }
}