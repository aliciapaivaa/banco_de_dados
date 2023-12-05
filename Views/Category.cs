namespace Views;
public class Category
{
    public struct CategoryElement
    {
        public string name;
        public Guid guid;
    }
    public List<CategoryElement> parents {get; set;} = new List<CategoryElement>();
    public Guid guid {get; set;}
    public string name {get; set;}
}