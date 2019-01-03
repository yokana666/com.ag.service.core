using Com.Danliris.Service.Core.Mongo.MongoModels;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class ProductSPPProperty : StandardEntity, IValidatableObject
    {
        public ProductSPPProperty()
        {
        }

        public ProductSPPProperty(ProductMongo mongoProduct)
        {
            Active = mongoProduct._active;
            _CreatedAgent = mongoProduct._createAgent;
            _CreatedBy = mongoProduct._createdBy;
            _CreatedUtc = mongoProduct._createdDate;
            _DeletedAgent = mongoProduct._updateAgent;
            _DeletedBy = mongoProduct._updatedBy;
            _DeletedUtc = mongoProduct._updatedDate;
            _IsDeleted = mongoProduct._deleted;
            _LastModifiedAgent = mongoProduct._updateAgent;
            _LastModifiedBy = mongoProduct._updatedBy;
            _LastModifiedUtc = mongoProduct._updatedDate;
        }

        public Product Product { get; set; }

        [ForeignKey("Product"), Key]
        public int ProductId { get; set; }

        public int ProductionOrderId { get; set; }
        [StringLength(25)]
        public string ProductionOrderNo { get; set; }
        [StringLength(250)]
        public string DesignCode { get; set; }
        [StringLength(250)]
        public string DesignNumber { get; set; }

        public int OrderTypeId { get; set; }
        [StringLength(25)]
        public string OrderTypeCode { get; set; }
        [StringLength(25)]
        public string OrderTypeName { get; set; }

        public int BuyerId { get; set; }
        [StringLength(250)]
        public string BuyerName { get; set; }
        [StringLength(250)]
        public string BuyerAddress { get; set; }

        [StringLength(250)]
        public string ColorName { get; set; }

        [StringLength(300)]
        public string Construction { get; set; }

        [StringLength(250)]
        public string Lot { get; set; }
        [StringLength(100)]
        public string Grade { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
