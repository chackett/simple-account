using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Repositories
{
    public class AccountRepositoryMemory : IRepository<List<Account>, string>
    {
        private readonly Dictionary<string, List<Account>> _db;

        public AccountRepositoryMemory()
        {
            _db = new Dictionary<string, List<Account>>();
        }

        public List<List<Account>> GetAll(string userId)
        {
            var result = new List<List<Account>>();
            result.Add(_db[userId]);

            return result;
        }

        public List<Account> Get(string userId)
        {
            _db.TryGetValue(userId, out var result);

            return result;
        }

        public void Add(string userId, List<Account> account)
        {
            _db[userId] = account;
        }

        public void Delete(string userId)
        {
            _db.Remove(userId);
        }

        public void Update(string userId, List<Account> account)
        {
            _db[userId] = account;
        }
    }
}