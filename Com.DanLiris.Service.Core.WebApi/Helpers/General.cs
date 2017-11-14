using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.WebApi.Helpers
{
    public static class General
    {
        public const int OK_STATUS_CODE = 200;
        public const int CREATED_STATUS_CODE = 201;
        public const int NOT_FOUND_STATUS_CODE = 404;
        public const int BAD_REQUEST_STATUS_CODE = 400;
        public const int INTERNAL_ERROR_STATUS_CODE = 500;

        public const string OK_MESSAGE = "Ok";
        public const string NOT_FOUND_MESSAGE = "Data Not Found";
        public const string BAD_REQUEST_MESSAGE = "Data does not pass validation";
        public const string INTERNAL_ERROR_MESSAGE = "Internal Server Error";
        public const string CSV_ERROR_MESSAGE = "The header row of CSV file is not valid";

        public const string ASCENDING = "asc";
        public const string DESCENDING = "desc";

        public static string TransformOrderBy(string Order)
        {
            List<string> Transform = new List<string> {
                "_id", "_deleted", "_active",
                "_createdDate", "_createdBy", "_createAgent",
                "_updatedDate", "_updatedBy", "_updateAgent"
            };

            if(Transform.FindIndex(t => string.Equals(Order, t, StringComparison.OrdinalIgnoreCase)) > -1)
            {
                /* Exists in the list */

                string TransformOrder = "";

                switch(Order)
                {
                    case "_deleted": TransformOrder = "_IsDeleted"; break;
                    case "_active": TransformOrder = "Active"; break;
                    case "_createdDate": TransformOrder = "_CreatedUtc"; break;
                    case "_createdBy": TransformOrder = "_CreatedBy"; break;
                    case "_createdAgent": TransformOrder = "_CreatedAgent"; break;
                    case "_updatedDate": TransformOrder = "_LastModifiedUtc"; break;
                    case "_updatedBy": TransformOrder = "_LastModifiedBy"; break;
                    case "_updateAgent": TransformOrder = "_LastModifiedAgent"; break;
                }

                return TransformOrder;
            }
            else
            {
                return Order;
            }
        }

        public static string BuildSearch(List<string> SearchAttributes, string Keyword)
        {
            string SearchQuery = String.Empty;
            foreach (string Attribute in SearchAttributes)
            {
                SearchQuery = string.Concat(SearchQuery, Attribute, ".Contains(@0) OR ");
            }

            SearchQuery = SearchQuery.Remove(SearchQuery.Length - 4);

            return SearchQuery;
        }
    }
}
