using System;

namespace ConvenienceStoreManager.Entities
{
    /// <summary>
    /// Lớp đại diện cho thông tin sản phẩm
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Mã sản phẩm (không phải ID, có thể là mã vạch)
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Đơn vị tính (hộp, cái, lon, v.v.)
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Giá bán
        /// </summary>
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// Số lượng tồn kho
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Constructor không tham số
        /// </summary>
        //public Product()
        //{
        //    ProductID = ProductID;
        //    ProductName = string.Empty;
        //    ProductCode = string.Empty;
        //    Unit = string.Empty;
        //    SellingPrice = 0;
        //    StockQuantity = 0;
        //}

        /// <summary>
        /// Constructor đầy đủ tham số
        /// </summary>
        /// 
        public Product() { }

        public Product(int productID, string productName, string productCode, string unit, decimal sellingPrice, int stockQuantity)
        {
            ProductID = productID;
            ProductName = productName;
            ProductCode = productCode;
            Unit = unit;
            SellingPrice = sellingPrice;
            StockQuantity = stockQuantity;
        }
    }
}