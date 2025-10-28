using System;
namespace TestDISC.Models.UtilsProject
{
    public class ObjectResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

