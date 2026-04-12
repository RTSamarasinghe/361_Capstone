using DataContracts;

public class CategoryEngine : ICategoryEngine
{
    private readonly ICategoryAccessor _categoryAccessor;

    public CategoryEngine(ICategoryAccessor categoryAccessor)
    {
        _categoryAccessor = categoryAccessor;
    }

    public int AddCategory(string name)
    {
        ValidateCategoryName(name);

        return _categoryAccessor.AddCategory(name.Trim());
    }

    public Category GetCategory(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Category id must be greater than 0.");
        }

        Category category = _categoryAccessor.GetCategory(id);

        if (category == null)
        {
            throw new Exception("Category not found.");
        }

        return category;
    }

    public List<Category> GetAllCategories()
    {
        return _categoryAccessor.GetAllCategories();
    }

    public void UpdateCategory(int id, string name)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Category id must be greater than 0.");
        }

        ValidateCategoryName(name);

        Category existingCategory = _categoryAccessor.GetCategory(id);
        if (existingCategory == null)
        {
            throw new Exception("Category not found.");
        }

        _categoryAccessor.UpdateCategory(id, name.Trim());
    }

    public void DeleteCategory(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Category id must be greater than 0.");
        }

        Category existingCategory = _categoryAccessor.GetCategory(id);
        if (existingCategory == null)
        {
            throw new Exception("Category not found.");
        }

        _categoryAccessor.DeleteCategory(id);
    }

    private static void ValidateCategoryName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name cannot be empty.");
        }
    }
}