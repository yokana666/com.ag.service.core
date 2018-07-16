using Com.DanLiris.Service.Core.Lib.Helpers;
using Com.DanLiris.Service.Core.Lib.Interfaces;
using Com.DanLiris.Service.Core.Lib.Models.Account_and_Roles;
using Com.DanLiris.Service.Core.Lib.ViewModels.Account_and_Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Services.Account_and_Roles
{
    public class PermissionService : BasicService<CoreDbContext, Permission>, IMap<Permission, PermissionViewModel>
    {
        public PermissionService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Permission MapToModel(PermissionViewModel viewModel)
        {
            Permission model = new Permission();
            PropertyCopier<PermissionViewModel, Permission>.Copy(viewModel, model);
            model.UnitId = viewModel.Unit.Id;
            model.UnitCode = viewModel.Unit.Code;
            model.Unit = viewModel.Unit.Name;
            return model;
        }

        public PermissionViewModel MapToViewModel(Permission model)
        {
            PermissionViewModel viewModel = new PermissionViewModel();
            PropertyCopier<Permission, PermissionViewModel>.Copy(model, viewModel);
            viewModel.Unit.Id = model.UnitId;
            viewModel.Unit.Name = model.Unit;
            viewModel.Unit.Code = model.UnitCode;
            return viewModel;
        }

        public override Tuple<List<Permission>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            throw new NotImplementedException();
        }
    }
}
