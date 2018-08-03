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

        public override Tuple<List<AccountBank>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<AccountBank> Query = this.DbContext.AccountBanks;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            /* Search With Keyword */
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                {
                    "BankName", "BankAddress", "AccountName", "AccountNumber"
                };

                Query = Query.Where(General.BuildSearch(SearchAttributes), Keyword);
            }

            /* Const Select */
            List<string> SelectedFields = new List<string>()
            {
                "Id", "Code", "BankName", "BankAddress", "AccountName", "AccountNumber", "SwiftCode", "Currency"
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
                OrderDictionary.Add("_LastModifiedUtc", General.DESCENDING);

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
            PropertyCopier<AccountBank, AccountBankViewModel>.Copy(accountBank, accountBankVM);

            accountBankVM.Currency = new CurrencyViewModel
            {
                Id = (int)accountBank.CurrencyId,
                Code = accountBank.CurrencyCode,
                Rate = accountBank.CurrencyId,
                Symbol = accountBank.CurrencySymbol
            };

            return accountBankVM;
        }

        public AccountBank MapToModel(AccountBankViewModel accountBankVM)
        {
            AccountBank accountBank = new AccountBank();
            PropertyCopier<AccountBankViewModel, AccountBank>.Copy(accountBankVM, accountBank);

            if (!Equals(accountBankVM.Currency, null))
            {
                accountBank.CurrencyId = accountBankVM.Currency.Id;
                accountBank.CurrencyCode = accountBankVM.Currency.Code;
                accountBank.CurrencyRate = accountBankVM.Currency.Rate; 
                accountBank.CurrencySymbol = accountBankVM.Currency.Symbol;
            }
            else
            {
                accountBank.CurrencyId = null;
                accountBank.CurrencyCode = null;
                accountBank.CurrencyRate = 0;
                accountBank.CurrencySymbol = null;
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