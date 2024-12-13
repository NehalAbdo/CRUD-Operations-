using System.ComponentModel;

namespace Demo.PL.ViewModels
{
    public class RoleVM
    {
        public String Id { get; set; }
        [DisplayName("Role")]
        public string Name { get; set; }
        public RoleVM() 
        {
            Id = Guid.NewGuid().ToString();
        } 
    }
}
