namespace Laptop.Common.DataType
{
    public static class Customer
    {
        public enum Gender : byte
        {
            Male = 0,
            FeMale = 1
        }
        public enum Status : byte
        {
            NotActive = 0,
            Active = 1
        }
    }
}