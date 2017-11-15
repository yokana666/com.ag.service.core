using Com.DanLiris.Service.Core.Lib.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Com.DanLiris.Service.Core.Lib.Helpers;
using Newtonsoft.Json;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class BuyerService : StandardEntityService<CoreDbContext, Buyer>, IGeneralService<Buyer, BuyerViewModel>
    {
        public BuyerService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Tuple<List<Buyer>, int, Dictionary<string, string>, List<string>> Read(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Buyer> Query = this.DbContext.Buyers;
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            /* Search With Keyword */
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                    {
                        "Name"
                    };

                Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
            }

            /* Const Select */
            List<string> SelectedFields = new List<string>()
                {
                    "_id", "code", "name", "address", "city", "country", "contact", "tempo"
                };

            Query = Query
                .Select(b => new Buyer
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Address = b.Address,
                    City = b.City,
                    Country = b.Country,
                    Contact = b.Contact,
                    Tempo = b.Tempo
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
            Pageable<Buyer> pageable = new Pageable<Buyer>(Query, Page - 1, Size);
            List<Buyer> Data = pageable.Data.ToList<Buyer>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public BuyerViewModel MapToViewModel(Buyer buyer)
        {
            BuyerViewModel buyerVM = new BuyerViewModel();

            buyerVM._id = buyer.Id;
            buyerVM._deleted = buyer._IsDeleted;
            buyerVM._active = buyer.Active;
            buyerVM._createdDate = buyer._CreatedUtc;
            buyerVM._createdBy = buyer._CreatedBy;
            buyerVM._createAgent = buyer._CreatedAgent;
            buyerVM._updatedDate = buyer._LastModifiedUtc;
            buyerVM._updatedBy = buyer._LastModifiedBy;
            buyerVM._updateAgent = buyer._LastModifiedAgent;
            buyerVM.code = buyer.Code;
            buyerVM.name = buyer.Name;
            buyerVM.address = buyer.Address;
            buyerVM.city = buyer.City;
            buyerVM.country = buyer.Country;
            buyerVM.contact = buyer.Contact;
            buyerVM.tempo = buyer.Tempo;
            buyerVM.type = buyer.Type;
            buyerVM.NPWP = buyer.NPWP;

            return buyerVM;
        }

        public Buyer MapToModel(BuyerViewModel buyerVM)
        {
            Buyer buyer = new Buyer();

            buyer.Id = buyerVM._id;
            buyer._IsDeleted = buyerVM._deleted;
            buyer.Active = buyerVM._active;
            buyer._CreatedUtc = buyerVM._createdDate;
            buyer._CreatedBy = buyerVM._createdBy;
            buyer._CreatedAgent = buyerVM._createAgent;
            buyer._LastModifiedUtc = buyerVM._updatedDate;
            buyer._LastModifiedBy = buyerVM._updatedBy;
            buyer._LastModifiedAgent = buyerVM._updateAgent;
            buyer.Code = buyerVM.code;
            buyer.Name = buyerVM.name;
            buyer.Address = buyerVM.address;
            buyer.City = buyerVM.city;
            buyer.Country = buyerVM.country;
            buyer.Contact = buyerVM.contact;
            buyer.Tempo = buyerVM.tempo;
            buyer.Type = buyerVM.type;
            buyer.NPWP = buyerVM.NPWP;

            return buyer;
        }
    }
}