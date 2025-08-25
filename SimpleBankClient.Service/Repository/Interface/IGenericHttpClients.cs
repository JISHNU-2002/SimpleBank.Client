namespace SimpleBank.Client.Repository.Interface
{
    public interface IGenericHttpClients
    {
        public Task<T> GetAsync<T>(string address);
        public Task<T> PostAsync<T>(string address, dynamic dynamicRequest);
        public Task<T> PutAsync<T>(string address, dynamic dynamicRequest);
        public Task<T> DeleteAsync<T>(string address);
    }
}
