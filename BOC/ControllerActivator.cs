using BOC.Controllers;
using BOC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using static CSharp.Functional.Extensions.ValidationExtension;
using static CSharp.Functional.Functional;

namespace BOC
{
    public class ControllerActivator : IControllerActivator
    {
        public ControllerActivator()
        {

        }
        public object Create(ControllerContext context)
        {
            var type = context.ActionDescriptor.ControllerTypeInfo;
            if (type.AsType().Equals(typeof(MakeTransferController)))
                return ConfigureMakeTransferController();
            else return null;
        }

        private MakeTransferController ConfigureMakeTransferController()
        {
            Validator<Account> validate = (t) => Valid(t);
            var controller = new MakeTransferController(validate);
            return controller;
        }

        public void Release(ControllerContext context, object controller)
        {
            var disposable = controller as IDisposable;
            if (disposable != null) disposable.Dispose();
        }
    }
}
