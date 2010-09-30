using BidsForKids.Controllers;
using Machine.Specifications;

namespace BidsForKids.Tests.Controllers
{
    public abstract class with_an_admin_controller
    {
        private static AdminController controller;

        private Establish context = () =>
            controller = new AdminController();
    }
}