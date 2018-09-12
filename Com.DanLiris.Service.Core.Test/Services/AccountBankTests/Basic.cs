using Com.Danliris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Services.AccountBankTests
{
    [Collection("ServiceProviderFixture Collection")]
    public class Basic : BasicServiceTest<CoreDbContext, AccountBankService, AccountBank>
    {
        private static readonly string[] createAttrAssertions = { "Code", "AccountName" };
        private static readonly string[] updateAttrAssertions = { "Code", "AccountName" };
        private static readonly string[] existAttrCriteria = { "Code" };

        public Basic(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(AccountBank model)
        {
            model.Code = string.Empty;
            model.AccountName = string.Empty;
        }

        public override void EmptyUpdateModel(AccountBank model)
        {
            model.Code = string.Empty;
            model.AccountName = string.Empty;
        }

        public override AccountBank GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new AccountBank()
            {
                Code = guid,
                AccountName = "TEST BANK",
                BankName = "TEST BANK",
                AccountNumber = "TEST BANK",
                BankAddress = "TEST BANK",
                CurrencyCode = "TEST BANK",
                CurrencyDescription = "TEST BANK",
                DivisionCode = "TEST BANK",
                DivisionName = "TEST BANK",
                Fax = "TEST BANK",
                Phone = "TEST BANK",
                CurrencySymbol = "TEST BANK",
                CurrencyRate=1,
                SwiftCode = "TEST BANK",
            };
        }
    }
}