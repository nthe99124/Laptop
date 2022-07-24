namespace Laptop.Common.DataType
{
    public class Bill
    {
        public enum Status : short
        {
            /// <summary>
            /// Chờ xác nhận
            /// </summary>
            WaitForConfirmation = 0,

            /// <summary>
            /// Đã xác nhận
            /// </summary>
            Confirmed = 1,

            /// <summary>
            /// Đang giao hàng
            /// </summary>
            Delivering = 2,

            /// <summary>
            /// Đã giao hàng
            /// </summary>
            Delivered = 3,

            /// <summary>
            /// Đã nhận được hàng
            /// </summary>
            Received = 4,

            /// <summary>
            /// Đơn hàng đã hủy
            /// </summary>
            Canceled = 5,
        }
    }
}