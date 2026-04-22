using Accessors;

[TestClass]
public class Adhoc {

    [TestMethod]
    public void AddCustomer()
    {
         var accessor = new CustomerAccessor();
        try
        {
            

            int newCustomerId = accessor.AddCustomer("John Doe", "test@test.gmail.com", "hashedpassword");
            Console.WriteLine($"New customer added with ID: {newCustomerId}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
       
    }
}