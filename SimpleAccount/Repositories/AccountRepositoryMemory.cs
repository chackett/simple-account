using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using SimpleAccount.DTO.Response;

namespace SimpleAccount.Repositories
{
    public class AccountRepositoryMemory : IRepository<List<Account>, string>
    {
        private readonly Dictionary<string, List<Account>> _db;
        private bool unused;
        
        public AccountRepositoryMemory()
        {
            _db = new Dictionary<string, List<Account>>();
            unused = true;
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
            unused = false;
        }

        public void Delete(string userId)
        {
            _db.Remove(userId);
            unused = false;
        }

        public void Update(string userId, List<Account> account)
        {
            _db[userId] = account;
            unused = false;
        }

        public bool Unused()
        {
            return unused;
        }
    }
}