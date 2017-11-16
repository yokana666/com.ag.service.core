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
    public class UomService : StandardEntityService<CoreDbContext, Uom>, IGeneralService<Uom, UomViewModel>
    {
        public UomService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Tuple<List<Uom>, int, Dictionary<string, string>, List<string>> Read(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Uom> Query = this.DbContext.UnitOfMeasurements;
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            /* Search With Keyword */
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                    {
                        "Unit"
                    };

                Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
            }

            /* Const Select */
            List<string> SelectedFields = new List<string>()
                {
                    "_id", "unit"
                };

            Query = Query
                .Select(u => new Uom
                {
                    Id = u.Id,
                    Unit = u.Unit
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
            Pageable<Uom> pageable = new Pageable<Uom>(Query, Page - 1, Size);
            List<Uom> Data = pageable.Data.ToList<Uom>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public UomViewModel MapToViewModel(Uom uom)
        {
            UomViewModel uomVM = new UomViewModel();

            uomVM._id = uom.Id;
            uomVM._deleted = uom._IsDeleted;
            uomVM._active = uom.Active;
            uomVM._createdDate = uom._CreatedUtc;
            uomVM._createdBy = uom._CreatedBy;
            uomVM._createAgent = uom._CreatedAgent;
            uomVM._updatedDate = uom._LastModifiedUtc;
            uomVM._updatedBy = uom._LastModifiedBy;
            uomVM._updateAgent = uom._LastModifiedAgent;
            uomVM.unit = uom.Unit;

            return uomVM;
        }

        public Uom MapToModel(UomViewModel uomVM)
        {
            Uom uom = new Uom();

            uom.Id = uomVM._id;
            uom._IsDeleted = uomVM._deleted;
            uom.Active = uomVM._active;
            uom._CreatedUtc = uomVM._createdDate;
            uom._CreatedBy = uomVM._createdBy;
            uom._CreatedAgent = uomVM._createAgent;
            uom._LastModifiedUtc = uomVM._updatedDate;
            uom._LastModifiedBy = uomVM._updatedBy;
            uom._LastModifiedAgent = uomVM._updateAgent;
            uom.Unit = uomVM.unit;

            return uom;
        }
    }
}