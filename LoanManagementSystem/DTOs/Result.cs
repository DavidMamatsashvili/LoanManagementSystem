namespace LoanManagementSystem.DTOs
{
    public class Result<T>
    {
        public int CustomerId { get; set; }
        public string? CustomerEmail { get; set; }
        public bool IsSuccess { get; init; }
        public T? Data { get; init; }
        public string? Message { get; init; }

        public static Result<T> Success(T data, int id, string email)
        {
            return new Result<T>
            {
                CustomerId = id,
                CustomerEmail = email,
                IsSuccess = true,
                Data = data,
                Message = "Successful operation"
            };
        }

        public static Result<T> Failure(string message)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Message = message
            };
        }
    }
}
