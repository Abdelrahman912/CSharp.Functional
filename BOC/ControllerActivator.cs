using BOC.Controllers;
using BOC.Core.Commands;
using BOC.Core.Data;
using BOC.Core.Domain;
using BOC.Core.Extensions;
using CSharp.Functional.Constructs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace BOC
{
    public class ControllerActivator : IControllerActivator
    {
        public IEventStore _eventStore { get; set; }
        public ControllerActivator(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public object Create(ControllerContext context)
        {
            var type = context.ActionDescriptor.ControllerTypeInfo;
            if (type.AsType().Equals(typeof(MakeTransferController)))
                return ConfigureMakeTransferController();
            if (type.AsType().Equals(typeof(CreateAccountController)))
                return ConfigureCreateAccountController();
            if (type.AsType().Equals(typeof(DepositCashController)))
                return ConfigureDepositCashController();
            else return null;
        }

        private MakeTransferController ConfigureMakeTransferController()
        {
            Func<Guid, AccountState> getAccount = id =>
            {
                var events = _eventStore.GetEvents(id);
                var opt = events.From();
                return opt.Match(()=>null,state => state);
            };
            
            Func<MakeTransfer, Validation<MakeTransfer>> _validate = cmd => cmd;
            var controller = new MakeTransferController(getAccount, _eventStore.Persist,_validate);
            return controller;
        }
        private DepositCashController ConfigureDepositCashController()
        {
            Func<Guid, AccountState> getAccount = id =>
            {
                var events = _eventStore.GetEvents(id);
                var opt = events.From();
                return opt.Match(() => null, state => state);
            };

            Func<DepositCash, Validation<DepositCash>> _validate = cmd => cmd;
            var controller = new DepositCashController(getAccount, _eventStore.Persist, _validate);
            return controller;
        }

        private CreateAccountController ConfigureCreateAccountController()
        {
           
            Func<CreateAccount, Validation<CreateAccount>> _validate = cmd => cmd;
            var controller = new CreateAccountController( _eventStore.Persist, _validate);
            return controller;
        }
        public void Release(ControllerContext context, object controller)
        {
            var disposable = controller as IDisposable;
            if (disposable != null) disposable.Dispose();
        }
    }
}
