using Com.DanLiris.Service.Core.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Com.DanLiris.Service.Core.Lib.Helpers;
using Newtonsoft.Json;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.Lib.Interfaces;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class AccountBankService : BasicService<CoreDbContext, AccountBank>, IMap<AccountBank, AccountBankViewModel>
    {
        public AccountBankService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<AccountBank>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<AccountBank> Query = this.DbContext.AccountBanks;
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            /* Search With Keyword */
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                {
                    "BankName", "BankAddress", "AccountName", "AccountNumber"
                };

                Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
            }

            /* Const Select */
            List<string> SelectedFields = new List<string>()
            {
                "_id", "code", "bankName", "bankAddress", "accountName", "accountNumber", "swiftCode", "currency"
            };

            Query = Query
                .Select(a => new AccountBank
                {
                    Id = a.Id,
                    Code = a.Code,
                    BankName = a.BankName,
                    BankAddress = a.BankAddress,
                    AccountName = a.AccountName,
                    AccountNumber = a.AccountNumber,
                    SwiftCode = a.SwiftCode,
                    CurrencyId = a.CurrencyId,
                    CurrencyCode = a.CurrencyCode
                });

            /* Order */
            if (OrderDictionary.Count.Equals(0))
            {
                OrderDictionary.Add("_updatedDate", General.DESCENDING);

                Query = Query.OrderByDescending(b => b._LastModifiedUtc); /* Default Order */
            }
            else
            {
                string Key = OrderDictionary.Keys.First();
                string OrderType = OrderDictionary[Key];
                string TransformKey = General.TransformOrderBy(Key);

                BindingFlags IgnoreCase = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

                Query = OrderType.Equals(General.ASCENDING) ?
                    Query.OrderBy(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b)) :
                    Query.OrderByDescending(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b));
            }

            /* Pagination */
            Pageable<AccountBank> pageable = new Pageable<AccountBank>(Query, Page - 1, Size);
            List<AccountBank> Data = pageable.Data.ToList<AccountBank>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public AccountBankViewModel MapToViewModel(AccountBank accountBank)
        {
            AccountBankViewModel accountBankVM = new AccountBankViewModel();
            accountBankVM.currency = new AccountBankCurrencyViewModel();

            accountBankVM._id = accountBank.Id;
            accountBankVM._deleted = accountBank._IsDeleted;
            accountBankVM._active = accountBank.Active;
            accountBankVM._createdDate = accountBank._CreatedUtc;
            accountBankVM._createdBy = accountBank._CreatedBy;
            accountBankVM._createAgent = accountBank._CreatedAgent;
            accountBankVM._updatedDate = accountBank._LastModifiedUtc;
            accountBankVM._updatedBy = accountBank._LastModifiedBy;
            accountBankVM._updateAgent = accountBank._LastModifiedAgent;
            accountBankVM.code = accountBank.Code;
            accountBankVM.bankName = accountBank.BankName;
            accountBankVM.bankAddress = accountBank.BankAddress;
            accountBankVM.accountName = accountBank.AccountName;
            accountBankVM.accountNumber = accountBank.AccountNumber;
            accountBankVM.swiftCode = accountBank.SwiftCode;
            accountBankVM.currency._id = accountBank.CurrencyId;
            accountBankVM.currency.code = accountBank.CurrencyCode;

            return accountBankVM;
        }

        public AccountBank MapToModel(AccountBankViewModel accountBankVM)
        {
            AccountBank accountBank = new AccountBank();

            accountBank.Id = accountBankVM._id;
            accountBank._IsDeleted = accountBankVM._deleted;
            accountBank.Active = accountBankVM._active;
            accountBank._CreatedUtc = accountBankVM._createdDate;
            accountBank._CreatedBy = accountBankVM._createdBy;
            accountBank._CreatedAgent = accountBankVM._createAgent;
            accountBank._LastModifiedUtc = accountBankVM._updatedDate;
            accountBank._LastModifiedBy = accountBankVM._updatedBy;
            accountBank._LastModifiedAgent = accountBankVM._updateAgent;
            accountBank.Code = accountBankVM.code;
            accountBank.BankName = accountBankVM.bankName;
            accountBank.BankAddress = accountBankVM.bankAddress;
            accountBank.AccountName = accountBankVM.accountName;
            accountBank.AccountNumber = accountBankVM.accountNumber;
            accountBank.SwiftCode = accountBankVM.swiftCode;

            if(!Equals(accountBankVM.currency, null))
            {
                accountBank.CurrencyId = accountBankVM.currency._id;
                accountBank.CurrencyCode = accountBankVM.currency.code;
            }
            else
            {
                accountBank.CurrencyId = null;
                accountBank.CurrencyCode = null;
            }

            return accountBank;
        }

        public override void OnCreating(AccountBank model)
        {
            CodeGenerator codeGenerator = new CodeGenerator();

            do
            {
                model.Code = codeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }
    }
}