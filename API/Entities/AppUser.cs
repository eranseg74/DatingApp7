namespace API.Entities
{
    public class AppUser
    {
        //The Id name is a convension. If another name is used (such as TheId or anything else) we would have to declare this field
        // as a primary key by adding the [Key] annotation above the field.
        // Since we are using the Id convension, entity framework identifies it as a primary key
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}